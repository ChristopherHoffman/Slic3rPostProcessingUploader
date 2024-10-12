﻿using System.Text;
using System.Text.RegularExpressions;

namespace Slic3rPostProcessingUploader.Services.Parsers
{
    internal class OrcaParser: IGcodeParser
    {
        private readonly string noteTemplate;

        public OrcaParser(string noteTemplate)
        {
            this.noteTemplate = noteTemplate;
        }

        public CuraSettingDto ParseGcode(string gcode)
        {
            var dto = new CuraSettingDto();

            var settings = new CuraSettings();

            settings.estimated_print_time_seconds = ParseEstimatedPrintTime(gcode);

            // Convert double to int
            settings.material_used_mg = (int?)EstimateFilamentUsageInMg(gcode);

            
            // settings.note = ParseSettingsIntoNotes(gcode);
            settings.note = RenderNoteTemplate(gcode);

            string? snapshot = getSnapshot(gcode);
            if (snapshot != null)
            {
                settings.Snapshot = snapshot;
            }

            // Hardcode the Cura version
            dto.Slicer = "OrcaSlicer";
            dto.CuraVersion = GetSlicerVersion(gcode);
            dto.settings = settings;

            return dto;
        }

        /// <summary>
        /// The note template will have placeholders that will be replaced with the actual values from the gcode.
        /// </summary>
        /// <param name="gcode"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private string RenderNoteTemplate(string gcode)
        {
            // Given the this.noteTemplate string which contains placeholders wrapped in {{ and }}, replace the placeholders with the actual values from the gcode.
            // For example, if the noteTemplate is "This is a {{test}}", and the gcode contains "test = hello", the output should be "This is a hello".

            if (string.IsNullOrEmpty(this.noteTemplate))
            {
                return ParseSettingsIntoNotes(gcode);
            }

            string template = this.noteTemplate;

            // Find each placeholder in the noteTemplate
            var matches = Regex.Matches(template, "{{(.*?)}}");
            if (matches != null) {
                foreach (Match match in matches)
                {
                    // Get the placeholder name
                    var placeholder = match.Groups[1].Value;

                    // Convert into orca-slicers format
                    string searchString = "; " + placeholder;

                    // Find the value of the placeholder in the gcode
                    var value = ParseSettingAsString(gcode, searchString);
                    // Replace the placeholder with the value
                    template = template.Replace( match.Value, value);
                }
            }

            return template;
        }

        private string GetSlicerVersion(string gcode)
        {
            // ; generated by OrcaSlicer 2.2.0-rc on

            var printTimeStringMatch = Regex.Match(gcode, @"generated by OrcaSlicer (.+) on", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            if (printTimeStringMatch.Success)
            {
                return printTimeStringMatch.Groups[1].Value.Trim();
            }

            return "Unknown";
        }

        private string? getSnapshot(string gcode)
        {
            var snapshotMatch = Regex.Match(gcode, "thumbnail begin[\\sa-zA-Z\\d]*([\\S\\s]*?); thumbnail end", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            if (snapshotMatch.Success)
            {
                return snapshotMatch.Groups[1].Value.Replace("\n; ", "").Replace(";", "").Trim();
            }
            return null;
        }   

        private int ParseEstimatedPrintTime(string gcode)
        {
            var printTimeStringMatch = Regex.Match(gcode, @"total estimated time: (.+)$", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            if (printTimeStringMatch.Success)
            {
                var time = ParseAsSeconds(printTimeStringMatch.Groups[1].Value);
                if (time.HasValue)
                {
                    return time.Value;
                }
            }

            var normalModePrintTimeMatch = ParseSettingAsString(gcode, "; estimated printing time (normal mode)");

            if (!string.IsNullOrEmpty(normalModePrintTimeMatch))
            {
                // Parse a time formated as 1h 35m 50s into seconds.
                var timeParts = normalModePrintTimeMatch.Split(' ');
                var hours = timeParts.Where(x => x.Contains("h")).Select(x => int.Parse(x.Replace("h", ""))).FirstOrDefault();
                var minutes = timeParts.Where(x => x.Contains("m")).Select(x => int.Parse(x.Replace("m", ""))).FirstOrDefault();
                var seconds = timeParts.Where(x => x.Contains("s")).Select(x => int.Parse(x.Replace("s", ""))).FirstOrDefault();

                return (hours * 3600) + (minutes * 60) + seconds;
            }

            var silentMode = ParseSettingAsString(gcode, "; estimated printing time (silent mode)");
            if (!string.IsNullOrEmpty(silentMode))
            {
                // Parse a time formated as 1h 35m 50s into seconds.
                var timeParts = silentMode.Split(' ');
                var hours = timeParts.Where(x => x.Contains("h")).Select(x => int.Parse(x.Replace("h", ""))).FirstOrDefault();
                var minutes = timeParts.Where(x => x.Contains("m")).Select(x => int.Parse(x.Replace("m", ""))).FirstOrDefault();
                var seconds = timeParts.Where(x => x.Contains("s")).Select(x => int.Parse(x.Replace("s", ""))).FirstOrDefault();

                return (hours * 3600) + (minutes * 60) + seconds;
            }


            return 0;
        }

        private int? ParseAsSeconds(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return null;
            }
            var durationAsMs = int.Parse(input); // Assuming the input is in milliseconds
            var durationAsSeconds = durationAsMs / 1000;
            return (int)Math.Floor((double)durationAsSeconds);
        }

        public double? EstimateFilamentUsageInMg(string gcode)
        {
            if (string.IsNullOrEmpty(gcode))
            {
                return 0;
            }

            // Check to see if the user setup their filament densities, thus we can directly return filament usage.
            var filamentUsedInGrams = ParseSettingAsNumber(gcode, "; filament used \\[g\\]");
            if (filamentUsedInGrams > 0)
            {
                return filamentUsedInGrams * 1000;
            }

            var filamentType = ParseSettingAsString(gcode, "; filament_type");
            // Try and grab the first diameter
            var filamentDiameter = double.Parse(ParseSettingAsString(gcode, "; filament_diameter").Split(',')[0]);
            if (double.IsNaN(filamentDiameter))
            {
                return 0;
            }
            var filamentUsageLengthInMM = double.Parse(ParseSettingAsString(gcode, "; filament used \\[mm\\]"));

            if (double.IsNaN(filamentUsageLengthInMM))
            {
                return 0;
            }

            if (filamentType.Contains("PLA"))
            {
                return CalculateWeightInMg(MaterialDensities.Materials.PLA, filamentUsageLengthInMM, filamentDiameter);
            }
            else if (filamentType.Contains("ABS"))
            {
                return CalculateWeightInMg(MaterialDensities.Materials.ABS, filamentUsageLengthInMM, filamentDiameter);
            }
            else if (filamentType.Contains("PETG"))
            {
                return CalculateWeightInMg(MaterialDensities.Materials.PETG, filamentUsageLengthInMM, filamentDiameter);
            }

            return 0;
        }

        private double CalculateWeightInMg(double materialDensityGramsPerCubicCm, double lengthInMm, double diameterInMm)
        {
            var radiusInMm = diameterInMm / 2;
            var filamentAreaInMm2 = Math.PI * Math.Pow(radiusInMm, 2);

            var volume = filamentAreaInMm2 * lengthInMm;

            var densityInCubicMm = materialDensityGramsPerCubicCm / 1000;

            var weightInGrams = volume * densityInCubicMm;
            return Math.Floor(weightInGrams * 1000);
        }

        private double ParseSettingAsNumber(string gcode, string settingName)
        {
            var regex = new Regex(settingName + " = (.+)$", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            var match = regex.Match(gcode);
            if (match.Success)
            {
                return double.Parse(match.Groups[1].Value);
            }
            return double.NaN;
        }

        private string ParseSettingAsString(string gcode, string settingName)
        {
            var regex = new Regex(settingName.Replace("(", "\\(").Replace(")", "\\)") + " = (.+)$", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            var match = regex.Match(gcode);
            if (match.Success)
            {
                return match.Groups[1].Value.Trim();
            }
            return string.Empty;
        }

        private string ParseSettingsIntoNotes(string gcode)
        {
            var notes = new StringBuilder();

            var settings = new List<string>
    {
        "; first_layer_height",
        "; layer_height",
        "; wall_loops",
        "; top_shell_layers",
        "; bottom_shell_layers",
        "; sparse_infill_density",
        "; perimeter_speed",
        "; infill_speed",
        "; travel_speed",
        "; nozzle_diameter",
        "; filament_type",
        "; filament_diameter",
        "; extrusion_multiplier",
        "; nozzle_temperature",
        "; first_layer_temperature",
        "; bed_temperature",
        "; first_layer_bed_temperature",
        "; fan_always_on",
        "; fan_below_layer_time",
        "; spiral_vase",
        "; brim_width",
        "; support_material",
        "; support_material_threshold",
        "; support_material_enforce_layers",
        "; raft_layers",
        "; total_layer_count",
        "; pressure_advance",
        "; pressure_advance_smooth",
        "; print_settings_id",
        "; printer_settings_id"
    };

            notes.AppendLine("Settings:");
            notes.AppendLine();


            foreach (var setting in settings)
            {
                var settingValue = ParseSettingAsString(gcode, setting);
                if (!string.IsNullOrEmpty(settingValue))
                {
                    // Remove the leading semicolon
                    var cleanedSettingName = setting.Substring(2);

                    notes.AppendLine($"  {cleanedSettingName}: {settingValue}");
                }
            }

            return notes.ToString().TrimEnd().Replace("\r", "");
        }

    }



}

﻿using System.Text.RegularExpressions;

namespace Slic3rPostProcessingUploader.Services.Parsers.AnycubicSlicerNext
{
    internal class AnycubicSlicerNextParser : IGcodeParser
    {
        private readonly string noteTemplate;

        public AnycubicSlicerNextParser(string noteTemplate)
        {
            if (string.IsNullOrEmpty(noteTemplate))
            {
                var defaultTemplate = new AnycubicSlicerNextDefaultNoteTemplate();
                this.noteTemplate = defaultTemplate.getNoteTemplate();
            }
            else
            {
                this.noteTemplate = noteTemplate;
            }
        }

        public CuraSettingDto ParseGcode(string gcode)
        {
            var dto = new CuraSettingDto();

            var settings = new CuraSettings();

            settings.estimated_print_time_seconds = ParseEstimatedPrintTime(gcode);

            settings.filamentUsage = GetFilamentUsage(gcode);

            if (settings.filamentUsage.Count == 0) { 

                // Convert double to int
                settings.material_used_mg = (int?)EstimateFilamentUsageInMg(gcode);
            }


            settings.note = RenderNoteTemplate(gcode);

            string? snapshot = getSnapshot(gcode);
            if (snapshot != null)
            {
                settings.Snapshot = snapshot;
            }

            // Hardcode the Cura version
            dto.Slicer = "AnycubicSlicerNext";
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
            string template = noteTemplate;

            // Find each placeholder in the noteTemplate
            var matches = Regex.Matches(template, "{{(.*?)}}");
            if (matches != null)
            {
                foreach (Match match in matches)
                {
                    // Get the placeholder name
                    var placeholder = match.Groups[1].Value;

                    // Convert into orca-slicers format
                    string searchString = "; " + placeholder;

                    // Find the value of the placeholder in the gcode
                    var value = ParseSettingAsString(gcode, searchString);
                    // Replace the placeholder with the value
                    template = template.Replace(match.Value, value);
                }
            }

            return template;
        }

        /// <summary>
        /// Count the number of placeholders in the note template that are found in the gcode. Used for heuristic matching.
        /// </summary>
        /// <param name="gcode"></param>
        /// <returns>The number of placeholders in the templates, and the number of successful matches for those placeholders</returns>
        public (int numPlaceholders, int numMatches) CountTemplateMatches(string gcode)
        {
            int numPlaceholders = 0;
            int numMatches = 0;

            // Given the this.noteTemplate string which contains placeholders wrapped in {{ and }}, replace the placeholders with the actual values from the gcode.
            // For example, if the noteTemplate is "This is a {{test}}", and the gcode contains "test = hello", the output should be "This is a hello".
            string template = this.noteTemplate;

            // Find each placeholder in the noteTemplate
            var matches = Regex.Matches(template, "{{(.*?)}}");
            if (matches != null)
            {
                foreach (Match match in matches)
                {
                    numPlaceholders++;

                    // Get the placeholder name
                    var placeholder = match.Groups[1].Value;

                    // Convert into orca-slicers format
                    string searchString = "; " + placeholder;

                    // Find the value of the placeholder in the gcode
                    var value = ParseSettingAsString(gcode, searchString);

                    if (!string.IsNullOrEmpty(value))
                    {
                        numMatches++;
                    }
                }
            }

            return (numPlaceholders, numMatches);
        }

        private string GetSlicerVersion(string gcode)
        {
            // ; generated by AnycubicSlicerNext 1.3.2 on 2025-01-07 at 08:31:59

            var printTimeStringMatch = Regex.Match(gcode, @"generated by AnycubicSlicerNext (.+) on", RegexOptions.IgnoreCase | RegexOptions.Multiline);
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
                return snapshotMatch.Groups[1].Value.Replace("\r\n; ", "").Replace("\n; ", "").Replace(";", "").Trim();
            }
            return null;
        }

        private int ParseEstimatedPrintTime(string gcode)
        {
            try { 
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
                    var time = ParseAsSeconds(normalModePrintTimeMatch);
                    if (time.HasValue)
                    {
                        return time.Value;
                    }
                }

                var silentMode = ParseSettingAsString(gcode, "; estimated printing time (silent mode)");
                if (!string.IsNullOrEmpty(silentMode))
                {
                    // Parse a time formated as 1h 35m 50s into seconds.
                    var time = ParseAsSeconds(silentMode);
                    if (time.HasValue)
                    {
                        return time.Value;
                    }
                }


                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return 0;
            }
        }

        private int? ParseAsSeconds(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return null;
            }

            if (int.TryParse(input, out var durationAsMs))
            {
                var durationAsSeconds = durationAsMs / 1000;
                return (int)Math.Floor((double)durationAsSeconds);
            }

            // Try and parse a time formated as 1h 35m 50s into seconds.
            var timeParts = input.Split(' ');
            var hours = timeParts.Where(x => x.Contains("h")).Select(x => int.Parse(x.Replace("h", ""))).FirstOrDefault();
            var minutes = timeParts.Where(x => x.Contains("m")).Select(x => int.Parse(x.Replace("m", ""))).FirstOrDefault();
            var seconds = timeParts.Where(x => x.Contains("s")).Select(x => int.Parse(x.Replace("s", ""))).FirstOrDefault();

            return hours * 3600 + minutes * 60 + seconds;

        }


        private List<PrintFilamentSummaryDto> GetFilamentUsage(string gcode)
        {
            List<PrintFilamentSummaryDto> filament = new List<PrintFilamentSummaryDto>();

            string filamentUsed = ParseSettingAsString(gcode, "; filament used \\[mm\\]");

            if (string.IsNullOrEmpty(filamentUsed))
            {
                return filament;
            }

            List<double> usage = filamentUsed.Split(',')
                                             .Select(x => double.Parse(x.Trim()))
                                             .ToList();

            for (int i = 0; i < usage.Count; i++)
            {
                if (usage[i] == 0)
                {
                    continue;
                }

                PrintFilamentSummaryDto filamentUsage = new PrintFilamentSummaryDto
                {
                    EstimatedSource = PrintFilamentSourceMeasurement.Length,
                    EstimatedLengthInM = Math.Round(usage[i] / 1000, 3),
                    Id = null,
                    Notes = string.Empty,
                    Source = PrintFilamentSourceMeasurement.Length,
                    Filament = new FilamentSummary
                    {
                        DisplayName = "Other",
                        Id = "00000000-0000-0000-0000-000000000000"
                    }
                };

                filament.Add(filamentUsage);
            }

            return filament;
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

        public static bool IsAnycubicSlicerNext(string gcode)
        {
            return gcode.Contains("generated by AnycubicSlicerNext");
        }
    }
}

using Microsoft.ApplicationInsights;
using Slic3rPostProcessingUploader.Services.Parsers.BambuStudio;
using Slic3rPostProcessingUploader.Services.Parsers.FLSunSlicer;
using Slic3rPostProcessingUploader.Services.Parsers.OrcaSlicer;
using Slic3rPostProcessingUploader.Services.Parsers.PrusaSlicer;

namespace Slic3rPostProcessingUploader.Services.Parsers
{
    internal class ParserFactory
    {
        public static IGcodeParser GetParser(ArgumentParser arguments, TelemetryClient telemetryClient, string gcode)
        {
            SendTemplateMetrics(arguments, telemetryClient);

            // search through the gcode to find the slicer used

            if (OrcaParser.IsOrcaSlicer(gcode))
            {
                return BuildOrcaParser(arguments);
            }
            else if (PrusaParser.IsPrusaSlicer(gcode))
            {
                return BuildPrusaParser(arguments);
            }
            else if (FLSunParser.IsFLSunSlicer(gcode))
            {
                return BuildFLSunParser(arguments);
            }
            else if (BambuStudioParser.IsBambuStudio(gcode))
            {
                return BuildBambuStudioParser(arguments);
            }
            else
            {

                // If the slicer is not recognized, then try and parse using all of them and see which one matches more closely
                // This is a fallback mechanism in case the slicer is not recognized

                // Try Orca
                var orcaFullTemplate = new OrcaFullNoteTemplate().getNoteTemplate();
                var orcaParser = new OrcaParser(orcaFullTemplate);
                var orcaResults = orcaParser.CountTemplateMatches(gcode);
                var orcaPercentMatch = orcaResults.numMatches / orcaResults.numPlaceholders;

                telemetryClient.TrackEvent("OrcaPercentMatch", new Dictionary<string, string> { { "PercentMatch", orcaPercentMatch.ToString() } });

                // Try Prusa
                var prusaFullTemplate = new PrusaFullNoteTemplate().getNoteTemplate();
                var prusaParser = new PrusaParser(prusaFullTemplate);
                var prusaResults = prusaParser.CountTemplateMatches(gcode);
                var PrusaPercentMatch = orcaResults.numMatches / orcaResults.numPlaceholders;

                telemetryClient.TrackEvent("PrusaPercentMatch", new Dictionary<string, string> { { "PercentMatch", PrusaPercentMatch.ToString() } });

                // Try FL Sun
                var flsunFullTemplate = new FLSunFullNoteTemplate().getNoteTemplate();
                var flsunParser = new FLSunParser(flsunFullTemplate);
                var flsunResults = flsunParser.CountTemplateMatches(gcode);
                var flsunPercentMatch = flsunResults.numMatches / flsunResults.numPlaceholders;

                telemetryClient.TrackEvent("FLSunPercentMatch", new Dictionary<string, string> { { "PercentMatch", flsunPercentMatch.ToString() } });

                // Try Bambu Studio
                var bambuStudioFullTemplate = new BambuStudioFullNoteTemplate().getNoteTemplate();
                var bambuStudioParser = new BambuStudioParser(bambuStudioFullTemplate);
                var bambuStudioResults = bambuStudioParser.CountTemplateMatches(gcode);
                var bambuStudioPercentMatch = bambuStudioResults.numMatches / bambuStudioResults.numPlaceholders;

                telemetryClient.TrackEvent("BambuStudioPercentMatch", new Dictionary<string, string> { { "PercentMatch", bambuStudioPercentMatch.ToString() } });

                // Compare Results
                if (orcaPercentMatch > PrusaPercentMatch && orcaPercentMatch > flsunPercentMatch && orcaPercentMatch > bambuStudioPercentMatch)
                {
                    return BuildOrcaParser(arguments);
                }
                else if (PrusaPercentMatch > orcaPercentMatch && PrusaPercentMatch > flsunPercentMatch && PrusaPercentMatch > bambuStudioPercentMatch)
                {
                    return BuildPrusaParser(arguments);
                }
                else if (flsunPercentMatch > orcaPercentMatch && flsunPercentMatch > PrusaPercentMatch && flsunPercentMatch > bambuStudioPercentMatch)
                {
                    return BuildFLSunParser(arguments);
                }
                else if (bambuStudioPercentMatch > orcaPercentMatch && bambuStudioPercentMatch > PrusaPercentMatch && bambuStudioPercentMatch > flsunPercentMatch)
                {
                    return BuildBambuStudioParser(arguments);
                }
                else
                {
                    return BuildOrcaParser(arguments);
                }

            }
        }

        private static IGcodeParser BuildPrusaParser(ArgumentParser arguments)
        {
            INoteTemplate template = arguments.UseDefaultNoteTemplate
                            ? new PrusaDefaultNoteTemplate()
                            : arguments.UseFullNoteTemplate
                            ? new PrusaFullNoteTemplate()
                            : new NoteTemplateFromFile(arguments.NoteTemplatePath);

            return new PrusaParser(template.getNoteTemplate());
        }

        private static IGcodeParser BuildOrcaParser(ArgumentParser arguments)
        {
            INoteTemplate template = arguments.UseDefaultNoteTemplate
                            ? new OrcaDefaultNoteTemplate()
                            : arguments.UseFullNoteTemplate
                            ? new OrcaFullNoteTemplate()
                            : new NoteTemplateFromFile(arguments.NoteTemplatePath);

            return new OrcaParser(template.getNoteTemplate());
        }

        private static IGcodeParser BuildFLSunParser(ArgumentParser arguments)
        {
            INoteTemplate template = arguments.UseDefaultNoteTemplate
                            ? new FLSunDefaultNoteTemplate()
                            : arguments.UseFullNoteTemplate
                            ? new FLSunFullNoteTemplate()
                            : new NoteTemplateFromFile(arguments.NoteTemplatePath);
            return new FLSunParser(template.getNoteTemplate());
        }

        private static IGcodeParser BuildBambuStudioParser(ArgumentParser arguments)
        {
            INoteTemplate template = arguments.UseDefaultNoteTemplate
                            ? new BambuStudioDefaultNoteTemplate()
                            : arguments.UseFullNoteTemplate
                            ? new BambuStudioFullNoteTemplate()
                            : new NoteTemplateFromFile(arguments.NoteTemplatePath);
            return new BambuStudioParser(template.getNoteTemplate());
        }

        private static void SendTemplateMetrics(ArgumentParser arguments, TelemetryClient telemetryClient)
        {
            // Track the template used as an event
            if (arguments.UseDefaultNoteTemplate)
            {
                telemetryClient.TrackEvent("Template", new Dictionary<string, string> { { "Template", "Default" } });
            }
            else if (arguments.UseFullNoteTemplate)
            {
                telemetryClient.TrackEvent("Template", new Dictionary<string, string> { { "Template", "Full" } });
            }
            else
            {
                telemetryClient.TrackEvent("Template", new Dictionary<string, string> { { "Template", "Custom" } });
            }
        }
    }
}

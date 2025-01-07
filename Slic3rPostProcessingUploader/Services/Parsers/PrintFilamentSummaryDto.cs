using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slic3rPostProcessingUploader.Services.Parsers
{
    public class PrintFilamentSummaryDto
    {
        /// <summary>
        /// GUID
        /// </summary>
        public string? Id { get; set; }
        public FilamentSummary Filament { get; set; }

        public double? AmountMg { get; set; }
        public double? LengthInM { get; set; }
        public double? VolumeMl { get; set; }

        public double? EstimatedAmountMg { get; set; }
        public double? EstimatedLengthInM { get; set; }
        public double? EstimatedVolumeMl { get; set; }

        public PrintFilamentSourceMeasurement Source { get; set; }
        public PrintFilamentSourceMeasurement EstimatedSource { get; set; }

        public string Notes { get; set; }
    }
}

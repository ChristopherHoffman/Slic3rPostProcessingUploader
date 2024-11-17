namespace Slic3rPostProcessingUploader.Services.Parsers.PrusaSlicer
{
    internal class PrusaDefaultNoteTemplate : INoteTemplate
    {
        public string getNoteTemplate()
        {
            return """
                Settings:

                
                """;
        }
    }
}

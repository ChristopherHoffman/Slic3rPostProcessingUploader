namespace Slic3rPostProcessingUploader.Services
{
    internal class NoteTemplateFromFile : INoteTemplate
    {
        private string filePath;

        public NoteTemplateFromFile(string filePath) {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException("filePath");
            }

            this.filePath = filePath;
        }

        public string getNoteTemplate()
        {
            // Validate the filePath
            if (!System.IO.File.Exists(filePath))
            {
                throw new System.IO.FileNotFoundException("The file does not exist", filePath);
            }

            // Load the contents of the file from the filePath
            return System.IO.File.ReadAllText(filePath);
        }
    }
}

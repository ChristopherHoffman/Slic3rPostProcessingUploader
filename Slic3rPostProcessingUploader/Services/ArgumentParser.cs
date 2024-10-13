namespace Slic3rPostProcessingUploader.Services
{
    /// <summary>
    /// Parses the arguments provided to the program
    /// </summary>
    internal class ArgumentParser
    {
        public string InputFile { get; private set; }
        public bool UseDefaultNoteTemplate { get; private set; }
        public bool UseFullNoteTemplate { get; private set; }

        public string NoteTemplatePath { get; private set; }

        public bool UseLocalDev { get; private set; }

        public string DebugPath { get; private set; }

        public ArgumentParser(string[] args) {
            this.UseDefaultNoteTemplate = true;
            this.UseFullNoteTemplate = false;
            this.NoteTemplatePath = null;
            this.InputFile = args.LastOrDefault();

            // Check for if the user wants a default, full, or custom note template
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "--default")
                {
                    this.UseDefaultNoteTemplate = true;
                    this.UseFullNoteTemplate = false;
                }
                else if (args[i] == "--full")
                {
                    this.UseDefaultNoteTemplate = false;
                    this.UseFullNoteTemplate = true;
                }
                else if (args[i] == "--template")
                {
                    this.UseDefaultNoteTemplate = false;
                    this.UseFullNoteTemplate = false;
                    this.NoteTemplatePath = args[i + 1];

                    if (string.IsNullOrEmpty(this.NoteTemplatePath))
                    {
                        throw new ArgumentNullException("Note template path cannot be null or empty");
                    }

                    if (this.NoteTemplatePath == this.InputFile)
                    {
                        throw new ArgumentException("Note template path cannot be null or empty");
                    }
                }

                if (args[i] == "--local-dev")
                {
                    this.UseLocalDev = true;
                }

                if (args[i] == "--debug")
                {
                    this.DebugPath = args[i + 1];

                    if (this.DebugPath == this.InputFile)
                    {
                        throw new ArgumentException("Note template path cannot be null or empty");
                    }

                    if (string.IsNullOrEmpty(this.DebugPath))
                    {
                        throw new ArgumentNullException("Debug path cannot be null or empty");
                    }

                    if(this.DebugPath.StartsWith("--"))
                    {
                        throw new ArgumentException("Debug path cannot start with --" + this.DebugPath);
                    }
                }
            }



        }
    }
}

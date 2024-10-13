namespace Slic3rPostProcessingUploader.Services
{
    internal class OrcaFullNoteTemplate : INoteTemplate
    {
        public string getNoteTemplate()
        {
            return """
                Full Note Template.

                Settings:
                    Layer Height: {{layer_height}}
                    First Layer Height: {{first_layer_height}}
                    Wall Loops: {{wall_loops}}
                    Top Shell Layers: {{top_shell_layers}}
                    Bottom Shell Layers: {{bottom_shell_layers}}
                    Sparse Infill Density: {{sparse_infill_density}}
                """;
        }
    }
}

﻿namespace Slic3rPostProcessingUploader.Services.Parsers.BambuStudio
{
    internal class BambuStudioDefaultNoteTemplate : INoteTemplate
    {
        public string getNoteTemplate()
        {
            return """
                Settings:

                Quality:
                  Layer Height:
                    Layer Height: {{layer_height}}
                    First Layer Height: {{initial_layer_print_height}}
                  Line Width:
                    Default Line Width: {{line_width}}
                  Seam:
                    Seam Position: {{seam_position}}
                  Precision:
                    Precise Wall: 
                  Ironing:
                    Ironing: {{ironing_type}}
                  Wall Generator:
                    Generator: {{wall_generator}}
                  Walls And Surfaces:
                    Only One Wall on Top Surfaces: 

                Strength:
                  Walls:
                    Wall Loops: {{wall_loops}}
                  Top/Bottom Shells:
                    Top Shell Layers: {{top_shell_layers}}
                    Top Shell Thickness: {{top_shell_thickness}}
                    Bottom Shell Layers: {{bottom_shell_layers}}
                    Bottom Shell Thickness: {{bottom_shell_thickness}}
                  Infill:
                    Sparse Infill Density: {{sparse_infill_density}}
                    Sparse Infill Pattern: {{sparse_infill_pattern}}

                Speed:
                  First Layer Speed:
                    First Layer: {{initial_layer_speed}}
                    Number of Slow Layers: {{slow_down_layers}}
                  Other Layers Speed:
                    Outer Wall: {{outer_wall_speed}}
                    Inner Wall: {{inner_wall_speed}}
                    Sparse Infill: {{sparse_infill_speed}}
                    Internal Solid Infill: {{internal_solid_infill_speed}}
                    Top Surface: {{top_surface_speed}}
                  Travel Speed:
                    Travel: {{travel_speed}}

                Support:
                  Support:
                    Enable Support: {{enable_support}}
                    Type: {{support_type}}
                    Style: {{support_style}}
                    Threshold Angle: {{support_threshold_angle}}
                    On Build Plate Only: {{support_on_build_plate_only}}
                  Raft:
                    Raft Layers: {{raft_layers}}

                Multimaterial:
                  Prime Tower:
                    Enable: {{enable_prime_tower}}
                    Width: {{prime_tower_width}}
                    Brim Width: {{prime_tower_brim_width}}
                  Ooze Prevention:
                    Enable: {{ooze_prevention}}
                  Flush Options:
                    Flush into Objects' Infill: {{flush_into_infill}}
                    Flush into Objects' Support: {{flush_into_support}}
                  Advanced:
                    Use Beam Interlocking: 

                Other:
                  Skirt:
                    Skirt Type: 
                    Skirt Loops: {{skirt_loops}}
                    Skirt Min Extrusion Length: {{min_skirt_length}}
                  Brim:
                    Brim Type: {{brim_type}}
                    Brim Width: {{brim_width}}
                  Special Mode:
                    Slicing Mode: {{slicing_mode}}
                    Print Sequence: {{print_sequence}}
                    Spiral Vase: {{spiral_mode}}
                    Fuzzy Skin: {{fuzzy_skin}}
                """;
        }
    }
}

﻿namespace Slic3rPostProcessingUploader.Services.Parsers.PrusaSlicer
{
    internal class PrusaDefaultNoteTemplate : INoteTemplate
    {
        public string getNoteTemplate()
        {
            return """
                Settings:
                
                Layers and Perimeters:
                  Layer Height:
                    Layer Height: {{layer_height}}
                    First Layer Height: {{first_layer_height}}
                  Vertical Shells:
                    Perimeters: {{perimeters}}
                    Spiral Vase: {{spiral_vase}}
                  Horizontal Shells:
                    Top Solid Layers: {{top_solid_layers}}
                    Top Min Shell Thickness: {{top_solid_min_thickness}}
                    Bottom Solid Layers: {{bottom_solid_layers}}
                    Bottom Min Shell Thickness: {{bottom_solid_min_thickness}}
                  Advanced:
                    Seam Position: {{seam_position}}
                    Perimeter Generator: {{perimeter_generator}}
                  Fuzzy Skin:
                    Fuzzy Skin: {{fuzzy_skin}}
                
                Infill:
                  Infill:
                    Fill Density: {{fill_density}}
                    Fill Pattern: {{fill_pattern}}
                    Top Fill Pattern: {{top_fill_pattern}}
                    Bottom Fill Pattern: {{bottom_fill_pattern}}
                  Ironing:
                    Enable Ironing: {{ironing}}
                    Ironing Type: {{ironing_type}}
                
                Skirt And Brim:
                  Skirt:
                    Loops (min): {{skirts}}
                    Distance from brim/object: {{skirt_distance}}
                    Minimal Filament Extrusion Length: {{min_skirt_length}}
                  Brim:
                    Brim Type: {{brim_type}}
                    Brim Width: {{brim_width}}
                
                Support Material:
                  Support Material:
                    Generate Support: {{support_material}}
                    Auto generated Supports: {{support_material_auto}}
                    Overhang Threshold: {{support_material_threshold}}
                  Raft:
                    Raft Layers: {{raft_layers}}
                  Options for Support Material:
                    Style: {{support_material_style}}
                    Support on Build Plate Only: {{support_material_buildplate_only}}
                
                Speed:
                  Speed for Print Moves:
                    Perimeters: {{perimeter_speed}}
                    Small Perimeters: {{small_perimeter_speed}}
                    External Perimeters: {{external_perimeter_speed}}
                    Infill: {{infill_speed}}
                    Solid Infill: {{solid_infill_speed}}
                    Top Solid Infill: {{top_solid_infill_speed}}
                    Support Material: {{support_material_speed}}
                    Bridges: {{bridge_speed}}
                  Speed For Non-Print Moves:
                    Travel: {{travel_speed}}
                    Z Travel: {{travel_speed_z}}
                  Modifiers:
                    First Layer Speed: {{first_layer_speed}}
                    Speed of Object First Layer Over Raft: {{first_layer_speed_over_raft}}
                  Auto Speed (advanced):
                    Max Print Speed: {{max_print_speed}}
                    Max Volumetric Speed: {{max_volumetric_speed}}
                
                Multiple Extruders:
                  Extruders:  
                    Perimeter Extruder: {{perimeter_extruder}}
                    Infill Extruder: {{infill_extruder}}
                    Solid Infill Extruder: {{solid_infill_extruder}}
                    Support Material/Raft/Skirt Extruder: {{support_material_extruder}}
                    Support Material/Raft Interface Extruder: {{support_material_interface_extruder}}
                    Wipe Tower Extruder: {{wipe_tower_extruder}}
                  Ooze Prevention:
                    Enable: {{ooze_prevention}}
                  Wipe Tower:
                    Enable: {{wipe_tower}}
                    Position X: {{wipe_tower_x}}
                    Position Y: {{wipe_tower_y}}
                    Width: {{wipe_tower_width}}
                
                Advanced:
                  Extrusion Width:
                    Default Extrusion Width: {{extrusion_width}}
                    First Layer Extrusion Width: {{first_layer_extrusion_width}}
                    Perimeters: {{perimeter_extrusion_width}}
                    External Perimeters: {{external_perimeter_extrusion_width}}
                    Infill: {{infill_extrusion_width}}
                    Solid Infill: {{solid_infill_extrusion_width}}
                    Top Solid Infill: {{top_infill_extrusion_width}}
                    Support Material: {{support_material_extrusion_width}}
                """;
        }
    }
}

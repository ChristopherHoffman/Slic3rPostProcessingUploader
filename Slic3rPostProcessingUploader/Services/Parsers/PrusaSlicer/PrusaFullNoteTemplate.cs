﻿namespace Slic3rPostProcessingUploader.Services.Parsers.PrusaSlicer
{
    internal class PrusaFullNoteTemplate : INoteTemplate
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
                  Quality:
                    Extra Perimeters If Needed: {{extra_perimeters}}
                    Extra Perimeters On Overhangs: {{extra_perimeters_on_overhangs}}
                    Avoid Crossing Curled Overhangs: {{avoid_crossing_curled_overhangs}}
                    Avoid Crossing Perimeters: {{avoid_crossing_perimeters}}
                    Avoid Crossing Perimeters - Max Detour Length: {{avoid_crossing_perimeters_max_detour}}
                    Detect Thin Walls: {{thin_walls}}
                    Thick Bridges: {{thick_bridges}}
                  Advanced:
                    Seam Position: {{seam_position}}
                    Staggered Inner Seams: {{staggered_inner_seams}}
                    External Perimeters First: {{external_perimeters_first}}
                    Fill Gaps: {{gap_fill_enabled}}
                    Perimeter Generator: {{perimeter_generator}}
                  Fuzzy Skin:
                    Fuzzy Skin: {{fuzzy_skin}}
                    Fuzzy Skin Thickness: {{fuzzy_skin_thickness}}
                    Fuzzy Skin Point Distance: {{fuzzy_skin_point_dist}}
                  Only One Perimeter:
                    Single Perimeter On Top Surfaces: {{top_one_perimeter_type}}
                    Only One Perimeter On First Layer: {{only_one_perimeter_first_layer}}

                Infill:
                  Infill:
                    Fill Density: {{fill_density}}
                    Fill Pattern: {{fill_pattern}}
                    Length of Infill Anchor: {{infill_anchor}}
                    Max Length of Anchor: {{infill_anchor_max}}
                    Top Fill Pattern: {{top_fill_pattern}}
                    Bottom Fill Pattern: {{bottom_fill_pattern}}
                  Ironing:
                    Enable Ironing: {{ironing}}
                    Ironing Type: {{ironing_type}}
                    Flow Rate: {{ironing_flowrate}}
                    Spacing Between Ironing Passes: {{ironing_spacing}}
                  Reducing Printing Time:
                    Combine Infill Every: {{infill_every_layers}}
                  Advanced:
                    Solid Infill Every: {{solid_infill_every_layers}}
                    Fill Angle: {{fill_angle}}
                    Solid Infill Threshold Area: {{solid_infill_below_area}}
                    Bridging Angle: {{bridge_angle}}
                    Only Retract When Crossing Perimeters: {{only_retract_when_crossing_perimeters}}
                    Infill Before Perimeters: {{infill_first}}

                Skirt And Brim:
                  Skirt:
                    Loops (min): {{skirts}}
                    Distance from brim/object: {{skirt_distance}}
                    Skirt Height: {{skirt_height}}
                    Draft Shield: {{draft_shield}}
                    Minimal Filament Extrusion Length: {{min_skirt_length}}
                  Brim:
                    Brim Type: {{brim_type}}
                    Brim Width: {{brim_width}}
                    Brim Separation Gap: {{brim_separation}}

                Support Material:
                  Support Material:
                    Generate Support: {{support_material}}
                    Auto generated Supports: {{support_material_auto}}
                    Overhang Threshold: {{support_material_threshold}}
                    Enforce Support For The First: {{support_material_enforce_layers}}
                    First Layer Density: {{raft_first_layer_density}}
                    First Layer Expansion: {{raft_first_layer_expansion}}
                  Raft:
                    Raft Layers: {{raft_layers}}
                    Raft Contact Z Distance: {{raft_contact_distance}}
                    Raft Expansion: {{raft_expansion}}
                  Options for Support Material:
                    Style: {{support_material_style}}
                    Top Contact Z Distance: {{support_material_top_contact_distance}}
                    Bottom Contact Z Distance: {{support_material_bottom_contact_distance}}
                    Pattern: {{support_material_pattern}}
                    With Sheath Around the Support: {{support_material_with_sheath}}
                    Pattern Spacing: {{support_material_spacing}}
                    Pattern Angle: {{support_material_angle}}
                    Closing Radius: {{support_material_closing_radius}}
                    Top Interface Layers: {{support_material_interface_layers}}
                    Bottom Interface Layers: {{support_material_bottom_interface_layers}}
                    Interface Pattern: {{support_material_interface_pattern}}
                    Interface Pattern Spacing: {{support_material_interface_spacing}}
                    Interface Loops: {{support_material_interface_contact_loops}}
                    Support on Build Plate Only: {{support_material_buildplate_only}}
                    ZY Separation Between and Object and Support: {{support_material_xy_spacing}}
                    Don't Support Bridges: {{dont_support_bridges}}
                    Synchronize with Object Layers: {{support_material_synchronize_layers}}
                  Organic Supports:
                    Max Branch Angle: {{support_tree_angle}}
                    Preferred Angle: {{support_tree_angle_slow}}
                    Branch Diameter: {{support_tree_branch_diameter}}
                    Branch Diameter Angle: {{support_tree_branch_diameter_angle}}
                    Branch Diameter With Double Walls: {{support_tree_branch_diameter_double_wall}}
                    Tip Diameter: {{support_tree_tip_diameter}}
                    Branch Distance: {{support_tree_branch_distance}}
                    Branch Density: {{support_tree_top_rate}}
                
                Speed:
                  Speed for Print Moves:
                    Perimeters: {{perimeter_speed}}
                    Small Perimeters: {{small_perimeter_speed}}
                    External Perimeters: {{external_perimeter_speed}}
                    Infill: {{infill_speed}}
                    Solid Infill: {{solid_infill_speed}}
                    Top Solid Infill: {{top_solid_infill_speed}}
                    Support Material: {{support_material_speed}}
                    Support Material Interface: {{support_material_interface_speed}}
                    Bridges: {{bridge_speed}}
                    Gap Fill: {{gap_fill_speed}}
                    Ironing: {{ironing_speed}}
                  Dynamic Overhang Speed:
                    Enable Dynamic Speed: {{enable_dynamic_overhang_speeds}}
                    Speed for 0% Overlap: {{overhang_speed_0}}
                    Speed for 25% Overlap: {{overhang_speed_1}}
                    Speed For 50% Overlap: {{overhang_speed_2}}
                    Speed for 75% Overlap: {{overhang_speed_3}}
                  Speed For Non-Print Moves:
                    Travel: {{travel_speed}}
                    Z Travel: {{travel_speed_z}}
                  Modifiers:
                    First Layer Speed: {{first_layer_speed}}
                    Speed of Object First Layer Over Raft: {{first_layer_speed_over_raft}}
                  Acceleration Control (Advanced):
                    External Perimeters: {{external_perimeter_acceleration}}
                    Perimeters: {{perimeter_acceleration}}
                    Top Solid Infill: {{top_solid_infill_acceleration}}
                    Solid Infill: {{solid_infill_acceleration}}
                    Infill: {{infill_acceleration}}
                    Bridge: {{bridge_acceleration}}
                    First Layer: {{first_layer_acceleration}}
                    First Object Layer Over Raft: {{first_layer_acceleration_over_raft}}
                    Wipe Tower: {{wipe_tower_acceleration}}
                    Travel: {{travel_acceleration}}
                    Default: {{default_acceleration}}
                  Auto Speed (advanced):
                    Max Print Speed: {{max_print_speed}}
                    Max Volumetric Speed: {{max_volumetric_speed}}
                  Pressure Equalizer (experimental):
                    Max Volumetric Slope Positive: {{max_volumetric_extrusion_rate_slope_positive}}
                    Max Volumetric Slope Negative: {{max_volumetric_extrusion_rate_slope_negative}}

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
                    Temperature Variation: 
                  Wipe Tower:
                    Enable: {{wipe_tower}}
                    Position X: {{wipe_tower_x}}
                    Position Y: {{wipe_tower_y}}
                    Width: {{wipe_tower_width}}
                    Wipe Tower Rotation Angle: {{wipe_tower_rotation_angle}}
                    Wipe Tower Brim Width: {{wipe_tower_brim_width}}
                    Maximal Bridging Distance: {{wipe_tower_bridging}}
                    Stabilization Cone Apex Angle: {{wipe_tower_cone_angle}}
                    Wipe Tower Purge Line Spacing: 
                    Extra Flow For Purging: {{wipe_tower_extra_flow}}
                    No Sparse Layers: {{wipe_tower_no_sparse_layers}}
                    Prime All Printing Extruders: 
                  Advanced:
                    Interface Shells: {{interface_shells}}
                    Maximal Width of Segment: {{mmu_segmented_region_max_width}}
                    Interlocking Depth of Segment: {{mmu_segmented_region_interlocking_depth}}

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
                  Overlap:
                    Infill/Perimeters Overlap: {{infill_overlap}}
                  Flow:
                    Bridge Flow Ratio: {{bridge_flow_ratio}}
                  Slicing:
                    Slice Gap Closing Radius: {{slice_closing_radius}}
                    Slicing Mode: {{slicing_mode}}
                    Slice Resolution: {{resolution}}
                    G-Code Resolution: {{gcode_resolution}}
                    Arc Fitting: {{arc_fitting}}
                    XY Size Compensation: {{xy_size_compensation}}
                    Elephant Foot Compensation: {{elefant_foot_compensation}}
                  Arachne Perimeter Generator:
                    Perimeter Transitioning Threshold Angle: 
                    Perimeter Transitioning Filter Margin: 
                    Perimeter Transition Length: 
                    Perimeter Distribution Count: 
                    Min Perimeter Width: 
                    Min Feature Size: {{min_feature_size}}
                """;
        }
    }
}
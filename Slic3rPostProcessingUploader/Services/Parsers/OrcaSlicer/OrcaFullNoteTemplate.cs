﻿namespace Slic3rPostProcessingUploader.Services.Parsers.OrcaSlicer
{
    internal class OrcaFullNoteTemplate : INoteTemplate
    {
        public string getNoteTemplate()
        {
            return """
                Settings:

                Quality:
                  Layer Height:
                    Layer Height: {{layer_height}}
                    First Layer Height: {{first_layer_height}}
                  Line Width:
                    Default Line Width: {{line_width}}
                    First Layer Line Width: {{initial_layer_line_width}}
                    Outer Wall Line Width: {{outer_wall_line_width}}
                    Inner Wall Line Width: {{inner_wall_line_width}}
                    Top Surface Line Width: {{top_surface_line_width}}
                    Sparse Infill Line Width: {{sparse_infill_line_width}}
                    Internal Solid Infill Line Width: {{internal_solid_infill_line_width}}
                    Support Line Width: {{support_line_width}}
                  Seam:
                    Seam Position: {{seam_position}}
                    Staggered Inner Seams: {{staggered_inner_seams}}
                    Scarf Joint: {{has_scarf_joint_seam}}
                    Wipe Speed: {{wipe_speed}}
                  Precision:
                    Slice Gap Closing Radius: {{slice_closing_radius}}
                    Resolution: {{resolution}}
                    Arc Fitting: {{enable_arc_fitting}}
                    X-Y Hole Compensation: {{xy_hole_compensation}}
                    X-Y Contour Compensation: {{xy_contour_compensation}}
                    Elephant Foot Compensation: {{elefant_foot_compensation}}
                    Precise Wall: {{precise_outer_wall}}
                    Precise Z Height: {{precise_z_height}}
                    Convert Holes to Polyholes: {{hole_to_polyhole}}
                  Ironing:
                    Ironing: {{ironing_type}}
                    Ironing Pattern: {{ironing_pattern}}
                    Ironing Speed: {{ironing_speed}}
                    Ironing Flow: {{ironing_flow}}
                    Ironing Line Spacing: {{ironing_spacing}}
                    Ironing Angle: {{ironing_angle}}
                  Wall Generator:
                    Generator: {{wall_generator}}
                  Walls And Surfaces:
                    Walls Printing Order: {{wall_sequence}}
                    Print Infill First: {{is_infill_first}}
                    Wall Loop Direction: {{wall_direction}}
                    Top Surface Flow Ratio: {{top_solid_infill_flow_ratio}}
                    Bottom Surface Flow Ratio: {{bottom_solid_infill_flow_ratio}}
                    Only One Wall on Top Surfaces: {{only_one_wall_top}}
                    One Wall Threshold: {{only_one_wall_first_layer}}
                    Avoid Crossing Walls: {{reduce_crossing_wall}}
                    Avoid Crossing Walls - Max Detour Length: {{max_travel_detour_distance}}
                    Small Area Flow Compensation: {{small_area_infill_flow_compensation}}
                  Bridging:
                    Bridge Flow Ratio: {{bridge_flow}}
                    Internal Bridge Flow Ratio: {{internal_bridge_flow}}
                    Bridge Density: {{bridge_density}}
                    Thick Bridges: {{thick_bridges}}
                    Thick Internal Bridges: {{thick_internal_bridges}}
                    Filter Out Small Internal Bridges: {{dont_filter_internal_bridges}}
                    Bridge Counterbore Holes: {{counterbore_hole_bridging}}
                  Overhangs:
                    Detect Overhang Walls: {{detect_overhang_wall}}
                    Make Overhangs Printable: {{make_overhang_printable}}
                    Extra Perimeters on Overhangs: {{extra_perimeters_on_overhangs}}
                    Reverse on Even: {{overhang_reverse}}

                Strength:
                  Walls:
                    Wall Loops: {{wall_loops}}
                    Alternate Extra Wall: {{alternate_extra_wall}}
                    Detect Thin Walls: {{detect_thin_wall}}
                  Top/Bottom Shells:
                    Top Surface Pattern: {{top_surface_pattern}}
                    Top Shell Layers: {{top_shell_layers}}
                    Top Shell Thickness: {{top_shell_thickness}}
                    Bottom Surface Pattern: {{bottom_surface_pattern}}
                    Bottom Shell Layers: {{bottom_shell_layers}}
                    Bottom Shell Thickness: {{bottom_shell_thickness}}
                    Top/Bottom Solid Infill/Wall Overlap: {{top_bottom_infill_wall_overlap}}
                  Infill:
                    Sparse Infill Density: {{sparse_infill_density}}
                    Sparse Infill Pattern: {{sparse_infill_pattern}}
                    Sparse Infill Anchor Length: {{infill_anchor}}
                    Max Length of Infill Anchor: {{infill_anchor_max}}
                    Internal Solid Infill Pattern: {{internal_solid_infill_pattern}}
                    Apply Gap Fill: {{apply_gap_fill}}
                    Filter Out Tiny Gaps: {{filter_out_gap_fill}}
                    Infill/Wall Overlap: {{infill_wall_overlap}}
                  Advanced:
                    Sparse Infill Direction: {{infill_direction}}
                    Solid Infill Direction: {{solid_infill_direction}}
                    Rotate Solid Infill Direction: {{rotate_solid_infill_direction}}
                    Bridge Infill Direction: {{bridge_angle}}
                    Min Sparse Infill Threshold: {{minimum_sparse_infill_area}}
                    Infill Combination - Max Layer Height: {{infill_combination_max_layer_height}}
                    Detect Narrow Internal Solid Infill: {{detect_narrow_internal_solid_infill}}
                    Ensure Vertical Shell Thickness: {{ensure_vertical_shell_thickness}}

                Speed:
                  First Layer Speed:
                    First Layer: {{initial_layer_speed}}
                    First Layer Infill: {{initial_layer_infill_speed}}
                    Initial Layer Travel Speed: {{initial_layer_travel_speed}}
                    Number of Slow Layers: {{slow_down_layers}}
                  Other Layers Speed:
                    Outer Wall: {{outer_wall_speed}}
                    Inner Wall: {{inner_wall_speed}}
                    Small Perimeters: {{small_perimeter_speed}}
                    Small Perimeters Threshold: {{small_perimeter_threshold}}
                    Sparse Infill: {{sparse_infill_speed}}
                    Internal Solid Infill: {{internal_solid_infill_speed}}
                    Top Surface: {{top_surface_speed}}
                    Gap Infill: {{gap_infill_speed}}
                    Support: {{support_speed}}
                    Support Interface: {{support_interface_speed}}
                  Overhang Speed:
                    Slow Down For Overhangs: {{slow_down_for_layer_cooling}}
                    Slow Down For Curled Perimeters: {{slowdown_for_curled_perimeters}}
                    Overhang Speed: {{overhang_2_4_speed}}
                    Bridge: {{bridge_speed}}
                  Travel Speed:
                    Travel: {{travel_speed}}
                  Acceleration:
                    Normal Printing: {{default_acceleration}}
                    Outer Wall: {{outer_wall_acceleration}}
                    Inner Wall: {{inner_wall_acceleration}}
                    Bridge: {{bridge_acceleration}}
                    Sparse Infill: {{sparse_infill_acceleration}}
                    Internal Solid Infill: {{internal_solid_infill_acceleration}}
                    First Layer: {{initial_layer_acceleration}}
                    Top Surface: {{top_surface_acceleration}}
                    Travel: {{travel_acceleration}}
                    Enable accel_to_decel: {{accel_to_decel_enable}}
                    accel_to_decel: {{accel_to_decel_factor}}
                  Jerk:
                    Default: {{default_jerk}}
                    Outer Wall: {{outer_wall_jerk}}
                    Inner Wall: {{inner_wall_jerk}}
                    Infill: {{infill_jerk}}
                    Top Surface: {{top_surface_jerk}}
                    First Layer: {{initial_layer_jerk}}
                    Travel: {{travel_jerk}}

                Support:
                  Support:
                    Enable Support: {{enable_support}}
                    Type: {{support_type}}
                    Style: {{support_style}}
                    Threshold Angle: {{support_threshold_angle}}
                    On Build Plate Only: {{support_on_build_plate_only}}
                    Remove Small Overhangs: {{support_remove_small_overhang}}
                  Raft:
                    Raft Layers: {{raft_layers}}
                  Filament For Supports:
                    Support/Raft Base: {{support_filament}}
                    Support/Raft Interface: {{support_interface_filament}}
                  Advanced:
                    Top Z Distance: {{support_top_z_distance}}
                    Bottom Z Distance: {{support_bottom_z_distance}}
                    Base Pattern: {{support_base_pattern}}
                    Base Pattern Spacing: {{support_base_pattern_spacing}}
                    Pattern Angle: {{support_angle}}
                    Top Interface Layers: {{support_interface_top_layers}}
                    Bottom Interface Layers: {{support_interface_bottom_layers}}
                    Interface Pattern: {{support_interface_pattern}}
                    Top Interface Spacing: {{support_interface_spacing}}
                    Bottom Interface Spacing: {{support_bottom_interface_spacing}}
                    Normal Support Expansion: {{support_expansion}}
                    Support/Object XY Distance: {{support_object_xy_distance}}
                    Don't Support Bridges: {{dont_support_bridges}}

                Multimaterial:
                  Prime Tower:
                    Enable: {{enable_prime_tower}}
                    Width: {{prime_tower_width}}
                    Brim Width: {{prime_tower_brim_width}}
                    Wipe Tower Rotation Angle: {{wipe_tower_rotation_angle}}
                    Maximal Bridging Distance: {{max_bridge_length}}
                  Ooze Prevention:
                    Enable: {{ooze_prevention}}
                  Flush Options:
                    Flush into Objects' Infill: {{flush_into_infill}}
                    Flush into Objects' Support: {{flush_into_support}}
                  Advanced:
                    Use Beam Interlocking: {{interlocking_beam}}
                    Max Width of Segment: {{interlocking_beam_width}}
                    Interlocking Depth of Segment: {{interlocking_depth}}

                Other:
                  Skirt:
                    Skirt Type: {{skirt_type}}
                    Skirt Loops: {{skirt_loops}}
                    Skirt Min Extrusion Length: {{min_skirt_length}}
                    Skirt Distance: {{skirt_distance}}
                    Skirt Height: {{skirt_height}}
                    Skirt Speed: {{skirt_speed}}
                    Draft Shield: {{draft_shield}}
                  Brim:
                    Brim Type: {{brim_type}}
                    Brim Width: {{brim_width}}
                    Brim-Object Gap: {{brim_object_gap}}
                  Special Mode:
                    Slicing Mode: {{slicing_mode}}
                    Print Sequence: {{print_sequence}}
                    Spiral Vase: {{spiral_mode}}
                    Fuzzy Skin: {{fuzzy_skin}}
                """;
        }
    }
}
namespace Slic3rPostProcessingUploader.Services
{
    internal class OrcaFullNoteTemplate : INoteTemplate
    {
        public string getNoteTemplate()
        {
            return """
                Full Note Template.

                Settings:

                Quality:
                  Layer Height:
                    Layer Height: {{layer_height}}
                    First Layer Height: {{first_layer_height}}
                    Wall Loops: {{wall_loops}}
                    Top Shell Layers: {{top_shell_layers}}
                    Bottom Shell Layers: {{bottom_shell_layers}}
                    Sparse Infill Density: {{sparse_infill_density}}
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
                    Wipe Speed:
                  Precision:
                    Slice Gap Closing Radius:
                    Resolution:
                    Arc Fitting:
                    X-Y Hole Compensation:
                    X-Y Contour Compensation:
                    Elephant Foot Compensation:
                    Precise Wall:
                    Precise Z Height:
                    Convert Holes to Polyholes:
                  Ironing:
                    Ironing: {{ironing_type}}
                    Ironing Pattern:
                    Ironing Speed:
                    Ironing Flow:
                    Ironing Line Spacing:
                    Ironing Angle:
                  Wall Generator:
                    Generator: {{wall_generator}}
                  Walls And Surfaces:
                    Walls Printing Order: {{wall_sequence}}
                    Print Infill First: {{is_infill_first}}
                    Wall Loop Direction: {{wall_direction}}
                    Top Surface Flow Ratio:
                    Bottom Surface Flow Ratio:
                    Only One Wall on Top Surfaces:
                    One Wall Threshold:
                    Avoid Crossing Walls:
                    Avoid Crossing Walls - Max Detour Length
                    Small Area Flow Compensation:
                  Bridging:
                    Bridge Flow Ratio:
                    Internal Bridge Flow Ratio:
                    Bridge Density:
                    Thick Bridges:
                    Thick Internal Bridges:
                    Filter Out Small Internal Bridges:
                    Bridge Counterbore Holes:
                  Overhangs:
                    Detect Overhang Walls:
                    Make Overhangs Printable:
                    Extra Perimeters on Overhangs:
                    Reverse on Even:

                Strength:
                  Walls:
                    Wall Loops:
                    Alternate Extra Wall:
                    Detect Thin Walls:
                  Top/Bottom Shells:
                    Top Surface Pattern:
                    Top Shell Layers:
                    Top Shell Thickness:
                    Bottom Surface Pattern:
                    Bottom Shell Layers:
                    Bottom Shell Thickness:
                    Top/Bottom Solid Infill/Wall Overlap:
                  Infill:
                    Sparse Infill Density:
                    Sparse Infill Pattern:
                    Sparse Infill Anchor Length:
                    Max Length of Infill Anchor:
                    Internal Solid Infill Pattern:
                    Apply Gap Fill:
                    Filter Out Tiny Gaps:
                    Infill/Wall Overlap:
                  Advanced:
                    Sparse Infill Direction:
                    Solid Infill Direction:
                    Rotate Solid Infill Direction:
                    Bridge Infill Direction:
                    Min Sparse Infill Threshold:
                    Infill Combination - Max Layer Height:
                    Detect Narrow Internal Solid Infill:
                    Ensure Vertical Shell Thickness:

                Speed:
                  First Layer Speed:
                    First Layer:
                    First Layer Infill:
                    Initial Layer Travel Speed:
                    Number of Slow Layers:
                  Other Layers Speed:
                    Outer Wall:
                    Inner Wall:
                    Small Perimeters:
                    Small Perimeters Threshold:
                    Sparse Infill:
                    Internal Solid Infill:
                    Top Surface:
                    Gap Infill:
                    Support:
                    Support Interface:
                  Overhand Speed:
                    Slow Down For Overhangs:
                    Slow Down For Curled Perimeters:
                    Overhang Speed:
                    Bridge:
                  Travel Speed:
                    Travel:
                  Acceleration:
                    Normal Printing:
                    Outer Wall:
                    Inner Wall:
                    Bridge:
                    Sparse Infill:
                    Internal Solid Infill:
                    First Layer:
                    Top Surface:
                    Travel:
                    Enable accel_to_decel:
                    accel_to_decel:
                  Jerk:
                    Default:
                    Outer Wall:
                    Inner Wall:
                    Infill:
                    Top Surface:
                    First Layer:
                    Travel:
                  Advanced:
                    Extrusion Rate Smoothing:

                Support:
                  Support:
                    Enable Support:
                    Type:
                    Style:
                    Threshold Angle:
                    First Layer Density:
                    First Layer Expansion:
                    On Build Plate Only:
                    Remove Small Overhangs:
                  Raft:
                    Raft Layers:
                  Filament For Supports:
                    Support/Raft Base:
                    Support/Raft Interface:
                  Advanced:
                    Top Z Distance:
                    Bottom Z Distance:
                    Base Pattern:
                    Base Pattern Spacing:
                    Pattern Angle:
                    Top Interface Layers:
                    Bottom Interface Layers:
                    Interface Pattern:
                    Top Interface Spacing:
                    Bottom Interface Spacing:
                    Normal Support Expansion:
                    Support/Object XY Distance:
                    Don't Support Bridges:

                Multimaterial:
                  Prime Tower:
                    Enable:
                    Width:
                    Brim Width:
                    Wipe Tower Rotation Angle:
                    Maximal Bridging Distance:
                  Ooze Prevention:
                    Enable:
                  Flush Options:
                    Flush into Objects' Infill:
                    Flush into Objects' Support:
                  Advanced:
                    Use Beam Interlocking:
                    Max Width of Segment:
                    Interlocking Depth of Segment:

                Other:
                  Skirt:
                    Skirt Type:
                    Skirt Loops:
                    Skirt Min Extrusion Length:
                    Skirt Distance:
                    Skirt Start Point:
                    Skirt Height:
                    Skirt Speed:
                    Draft Shield:
                  Brim:
                    Brim Type:
                    Brim Width:
                    Brim-Object Gap:
                  Special Mode:
                    Slicing Mode:
                    Print Sequence:
                    Intra-Layer Order
                    Spiral Vase:
                    Fuzzy Skin:
                """;
        }
    }
}

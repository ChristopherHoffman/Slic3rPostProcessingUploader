namespace Slic3rPostProcessingUploader.Services.Parsers.PrusaSlicer
{
    internal class PrusaFullNoteTemplate : INoteTemplate
    {
        public string getNoteTemplate()
        {
            return """
                Settings:

                Layers and Perimeters:
                  Layer Height:
                    Layer Height:
                    First Layer Height:
                  Vertical Shells:
                    Perimeters:
                    Spiral Vase:
                  Horizontal Shells:
                    Solid Layers:
                    Min Shell Thickness:
                  Quality:
                    Extra Perimeters If Needed:
                    Extra Perimeters On Overhangs:
                    Avoid Crossing Curled Overhangs:
                    Avoid Crossing Perimeters:
                    Avoid Crossing Perimeters - Max Detour Length:
                    Detect Thin Walls:
                    Thick Bridges:
                    Detect Bridging Perimeters:
                  Advanced:
                    Seam Position:
                    Staggered Inner Seams:
                    External Perimeters First:
                    Fill Gaps:
                    Perimeter Generator:
                  Fuzzy Skin:
                    Fuzzy Skin:
                    Fuzzy Skin Thickness:
                    Fuzzy Skin Point Distance:
                  Only One Perimeter:
                    Single Perimeter On Top Surfaces:
                    Only One Perimeter On First Layer:

                Infill:
                  Infill:
                    Fill Density:
                    Fill Pattern:
                    Length of Infill Anchor:
                    Max Length of Anchor:
                    Top Fill Pattern:
                    Bottom Fill Pattern:
                  Ironing:
                    Enable Ironing:
                    Ironing Type:
                    Flow Rate:
                    Spacing Between Ironing Passes:
                  Reducing Printing Time:
                    Combine Infill Every:
                  Advanced:
                    Solid Infill Every:
                    Fill Angle:
                    Solid Infill Threshold Area:
                    Bridging Angle:
                    Only Retract When Crossing Perimeters:
                    Infill Before Perimeters:

                Skirt And Brim:
                  Skirt:
                    Loops (min):
                    Distance from brim/object:
                    Skirt Height:
                    Draft Shield:
                    Minimal Filament Extrusion Length:
                  Brim:
                    Brim Type:
                    Brim Width:
                    Brim Separation Gap:

                Support Material:
                  Support Material:
                    Generate Support:
                    Auto generated Supports:
                    Overhang Threshold:
                    Enforce Support For The First:
                    First Layer Density
                    First Layer Expansion:
                  Raft:
                    Raft Layers:
                    Raft Contact Z Distance:
                    Raft Expansion:
                  Options for Support Material:
                    Style:
                    Top Contact Z Distance:
                    Bottom Contact Z Distance:
                    Pattern:
                    With Sheath Around the Support:
                    Pattern Spacing:
                    Pattern Angle:
                    Closing Radius:
                    Top Interface Layers:
                    Bottom Interface Layers:
                    Interface Pattern:
                    Interface Pattern Spacing:
                    Interface Loops:
                    Support on Build Plate Only:
                    ZY Separation Between and Object and Support:
                    Don't Support Bridges:
                    Synchronize with Object Layers:
                  Organic Supports:
                    Max Branch Angle:
                    Preferred Angle:
                    Branch Diameter:
                    Branch Diameter Angle:
                    Branch Diameter With Double Walls:
                    Tip Diameter:
                    Branch Distance:
                    Branch Density:
                
                Speed:
                  Speed for Print Moves:
                    Perimeters:
                    Small Perimeters:
                    External Perimeters:
                    Infill:
                    Solid Infill:
                    Top Solid Infill:
                    Support Material:
                    Support Material Interface:
                    Bridges:
                    Gap Fill:
                    Ironing:
                  Dynamic Overhang Speed:
                    Enable Dynamic Speed:
                    Speed for 0% Overlap:
                    Speed for 25% Overlap:
                    Speed For 50% Overalp:
                    Speed for 75% Overlap:
                  Speed For Non-Print Moves:
                    Travel:
                    Z Travel:
                  Modifiers:
                    First Layer Speed:
                    Speed of Object First Layer Over Raft:
                  Acceleration Control (Advanced):
                    External Perimeters:
                    Perimeters:
                    Top Solid Infill:
                    Solid Infill:
                    Infill:
                    Bridge:
                    First Layer:
                    First Object Layer Over Raft:
                    Wipe Tower:
                    Travel:
                    Default:
                  Auto Speed (advanced):
                    Max Print Speed:
                    Max Volumetric Speed:
                  Pressure Equalizer (experimental):
                    Max Volumetric Slope Positive:
                    Max Volumetric Slope Negative:

                Multiple Extruders:
                  Extruders:  
                    Perimeter Extruder:
                    Infill Extruder:
                    Solid Infill Extruder:
                    Support Material/Raft/Skirt Extruder:
                    Support Material/Raft Interface Extruder:
                    Wipe Tower Extruder:
                  Ooze Prevention:
                    Enable:
                    Temperature Variation:
                  Wipe Tower:
                    Enable:
                    Position X:
                    Position Y:
                    Width:
                    Wipe Tower Rotation Angle:
                    Wipe Tower Brim Width:
                    Maximal Bridging Distance:
                    Stabilization Cone Apex Angle:
                    Wipe Tower Purge Line Spacing:
                    Extra Flow For Purging:
                    No Sparse Layers:
                    Prime All Printing Extruders:
                  Advanced:
                    Interface Shells:
                    Maximal Width of Segment:
                    Interlocking Depth of Segment:

                Advanced:
                  Extrusion Width:
                    Default Extrusion Width:
                    First Layer Extrusion Width:
                    Perimeters:
                    External Perimeters:
                    Infill:
                    Solid Infill:
                    Top Solid Infill:
                    Support Material:
                  Overlap:
                    Infill/Perimeters Overlap:
                  Flow:
                    Bridge Flow Ratio:
                  Slicing:
                    Slice Gap Closing Radius:
                    Slicing Mode:
                    Slice Resolution:
                    G-Code Resolution:
                    Arc Fitting:
                    XY Size Compensation:
                    Elephant Foot Compensation:
                  Arachne Perimeter Generator:
                    Perimeter Transitioning Threshold Angle:
                    Perimeter Transitioning Filter Margin:
                    Perimeter Transition Length:
                    Perimeter Distribution Count:
                    Min Perimeter Width:
                    Min Feature Size:
                """;
        }
    }
}
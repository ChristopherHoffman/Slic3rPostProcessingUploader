# Slic3r Post-Processing Uploader for 3D Print Log

A Slic3r/PrusaSlicer/OrcaSlicer/Bambu Studio Post-Processing script for uploading print details to https://www.3dprintlog.com.

This program will parse the gcode file and open up https://www.3dprintlog.com with the print details filled out.

Create a free account at https://www.3dprintlog.com today and enjoy all the features.

## Usage:

Usage: In the Slicer's 'Post-Processing Scripts' section, add the path to this file
Slic3rPostProcessingUploader.exe [options]

## Options:

`--help`, `-h`: Display this help message. No settings will be uploaded if help is displayed.
`--local-dev`: Use the local development environment
`--debug <path>`: Save debug information to the specified path
`--opt-out-telemetry`: Disable telemetry tracking. To help improve the plugin, we track slicer and plugin versions, as well as log errors that are thrown. No personal data is collected.

### Note Template Options:

`--default`: Use the default note template, which contains a curated list of general settings. Preferred by most users. The Default template is used if no other note template option is given.
`--full`: Use the full note template, which lists most of the settings available in the slicers
`--template <path>`: Use a custom note template. Absolute paths work better. See README for more details on syntax

## Example

In your Slicer's Post Processing text box, input:
`Slic3rPostProcessingUploader --default`

Once you slice your object and export the `.gcode` file, this plugin will run and open your default web browser to https://www.3dprintlog.com with all of the print details and settings filled out.

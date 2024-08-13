# FBX Material Assign Tool

FBX Material Assign Tool is a Unity Editor plugin that simplifies the process of assigning materials to FBX models. It ensures that only the correct materials are applied, and automatically sets the "Material Creation Mode" to "None" for FBX files that do not have matching materials.

## Features

- **Automatic Material Assignment**: Searches and remaps materials for all FBX files in a selected folder.
- **Custom Material Handling**: Automatically disables material creation for FBX files that do not have matching materials.
- **Batch Processing**: Handles multiple FBX files in a single operation.
- **User-Friendly Interface**: Simple drag-and-drop interface for selecting folders and assigning materials.

## Installation

1. Download or clone this repository.
2. Copy the `FBXMaterialAssign.cs` script into your Unity project's `Editor` folder.
3. The tool will appear under the `Window > Level Design > FBX Material Assign` menu in the Unity Editor.

## Usage

1. Open the FBX Material Assign Tool from `Window > Level Design > FBX Material Assign`.
2. Drag and drop a folder containing FBX files into the designated area in the tool's interface.
3. Click the "Assign Materials" button to start the material remapping process.
4. The tool will search for corresponding materials in your project. If no matching materials are found, the "Material Creation Mode" will be set to "None" for the affected FBX files.

## Example Workflow

1. Place all your FBX files in a single folder within your Unity project.
2. Prepare the corresponding materials in your Unity project.
3. Open the FBX Material Assign Tool, drag the folder into the tool, and click "Assign Materials".
4. The tool will automatically remap materials and disable material creation for any FBX files without matching materials.

## Contributing

Contributions are welcome! If you have suggestions for improvements or new features, feel free to open an issue or submit a pull request.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.



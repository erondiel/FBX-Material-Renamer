using UnityEditor;
using UnityEngine;
using System.IO;

public class FBXMaterialAssign : EditorWindow
{
    private Object folderObject;

    [MenuItem("Window/Level Design/FBX Material Assign")]
    public static void ShowWindow()
    {
        GetWindow<FBXMaterialAssign>("FBX Material Assign");
    }

    private void OnGUI()
    {
        GUILayout.Label("Assign Folder to Search for FBX Files", EditorStyles.boldLabel);

        // Drag-and-Drop Folder Field
        folderObject = EditorGUILayout.ObjectField("Folder", folderObject, typeof(Object), false);

        if (folderObject != null)
        {
            string folderPath = AssetDatabase.GetAssetPath(folderObject);

            if (Directory.Exists(folderPath))
            {
                if (GUILayout.Button("Assign Materials", GUILayout.Height(40)))
                {
                    AssignMaterials(folderPath);
                }
            }
            else
            {
                EditorGUILayout.HelpBox("Please select a valid folder.", MessageType.Warning);
            }
        }
        else
        {
            EditorGUILayout.HelpBox("Drag and drop a folder here.", MessageType.Info);
        }
    }

    private void AssignMaterials(string folderPath)
    {
        string[] fbxFiles = Directory.GetFiles(folderPath, "*.fbx", SearchOption.AllDirectories);

        foreach (string fbxFile in fbxFiles)
        {
            string assetPath = fbxFile.Replace("\\", "/").Replace(Application.dataPath, "Assets");
            ModelImporter importer = AssetImporter.GetAtPath(assetPath) as ModelImporter;

            if (importer != null)
            {
                // Perform the material search and remap
                importer.materialImportMode = ModelImporterMaterialImportMode.ImportStandard;
                importer.materialSearch = ModelImporterMaterialSearch.Everywhere;
                importer.SearchAndRemapMaterials(ModelImporterMaterialName.BasedOnMaterialName, ModelImporterMaterialSearch.Everywhere);
                AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);

                // Check if any material was actually assigned
                bool materialAssigned = CheckIfMaterialAssigned(importer);

                // If no material was assigned, set the Material Creation Mode to None
                if (!materialAssigned)
                {
                    importer.materialImportMode = ModelImporterMaterialImportMode.None;
                    Debug.Log($"No material found for: {assetPath}. Material Creation Mode set to None.");
                    AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
                }
                else
                {
                    Debug.Log($"Updated material settings for: {assetPath}");
                }
            }
        }

        AssetDatabase.Refresh();
        Debug.Log("Material assignment complete.");
    }

    private bool CheckIfMaterialAssigned(ModelImporter importer)
    {
        var materialMap = importer.GetExternalObjectMap();

        foreach (var entry in materialMap)
        {
            if (entry.Value is Material)
            {
                return true;  // Material successfully remapped
            }
        }

        return false;  // No materials were remapped
    }
}

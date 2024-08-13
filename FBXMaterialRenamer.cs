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
                importer.materialImportMode = ModelImporterMaterialImportMode.ImportStandard;
                importer.materialSearch = ModelImporterMaterialSearch.Everywhere;
                importer.SearchAndRemapMaterials(ModelImporterMaterialName.BasedOnMaterialName, ModelImporterMaterialSearch.Everywhere);
                AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
                Debug.Log($"Updated material settings for: {assetPath}");
            }
        }

        AssetDatabase.Refresh();
        Debug.Log("Material assignment complete.");
    }
}

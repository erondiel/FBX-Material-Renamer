using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class FBXMaterialAssign : EditorWindow
{
    private Object folderObject;
    private List<Object> fileObjects = new List<Object>();

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
                if (GUILayout.Button("Assign Materials from Folder", GUILayout.Height(40)))
                {
                    AssignMaterialsFromFolder(folderPath);
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

        GUILayout.Space(20);

        GUILayout.Label("Assign Materials to Selected Files", EditorStyles.boldLabel);

        // Drag-and-Drop Files Area
        Rect dropArea = GUILayoutUtility.GetRect(0.0f, 50.0f, GUILayout.ExpandWidth(true));
        GUI.Box(dropArea, "Drag and drop one or more FBX files here");

        HandleDragAndDrop(dropArea);

        if (fileObjects.Count > 0)
        {
            if (GUILayout.Button("Assign Materials to Selected Files", GUILayout.Height(40)))
            {
                AssignMaterialsToFiles(fileObjects.ToArray());
            }
        }
    }

    private void HandleDragAndDrop(Rect dropArea)
    {
        Event evt = Event.current;
        switch (evt.type)
        {
            case EventType.DragUpdated:
            case EventType.DragPerform:
                if (!dropArea.Contains(evt.mousePosition))
                    return;

                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                if (evt.type == EventType.DragPerform)
                {
                    DragAndDrop.AcceptDrag();

                    foreach (Object draggedObject in DragAndDrop.objectReferences)
                    {
                        if (draggedObject != null && AssetDatabase.GetAssetPath(draggedObject).EndsWith(".fbx"))
                        {
                            fileObjects.Add(draggedObject);
                        }
                    }
                }
                Event.current.Use();
                break;
        }
    }

    private void AssignMaterialsFromFolder(string folderPath)
    {
        string[] fbxFiles = Directory.GetFiles(folderPath, "*.fbx", SearchOption.AllDirectories);
        ProcessFBXFiles(fbxFiles);
    }

    private void AssignMaterialsToFiles(Object[] files)
    {
        string[] fbxFiles = new string[files.Length];

        for (int i = 0; i < files.Length; i++)
        {
            string assetPath = AssetDatabase.GetAssetPath(files[i]);
            fbxFiles[i] = assetPath;
        }

        ProcessFBXFiles(fbxFiles);
    }

    private void ProcessFBXFiles(string[] fbxFiles)
    {
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

                bool materialAssigned = CheckIfMaterialAssigned(importer);

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
                return true;
            }
        }

        return false;
    }
}

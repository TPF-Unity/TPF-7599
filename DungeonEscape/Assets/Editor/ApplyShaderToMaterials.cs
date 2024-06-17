using UnityEngine;
using UnityEditor;

public class ApplyShaderToMaterialsInFolder : EditorWindow
{
    private Shader targetShader;
    private string targetFolder = "";

    [MenuItem("Tools/Apply Shader to all Materials in Folder")]
    static void ShowWindow()
    {
        GetWindow<ApplyShaderToMaterialsInFolder>("Apply Shader");
    }

    void OnGUI()
    {
        targetShader = (Shader)EditorGUILayout.ObjectField("Target Shader", targetShader, typeof(Shader), false);

        GUILayout.Space(10);

        EditorGUILayout.LabelField("Target Folder:");
        targetFolder = EditorGUILayout.TextField(targetFolder);

        if (GUILayout.Button("Apply Shader"))
        {
            if (targetShader != null)
            {
                ApplyShaderToMaterials();
            }
        }
    }

    void ApplyShaderToMaterials()
    {
        string[] allMaterialGuids = AssetDatabase.FindAssets("t:Material", new[] { targetFolder });
        foreach (string materialGuid in allMaterialGuids)
        {
            string materialPath = AssetDatabase.GUIDToAssetPath(materialGuid);
            Material material = AssetDatabase.LoadAssetAtPath<Material>(materialPath);

            if (material != null)
            {
                material.shader = targetShader;
                EditorUtility.SetDirty(material);
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
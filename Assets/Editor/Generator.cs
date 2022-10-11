using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class Generator : EditorWindow
{
    private static bool error;
    private static Material _material;
    private static Material _material2;
    public Vector2 oceanSize = new Vector2(100, 100);
    public static void ShowWindow()
    {
        GetWindow<Generator>("Create New Island");
        
    }
    
    // TODO: Add a custom menu with customization options.
    [MenuItem("GameObject/Create New Island")]
    static void CreateNewIsland()
    {
        ShowWindow();
    }

    static void CreateIsland(Material mat)
    {
        // Create new Island
        GameObject gameObject = new GameObject("New Island", typeof(MeshRenderer), typeof(MeshFilter),
            typeof(MeshCollider), typeof(MeshGenerator));
        gameObject.GetComponent<MeshGenerator>().Generate();
        gameObject.GetComponent<MeshRenderer>().material = mat;
    }

    private void OnGUI()
    {
        GUILayout.Space(10);
        GUI.contentColor = Color.yellow;
        GUILayout.Label("*Note: You can find two sample materials in the Materials folder:\n Island Colored Material, Island Textured Material and Ocean Material", EditorStyles.wordWrappedLabel);
        GUI.contentColor = Color.white;
        GUILayout.Space(10);
        GUILayout.Label("Select the material you wanna apply for the island.");
        GUILayout.Space(10);
        _material = (Material)EditorGUILayout.ObjectField("Select Island material", _material, typeof(Material));
        if (error)
        {
            GUI.contentColor = Color.red;
            GUILayout.Label("You have to select a material");
            GUI.contentColor = Color.white;
        }
        GUILayout.Space(10);
        if (GUILayout.Button("Create new Island"))
        {
            if (!_material)
            {
                error = true;
                Debug.LogError("Select a material!");
            }
            else CreateIsland(_material);
        }
        
        GUILayout.Space(50);
        _material2 = (Material)EditorGUILayout.ObjectField("Select ocean material", _material2, typeof(Material));
        GUILayout.Space(10);
        oceanSize = EditorGUILayout.Vector2Field("Ocean Size", oceanSize);
        if (GUILayout.Button("Create Ocean"))
        {
            if (!_material2)
            {
                error = true;
                Debug.LogError("Select a material!");
            }
            else
            {
                GameObject ocean = GameObject.CreatePrimitive(PrimitiveType.Plane);
                ocean.transform.position = new Vector3(0, 1.5f, 0);
                ocean.transform.localScale = new Vector3(oceanSize.x, 1, oceanSize.y);
                ocean.GetComponent<MeshRenderer>().material = _material2;
                ocean.name = "Ocean";
            }
        }
    }
}

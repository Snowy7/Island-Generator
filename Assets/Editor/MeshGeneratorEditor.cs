using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor (typeof (MeshGenerator))]
public class MeshGeneratorEditor : Editor {

    public override void OnInspectorGUI() {
        MeshGenerator mapGen = (MeshGenerator)target;

        if (DrawDefaultInspector()) {
            if (mapGen.autoUpdate) {
                mapGen.Generate();
            }
        }

        if (GUILayout.Button ("Generate")) {
            mapGen.Generate ();
        }

        if (GUILayout.Button("Random Seed"))
        {
             mapGen.noiseSettings.seed = System.DateTime.Now.Millisecond;
             mapGen.Generate();
        }
    }
}

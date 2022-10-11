using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MeshGenerator : MonoBehaviour
{

     public bool autoUpdate = true;
     [Header("Settings")]
     [SerializeField] private MeshFilter meshFilter;
     [SerializeField] private MeshCollider meshCollider;
     [SerializeField] private Vector2Int size = new Vector2Int(100, 100);
     [SerializeField] private float height = 5;
     public NoiseSettings noiseSettings = new NoiseSettings();
     [SerializeField] private FalloffSettings _falloffSettings = new FalloffSettings();
     
     private Mesh _mesh;

     private void Start()
     {
          Generate();
     }

     public void Generate()
     {
          if (!meshFilter) meshFilter = GetComponent<MeshFilter>();
          if (!meshFilter) meshFilter = gameObject.AddComponent<MeshFilter>();
          if (!meshCollider) meshCollider = GetComponent<MeshCollider>();
          if (!meshCollider) meshCollider = gameObject.AddComponent<MeshCollider>();
          Mesh mesh = new Mesh();
          meshFilter.sharedMesh = mesh;
          mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
          
          mesh.vertices = CreateVertices();
          mesh.triangles = CreateTriangles();

          mesh.RecalculateNormals();
          meshCollider.sharedMesh = mesh;
     }

     private Vector3[] CreateVertices()
     {
          noiseSettings.ValidateValues();
          float[,] noiseMap = Noise.GenerateNoiseMap(size.x + 1, size.y + 1, noiseSettings.seed, noiseSettings.scale,
               noiseSettings.octaves, noiseSettings.persistance, noiseSettings.lactuanirty, noiseSettings.offset);

          float[,] falloffMap = FallOffGenerator.Generate(new Vector2Int(size.x + 1, size.y + 1), _falloffSettings.falloffStart, _falloffSettings.falloffEnd);
          Vector3[] vertices = new Vector3[(size.x + 1) * (size.y + 1)];

          for (int i = 0, z = 0; z <= size.y; z++)
          {
               for (int x = 0; x <= size.x; x++)
               {
                    float y = Mathf.Clamp01(noiseMap [x, z] - falloffMap [x, z]);
                    vertices[i] = new Vector3(x, y * height, z);
                    i++;
               }
          }

          return vertices;
     }

     int[] CreateTriangles()
     {
          int[] triangles = new int[size.x * size.y * 6];
          for (int tris = 0, vert = 0, z = 0; z < size.y; z++)
          {
               for (int x = 0; x < size.x; x++)
               {
                    triangles[tris + 0] = vert + 0;
                    triangles[tris + 1] = vert + size.x + 1;
                    triangles[tris + 2] = vert + 1;
                    triangles[tris + 3] = vert + 1;
                    triangles[tris + 4] = vert + size.x + 1;
                    triangles[tris + 5] = vert + size.x + 2;

                    tris += 6;
                    vert++;
               }
               vert++;
          }

          return triangles;
     }
}
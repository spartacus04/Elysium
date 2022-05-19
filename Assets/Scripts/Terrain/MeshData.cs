using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshData
{
    public Vector3[] Vertices { get; set; }
    public int[] Triangles { get; set; }
    public Vector2[] UVs { get; set; }
    
    private int triangleIndex;

    public MeshData(int meshWidth, int meshHeight, Terrain terrain, float multiplier, float treshold = 0.1f) {
        // Init
        Vertices = new Vector3[meshWidth * meshHeight];
        UVs = new Vector2[meshWidth * meshHeight];
        Triangles = new int[(meshWidth - 1) * (meshHeight - 1) * 6];

        float topLeftX = (terrain.Width - 1) / -2f;
        float topLeftZ = (terrain.Height - 1) / 2f;

        int i = 0;

        for (int y = 0; y < terrain.Height; y++)
        {
            for (int x = 0; x < terrain.Width; x++)
            {
                Vertices[i] = new Vector3(topLeftX + x, terrain.HeightMap[x, y] * multiplier, topLeftZ - y);
                UVs[i] = new Vector2(x / (float)terrain.Width, y / (float)terrain.Height);

                if (terrain.HeightMap[x, y] >= treshold) {
                    if (x < terrain.Width - 1 && y < terrain.Height - 1)
                    {
                        AddTriangle(i, i + terrain.Width + 1, i + terrain.Width);
                        AddTriangle(i, i + 1, i + terrain.Width + 1);
                    }

                }
                
                i++;
            }
        }
    }

    public void AddTriangle(int a, int b, int c)
    {
        Triangles[triangleIndex] = a;
        Triangles[triangleIndex + 1] = b;
        Triangles[triangleIndex + 2] = c;

        triangleIndex += 3;
    }

    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = Vertices;
        mesh.triangles = Triangles;
        mesh.uv = UVs;
        mesh.RecalculateNormals();
        return mesh;
    }
}
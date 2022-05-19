using System.Collections.Generic;
using UnityEngine;

public class MeshData
{
	public int chunkSize { get; protected set; }
	public struct Data {
    	public int triangleIndex {get; set; }
    	public Vector3[] Vertices { get; set; }
    	public int[] Triangles { get; set; }
    	public Vector2[] UVs { get; set; }

		public void AddTriangle(int a, int b, int c)
    	{
        	Triangles[triangleIndex] = a;
        	Triangles[triangleIndex + 1] = b;
        	Triangles[triangleIndex + 2] = c;

        	triangleIndex += 3;
    	}
	}

    public Data[,] meshesData { get; protected set; }

    public MeshData(Terrain terrain, int chunkSize, float multiplier, float treshold = 0.1f) {
		int width = terrain.Width / chunkSize;
		int height = terrain.Height / chunkSize;
		meshesData = new Data[width, height];

		float topLeftX = (terrain.Width - 1) / -2f;
        float topLeftZ = (terrain.Height - 1) / 2f;

		for(int chunkX = 0; chunkX < width; chunkX++) {
			for(int chunkY = 0; chunkY < height; chunkY++) {

				int chunkOffsetX = chunkX * chunkSize;
				int chunkOffsetY = chunkY * chunkSize;

				int i = 0;
				for(int x = 0; x < chunkSize; x++) {
					for(int y = 0; y < chunkSize; y++) {
						Data data = new Data();

						data.triangleIndex = 0;
						data.Vertices = new Vector3[chunkSize * chunkSize];
						data.Triangles = new int[chunkSize * chunkSize * 6];
						data.UVs = new Vector2[chunkSize * chunkSize];

						data.Vertices[i] = new Vector3(topLeftX + chunkOffsetX + x, terrain.HeightMap[x + chunkOffsetX, y + chunkOffsetY] * multiplier, topLeftZ - chunkOffsetY - y);
						data.UVs[i] = new Vector2((float)x / (float)chunkSize, (float)y / (float)chunkSize);

						//if (terrain.HeightMap[x, y] >= treshold) {
        	            	if (x < chunkSize - 1 && y < chunkSize - 1)
            	    	    {
            		            data.AddTriangle(i, i + chunkSize + 1, i + chunkSize);
        	        	        data.AddTriangle(i, i + 1, i + chunkSize + 1);
    	                	}
    	            	//}

						meshesData[chunkX, chunkY] = data;
						i++;
					}
				}

				Debug.Log("Chunk " + chunkX + ", " + chunkY + " created");
			};
		};
    }

    

    public List<Mesh> CreateMesh()
    {
		List<Mesh> meshes = new List<Mesh>();

		for(int x = 0; x < meshesData.GetLength(0); x++) {
			for(int y = 0; y < meshesData.GetLength(1); y++) {
				Data data = meshesData[x, y];

				Mesh mesh = new Mesh();
				mesh.vertices = data.Vertices;
				mesh.triangles = data.Triangles;
				mesh.uv = data.UVs;
				mesh.RecalculateNormals();

				meshes.Add(mesh);
			}
		}
		
        return meshes;
    }
}
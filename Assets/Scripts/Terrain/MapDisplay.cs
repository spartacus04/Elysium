using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class MapDisplay : MonoBehaviour
{
	public GameObject meshParent;
    public Material material;

	GameObject addNewMesh() {
		GameObject g = new GameObject("MeshPart");
		g.transform.parent = meshParent.transform;
		g.transform.localPosition = Vector3.zero;
		g.AddComponent(typeof(MeshRenderer));
		g.AddComponent(typeof(MeshFilter));
		g.AddComponent(typeof(MeshCollider));

		return g;
	}

    public void clear() {
        List<Transform> transforms = meshParent.transform.Cast<Transform>().ToList();
        foreach (Transform child in transforms)
        {
            DestroyImmediate(child.gameObject);
        }
    }

    public void DrawNoiseMap(float[,] noiseMap, int chunkSize, int heightMultiplier)
    {
        int width = noiseMap.GetLength(0);
        int height = noiseMap.GetLength(1);

        Texture2D texture = new Texture2D(width, height);

        Color[] colorMap = new Color[width * height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                colorMap[y * width + x] = Color.Lerp(Color.black, Color.white, noiseMap[x, y]);
            }
        }

        texture.SetPixels(colorMap);
        texture.Apply();

		foreach(Transform child in meshParent.transform) {
            child.GetComponent<MeshRenderer>().sharedMaterial = material;
    	    child.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = texture;
	        child.localScale = new Vector3(width, heightMultiplier, height);
		}
    }

    public void DrawMesh(MeshData data)
    {
		List<Mesh> meshes = data.CreateMesh();
        Debug.Log(meshes.Count);
        for(int i = 0; i < meshes.Count; i++)
		{
			GameObject g = addNewMesh();
			g.GetComponent<MeshFilter>().mesh = meshes[i];
		}
    }
}
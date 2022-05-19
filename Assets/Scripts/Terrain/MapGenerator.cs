using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public int chunksWidth;
    public int chunksHeight;
	public int chunkSize = 200;
    public float noiseScale;

    public int octaves;
    [Range(0, 1)]
    public float persistance;
    public float lacunarity;
    
    public int seed;
    public Vector2 offset;

    public bool autoUpdate;

    public void CreateMap()
    {
		int mapWidth = chunksWidth * chunkSize;
		int mapHeight = chunksHeight * chunkSize;

        Terrain t = new TerrainBuilder(mapWidth, mapHeight, noiseScale, seed, octaves, persistance, lacunarity, offset).Normalize().Build();
        Terrain outp = (t - Utils.GetFalloffMap(t));

        MapDisplay display = FindObjectOfType<MapDisplay>();
        display.DrawMesh(new MeshData(t, chunkSize, 1000));
        //display.DrawNoiseMap(outp.Normalized().HeightMap);
    }

    private void OnValidate()
    {
        if (chunksWidth < 1) chunksWidth = 1;
        if (chunksHeight < 1) chunksHeight = 1;
        if (lacunarity < 1) lacunarity = 1;
        if (octaves < 0) octaves = 0;
    }
}
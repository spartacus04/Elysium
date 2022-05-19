using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public int mapWidth;
    public int mapHeight;
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
        Terrain t = new TerrainBuilder(mapWidth, mapHeight, noiseScale, seed, octaves, persistance, lacunarity, offset).Normalize().Build();
        Terrain outp = (t - Utils.GetFalloffMap(t));

        MapDisplay display = FindObjectOfType<MapDisplay>();
        display.DrawNoiseMap(outp.Normalized().HeightMap);
        display.DrawMesh(MeshGenerator.GenerateTerrainMesh(outp, 1000));
    }

    private void OnValidate()
    {
        if (mapWidth < 1) mapWidth = 1;
        if (mapHeight < 1) mapHeight = 1;
        if (lacunarity < 1) lacunarity = 1;
        if (octaves < 0) octaves = 0;
    }
}
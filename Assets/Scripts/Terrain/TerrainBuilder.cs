using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainBuilder
{
    #region Props
    public int Width { get; private set; }
    public int Height { get; private set; }
    public int Depth { get; private set; }
    public float Scale { get; private set; }
    public int Seed { get; private set; }
    public int Octaves { get; private set; }
    public float Persistance { get; private set; }
    public float Lacunarity { get; private set; }
    public Vector2 Offset { get; private set; }

    public float[,] HeightMap { get; private set; }

    private float max = float.MinValue, min = float.MaxValue;
    #endregion

    #region Contructors
    public TerrainBuilder(int width, int height, float scale, int seed, int octaves, float persistance, float lacunarity, Vector2 offset)
    {
        this.Width = width < 1 ? 1 : width;
        this.Height = height < 1 ? 1 : height;
        this.Scale = scale <= 0 ? 0.001f : scale;
        this.Seed = seed;
        this.Octaves = octaves < 0 ? 0 : octaves;
        this.Persistance = persistance;
        this.Lacunarity = lacunarity < 1 ? 1 : lacunarity;
        this.Offset = offset;

        this.HeightMap = new float[this.Width, this.Height];
        Generate();
    }
    #endregion

    #region Methods
    public void Generate()
    {
        Vector2[] octaveOffsets = OctavesOffsets();

        float halfWidth = Width / 2f;
        float halfHeight = Height / 2f;

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                // Octave handling
                for (int i = 0; i < Octaves; i++)
                {
                    float sampleX = (x - halfWidth) / Scale * frequency + octaveOffsets[i].x;
                    float sampleY = (y - halfHeight) / Scale * frequency + octaveOffsets[i].y;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= Persistance;
                    frequency *= Lacunarity;
                }

                HeightMap[x, y] = noiseHeight;

                if (HeightMap[x, y] > max) max = HeightMap[x, y];
                if (HeightMap[x, y] < min) min = HeightMap[x, y];
            }
        }
    }

    public TerrainBuilder Normalize()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                HeightMap[x, y] = Mathf.InverseLerp(min, max, HeightMap[x, y]);
            }
        }

        min = 0;
        max = 1;

        return this;
    }

    public TerrainBuilder Invert()
    {
        float mid = (max - min) / 2 + min;
        
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                if (HeightMap[x, y] > mid)
                {
                    HeightMap[x, y] = mid - (HeightMap[x, y] - mid);
                }
                else if (HeightMap[x, y] < mid)
                {
                    HeightMap[x, y] = mid + (mid - HeightMap[x, y]);
                }
            }
        }

        return this;
    }

    public Terrain Build()
    {
        return new Terrain(HeightMap);
    }
    #endregion

    #region Utils
    Vector2[] OctavesOffsets()
    {
        System.Random prng = new System.Random(Seed);
        Vector2[] offsets = new Vector2[Octaves];
        for (int i = 0; i < Octaves; i++)
        {
            float offsetX = prng.Next(-100000, 100000) + Offset.x;
            float offsetY = prng.Next(-100000, 100000) + Offset.y;

            offsets[i] = new Vector2(offsetX, offsetY);
        }

        return offsets;
    }
    #endregion
}

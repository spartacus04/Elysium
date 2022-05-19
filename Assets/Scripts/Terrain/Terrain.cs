using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terrain
{
    #region Props
    public float[,] HeightMap { get; protected set; }
    public int Width { get; protected set; }
    public int Height { get; protected set; }
    #endregion

    #region Constructors
    public Terrain(float[,] heightMap)
    {
        this.HeightMap = heightMap;
        this.Width = heightMap.GetLength(0);
        this.Height = heightMap.GetLength(1);
    }

    public Terrain(Terrain terrain)
    {
        this.Width = terrain.Width;
        this.Height = terrain.Height;
        this.HeightMap = terrain.HeightMap;
    }

    #endregion

    #region Methods

    public Terrain Normalize()
    {

        float min = float.MaxValue;
        float max = float.MinValue;

        for (int x = 0; x < this.Width; x++)
        {
            for (int y = 0; y < this.Height; y++)
            {
                if (HeightMap[x, y] > max) max = HeightMap[x, y];
                if (HeightMap[x, y] < min) min = HeightMap[x, y];
            }
        }

        for (int x = 0; x < this.Width; x++)
        {
            for (int y = 0; y < this.Height; y++)
            {
                HeightMap[x, y] = Mathf.InverseLerp(min, max, HeightMap[x, y]);
            }
        }

        return this;
    }

    public Terrain Invert()
    {
        // Inverts the heightmap
        for (int x = 0; x < this.Width; x++)
        {
            for (int y = 0; y < this.Height; y++)
            {
                HeightMap[x, y] = 1 - HeightMap[x, y];
            }
        }

        return this;
    }

    public Terrain Normalized() {
        return new Terrain(this).Normalize();
    }

    public Terrain Scale(int width, int height)
    {
        float[,] scaledHeightMap = new float[width, height];

        float widthRatio = HeightMap.GetLength(0) / width;
        float heightRatio = HeightMap.GetLength(1) / height;
        
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                scaledHeightMap[x, y] = HeightMap[Mathf.RoundToInt(x * widthRatio), Mathf.RoundToInt(y * heightRatio)];
            }
        }

        return new Terrain(scaledHeightMap);
    }

    public Terrain Sum(Terrain terrain)
    {
        Terrain b = terrain.Scale(Width, Height);
        
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                HeightMap[x, y] += b.HeightMap[x, y];
            }
        }

        return this.Normalize();
    }

    public Terrain Subtract(Terrain terrain)
    {
        Terrain b = terrain.Scale(Width, Height);

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                HeightMap[x, y] -= b.HeightMap[x, y];
            }
        }

        return this.Normalize();
    }

    public Terrain Multiply(int value)
    {
        for (int i = 0; i < this.Width; i++)
        {
            for (int j = 0; j < this.Height; j++)
            {
                this.HeightMap[i, j] *= value;
            }
        }

        return this;
    }

    public Terrain Clone()
    {
        return new Terrain(this);
    }

    //TODO: textures
    #endregion

    #region Operators
    public static Terrain operator +(Terrain a, Terrain b)
    {
        Terrain terrain = new Terrain(b).Scale(a.Width, a.Height);

        for (int x = 0; x < terrain.Width; x++)
        {
            for (int y = 0; y < terrain.Height; y++)
            {
                terrain.HeightMap[x, y] += a.HeightMap[x, y];
            }
        }

        return terrain.Normalize();
    }

    public static Terrain operator -(Terrain a, Terrain b)
    {
        Terrain terrain = new Terrain(b).Scale(a.Width, a.Height);

        for (int x = 0; x < terrain.Width; x++)
        {
            for (int y = 0; y < terrain.Height; y++)
            {
                terrain.HeightMap[x, y] = Mathf.Clamp01(a.HeightMap[x, y] - b.HeightMap[x, y]);
            }
        }

        return terrain;
    }

    public static Terrain operator *(Terrain a, int b)
    {
        for (int x = 0; x < a.Width; x++)
        {
            for (int y = 0; y < a.Height; y++)
            {
                a.HeightMap[x, y] *= b;
            }
        }

        return a;
    }
    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static Terrain GetFalloffMap(Terrain terrain)
    {
        float[,] map = new float[terrain.Width, terrain.Height];

        for (int i = 0; i < terrain.Width; i++)
        {
            for (int j = 0; j < terrain.Height; j++)
            {
                float x = i / (float)terrain.Width * 2 - 1;
                float y = j / (float)terrain.Height * 2 - 1;

                map[i, j] = falloffCurve(Mathf.Max(Mathf.Abs(x), Mathf.Abs(y)));
            }
        }

        return new Terrain(map).Normalize();
    }
    
    private static float falloffCurve(float x)
    {
        float a = 2.5f;
        float b = 0.5f;

        return Mathf.Pow(x, a) / (Mathf.Pow(x, a) + Mathf.Pow(b - b * x, a));
    }
}

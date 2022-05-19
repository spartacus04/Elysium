using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseUtils : MonoBehaviour
{
    public static float[,] sum(List<float[,]> heightMaps)
    {
        // Check if heightMaps need scaling relative to first one
        int width = heightMaps[0].GetLength(0);
        int height = heightMaps[0].GetLength(1);

        float[,] sum = new float[width, height];

        for (int i = 0; i < heightMaps.Count; i++)
        {
            float[,] currentHeightMap = heightMaps[i];
            
            if (currentHeightMap.GetLength(0) != width || currentHeightMap.GetLength(1) != height)
            {
                currentHeightMap = scale(currentHeightMap, width, height);
            }
            
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    sum[x, y] += currentHeightMap[x, y];
                }
            }
        }

        return sum;
    }

    public static float[,] scale(float[,] heightMap, int width, int height)
    {
        float[,] scaledHeightMap = new float[width, height];

        float widthRatio = heightMap.GetLength(0) / width;
        float heightRatio = heightMap.GetLength(1) / height;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                scaledHeightMap[x, y] = heightMap[Mathf.RoundToInt(x * widthRatio), Mathf.RoundToInt(y * heightRatio)];
            }
        }
        
        return scaledHeightMap;
    }
}

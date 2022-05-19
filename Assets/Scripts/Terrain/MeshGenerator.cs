using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshGenerator
{
    public static MeshData GenerateTerrainMesh(Terrain terrain, float multiplier)
    {
        return new MeshData(terrain.Width, terrain.Height, terrain, multiplier);
    }
}

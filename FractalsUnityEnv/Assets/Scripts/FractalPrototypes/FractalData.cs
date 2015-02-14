using System;
using UnityEngine;

[Serializable]
public class FractalData
{
    //Maximum depth of the fractal. Child depth
    public int MaxDepth;

    //Amount to scale each child
    public float ChildScale;

    //Probability of a child to spawn
    [Range(0,1)]
    public float SpawnProbability;

    //All Meshes to be used;
    public Mesh[] Meshes;

    //Material to be cloned
    public Material FractalMaterial;

    //Max and min spawn delay
    public float MinSpawnDelay;
    public float MaxSpawnDelay;

}

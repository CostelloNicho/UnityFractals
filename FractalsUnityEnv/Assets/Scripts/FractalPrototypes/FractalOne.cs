// Copyright 2015 Nicholas Costello <NicholasJCostello@gmail.com>

using System.Collections;
using UnityEngine;

public class FractalOne : MonoBehaviour
{
    //Static number of children spawned
    public static int NumberOfChildren = 0;

    //All directions a fractal can spawn
    private static readonly Vector3[] ChildDirections =
    {
        Vector3.up,
        Vector3.right,
        Vector3.left,
        Vector3.forward,
        Vector3.back
    };

    //Orientation a child can spawn in
    private static readonly Quaternion[] ChildOrientations =
    {
        Quaternion.identity,
        Quaternion.Euler(0f, 0f, -90f),
        Quaternion.Euler(0f, 0f, 90f),
        Quaternion.Euler(90f, 0f, 0f),
        Quaternion.Euler(-90f, 0f, 0f)
    };

    //Data for the fractal
    public FractalData Fractal;

    //Materials used, 1 for each depth
    private Material[] _materials;

    //Current depth of this object
    private int _currentDepth = 0;

    protected void Start()
    {
        //increment number of children
        NumberOfChildren++;

        //If this is the first fractal object initialize the materils
        if (_materials == null)
            InitializeMaterials();

        //Make this object
        gameObject.AddComponent<MeshFilter>().mesh =
            Fractal.Meshes[Random.Range(0, Fractal.Meshes.Length)];
        gameObject.AddComponent<MeshRenderer>().material =
            _materials[_currentDepth];

        if (_currentDepth < Fractal.MaxDepth)
            StartCoroutine(CreateChildren());
    }

    private void Initialize(FractalOne parent, int childIndex)
    {
        //Get fractal data
        Fractal = parent.Fractal;
        //Set the materials
        _materials = parent._materials;
        //Configure the current depth
        _currentDepth = parent._currentDepth + 1;
        //Set as a child
        transform.parent = parent.transform;
        //Set location/rotation/scale
        transform.localPosition = ChildDirections[childIndex]*(0.5f + 0.5f*Fractal.ChildScale);
        transform.localRotation = ChildOrientations[childIndex];
        transform.localScale = Vector3.one*Fractal.ChildScale;
    }


    private void InitializeMaterials()
    {
        _materials = new Material[Fractal.MaxDepth + 1];
        for (var i = 0; i < _materials.Length; ++i)
        {
            var t = (float) i/(Fractal.MaxDepth);

            _materials[i] = new Material(Fractal.FractalMaterial);
            _materials[i].color = Color.Lerp(Color.blue, Color.white, t);
        }
        Debug.Log("Material Initialized");
    }

    private IEnumerator CreateChildren()
    {
        for (var i = 0; i < ChildDirections.Length; i++)
        {
            if (Random.value < Fractal.SpawnProbability)
            {
                yield return new WaitForSeconds(Random.Range(
                    Fractal.MinSpawnDelay, Fractal.MaxSpawnDelay));
                new GameObject(string.Format("Child Depth {0}", _currentDepth))
                    .AddComponent<FractalOne>().Initialize(this, i);
            }
        }
    }
}
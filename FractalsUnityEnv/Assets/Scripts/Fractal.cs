﻿// Copyright 2015 Nicholas Costello <NicholasJCostello@gmail.com>

using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Fractal : MonoBehaviour
{
    public Mesh[] Meshes;
    public Material FractalMat;

    public int MaxDepth = 4;
    public float ChildScale = 0.5f;

    private int _depth = 0;

    private static readonly Vector3[] ChildDirections =
    {
        Vector3.up,
        Vector3.right,
        Vector3.left,
        Vector3.forward,
		Vector3.back
    };

    private static readonly Quaternion[] ChildOrientations =
    {
        Quaternion.identity,
        Quaternion.Euler(0f, 0f, -90f),
        Quaternion.Euler(0f, 0f, 90f),
        Quaternion.Euler(90f, 0f, 0f),
		Quaternion.Euler(-90f, 0f, 0f)
    };

    private Material[] _materials;

    protected void Start()
    {

        if(_materials == null)
            InitializeMaterials();

        gameObject.AddComponent<MeshFilter>().mesh = Meshes[Random.Range(0, Meshes.Length)];
        gameObject.AddComponent<MeshRenderer>().material = _materials[_depth];

        if (_depth < MaxDepth)
            StartCoroutine(CreateChildren());
    }


    private void Initialize(Fractal parent, int childIndex)
    {
        Meshes = parent.Meshes;
        _materials = parent._materials;
        MaxDepth = parent.MaxDepth;
        _depth = parent._depth + 1;
        ChildScale = parent.ChildScale;
        transform.parent = parent.transform;
        transform.localScale = Vector3.one*ChildScale;
        transform.localPosition = ChildDirections[childIndex]*(0.5f + 0.5f*ChildScale);
        transform.localRotation = ChildOrientations[childIndex];
    }

    private void InitializeMaterials()
    {
        _materials = new Material[MaxDepth + 1];
        for (var i = 0; i < _materials.Length; ++i)
        {
            float t = i/(MaxDepth - 1f);
            t *= t;
            _materials[i] = new Material(FractalMat);
            _materials[i].color = Color.Lerp(Color.blue, Color.white, t);
        }
        _materials[MaxDepth].color = Color.magenta;

    }

    private IEnumerator CreateChildren()
    {
        for (var i = 0; i < ChildDirections.Length; i++)
        {
            yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));
            new GameObject("Fractal Child").AddComponent<Fractal>().Initialize(this, i);
        }
    }
}
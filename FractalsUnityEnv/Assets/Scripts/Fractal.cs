// Copyright 2015 Nicholas Costello <NicholasJCostello@gmail.com>

using System.Collections;
using UnityEngine;

public class Fractal : MonoBehaviour
{
    public Mesh FractalMesh;
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

    protected void Start()
    {
        gameObject.AddComponent<MeshFilter>().mesh = FractalMesh;
        gameObject.AddComponent<MeshRenderer>().material = FractalMat;

        if (_depth < MaxDepth)
            StartCoroutine(CreateChildren());
    }


    private void Initialize(Fractal parent, int childIndex)
    {
        FractalMesh = parent.FractalMesh;
        FractalMat = parent.FractalMat;
        MaxDepth = parent.MaxDepth;
        _depth = parent._depth + 1;
        ChildScale = parent.ChildScale;
        transform.parent = parent.transform;
        transform.localScale = Vector3.one*ChildScale;
        transform.localPosition = ChildDirections[childIndex]*(0.5f + 0.5f*ChildScale);
        transform.localRotation = ChildOrientations[childIndex];
    }

    private IEnumerator CreateChildren()
    {
        for (var i = 0; i < ChildDirections.Length; i++)
        {
            yield return new WaitForSeconds(0.5f);
            new GameObject("Fractal Child").AddComponent<Fractal>().Initialize(this, i);
        }
    }
}
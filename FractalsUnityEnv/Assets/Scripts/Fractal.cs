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

    protected void Start()
    {
        gameObject.AddComponent<MeshFilter>().mesh = FractalMesh;
        gameObject.AddComponent<MeshRenderer>().material = FractalMat;

        if (_depth < MaxDepth)
            StartCoroutine(CreateChildren());
    }


    private void Initialize(Fractal parent, Vector3 direction, Quaternion orientation)
    {
        FractalMesh = parent.FractalMesh;
        FractalMat = parent.FractalMat;
        MaxDepth = parent.MaxDepth;
        _depth = parent._depth + 1;
        ChildScale = parent.ChildScale;
        transform.parent = parent.transform;
        transform.localScale = Vector3.one*ChildScale;
        transform.localPosition = direction*(0.5f + 0.5f*ChildScale);
        transform.localRotation = orientation;
    }

    private IEnumerator CreateChildren()
    {
        yield return new WaitForSeconds(0.5f);
        new GameObject("Fractal Child").AddComponent<Fractal>()
            .Initialize(this, Vector3.up, Quaternion.identity);

        yield return new WaitForSeconds(0.5f);
        new GameObject("Fractal Child").AddComponent<Fractal>()
            .Initialize(this, Vector3.right, Quaternion.Euler(0f, 0f, -90f));

        yield return new WaitForSeconds(0.5f);
        new GameObject("Fractal Child").AddComponent<Fractal>()
            .Initialize(this, Vector3.left, Quaternion.Euler(0f, 0f, 90f));
    }
}
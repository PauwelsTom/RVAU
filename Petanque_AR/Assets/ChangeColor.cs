using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    public float r;
    public float g;
    public float b;

    // Update is called once per frame
    void Update()
    {
        
    }

    public MeshRenderer meshRenderer;

    private void Start()
    {
        Color redColor = new Color(r, g, b);
        meshRenderer.material.SetColor("_Color", Color.red);
    }
};

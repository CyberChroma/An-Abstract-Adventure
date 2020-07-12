using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TetherLine : MonoBehaviour
{
    public Transform cubePos;
    public Transform spherePos;

    private LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        lineRenderer.SetPosition(0, cubePos.position);
        lineRenderer.SetPosition(1, (cubePos.position + spherePos.position) / 2);
        lineRenderer.SetPosition(2, spherePos.position);
    }
}

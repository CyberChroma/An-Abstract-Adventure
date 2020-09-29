using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TetherLine : MonoBehaviour
{
    public Transform cubePlayer;
    public Transform spherePlayer;
    public float widthMultiplier;
    public float maxWidth;

    private Vector2 cubePos;
    private Vector2 spherePos;
    private float dis;
    private LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        cubePos = cubePlayer.position;
        spherePos = spherePlayer.position;
        lineRenderer.SetPosition(0, cubePos);
        lineRenderer.SetPosition(1, (cubePos + spherePos) / 2);
        lineRenderer.SetPosition(2, spherePos);
        float dis = (cubePos - spherePos).magnitude;
        if (dis != 0)
        {
            lineRenderer.widthMultiplier = Mathf.Min(1 / dis * widthMultiplier, maxWidth);
        }
    }
}

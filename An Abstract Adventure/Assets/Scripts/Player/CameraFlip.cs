using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class CameraFlip : MonoBehaviour
{
    private Camera cam;

    void Awake()
    {
        cam = GetComponent<Camera>();
    }

    void OnEnable()
    {
        cam.projectionMatrix *= Matrix4x4.Scale(new Vector3(-1, 1, 1));
    }

    void OnDisable()
    {
        cam.projectionMatrix *= Matrix4x4.Scale(new Vector3(-1, 1, 1));
    }
}

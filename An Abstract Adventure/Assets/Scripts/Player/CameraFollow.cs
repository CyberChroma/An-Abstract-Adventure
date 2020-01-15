using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float smoothing;
    public float offsetHeight;
    public float zoom;
    public float zoomSmoothing;

    [HideInInspector] public Transform player;
    [HideInInspector] public Transform target;
    [HideInInspector] public float tSmoothing;
    [HideInInspector] public float tOffsetHeight;
    [HideInInspector] public float tZoom;
    [HideInInspector] public float tZoomSmoothing;

    private Vector3 movePos;
    private Vector3 camVelocity;
    private float zoomVelocity;
    private Camera mCam;

    void Awake()
    {
        if (!GetComponent<PlayerChange>())
        {
            if (GameObject.Find("Kall"))
            {
                player = GameObject.Find("Kall").transform;
            }
            else
            {
                player = GameObject.Find("Que").transform;
            }
        }
        mCam = GetComponent<Camera>();
        mCam.fieldOfView = zoom;
    }

    void FixedUpdate()
    {
        if (target == null)
        {
            movePos = Vector3.SmoothDamp(transform.position, new Vector3(player.position.x, player.position.y + offsetHeight, transform.position.z), ref camVelocity, smoothing, Mathf.Infinity, Time.deltaTime);
            mCam.fieldOfView = Mathf.SmoothDamp(mCam.fieldOfView, zoom, ref zoomVelocity, zoomSmoothing, Mathf.Infinity, Time.deltaTime);
        }
        else
        {
            movePos = Vector3.SmoothDamp(transform.position, new Vector3(target.position.x, target.position.y + tOffsetHeight, transform.position.z), ref camVelocity, tSmoothing, Mathf.Infinity, Time.deltaTime);
            mCam.fieldOfView = Mathf.SmoothDamp(mCam.fieldOfView, tZoom, ref zoomVelocity, tZoomSmoothing, Mathf.Infinity, Time.deltaTime);
        }
        transform.position = movePos;
    }
}

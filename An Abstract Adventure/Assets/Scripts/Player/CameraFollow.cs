using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float smoothing;
    public float velocityDisX;
    public float velocityDisY;
    public float offsetHeight;
    public float zoom;
    public float zoomSmoothing;

    [HideInInspector] public Rigidbody2D player;
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
                player = GameObject.Find("Kall").GetComponent<Rigidbody2D>();
            }
            else
            {
                player = GameObject.Find("Que").GetComponent<Rigidbody2D>();
            }
        }
        mCam = GetComponent<Camera>();
        mCam.fieldOfView = zoom;
    }

    void FixedUpdate()
    {
        if (target == null)
        {
            movePos = Vector3.SmoothDamp(transform.position, new Vector3(player.position.x + player.velocity.x * velocityDisX, player.position.y + player.velocity.y * velocityDisY + offsetHeight, -10), ref camVelocity, smoothing, Mathf.Infinity, Time.deltaTime);
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

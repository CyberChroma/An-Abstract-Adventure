using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float smoothing;
    public float velocityDisX;
    public float velocityDisY;
    public float cameraDis;
    public float offsetHeight;
    public float zoom;
    public float zoomSmoothing;
    public Vector2 maxVelocity;

    [HideInInspector] public Rigidbody player;
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
                player = GameObject.Find("Kall").GetComponent<Rigidbody>();
            }
            else
            {
                player = GameObject.Find("Que").GetComponent<Rigidbody>();
            }
        }
        mCam = GetComponent<Camera>();
        mCam.fieldOfView = zoom;
    }

    void FixedUpdate()
    {
        if (target == null)
        {
            movePos = Vector3.SmoothDamp(transform.position, new Vector3(player.position.x + Mathf.Min(Mathf.Abs(player.velocity.x), maxVelocity.x) * player.velocity.normalized.x * velocityDisX, player.position.y + Mathf.Min(Mathf.Abs(player.velocity.y), maxVelocity.y) * player.velocity.normalized.y * velocityDisY + offsetHeight, player.position.z + -cameraDis), ref camVelocity, smoothing, Mathf.Infinity, Time.deltaTime);
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

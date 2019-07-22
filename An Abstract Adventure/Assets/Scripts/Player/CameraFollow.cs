using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public enum ActivePlayer
    {
        Kall,
        Que
    }

    public ActivePlayer activePlayer;
    public float smoothing;
    public float offsetHeight;
    public float zoom;
    public float zoomSmoothing;
    public bool canSwitch;

    [HideInInspector] public Transform target;
    [HideInInspector] public float tSmoothing;
    [HideInInspector] public float tOffsetHeight;
    [HideInInspector] public float tZoom;
    [HideInInspector] public float tZoomSmoothing;

    private Vector3 movePos;
    private Vector3 camVelocity;
    private float zoomVelocity;
    private Camera mCam;
    private Transform kall;
    private Transform que;

    void Awake()
    {
        kall = GameObject.Find("Kall").transform;
        que = GameObject.Find("Que").transform;
        mCam = GetComponent<Camera>();
        mCam.orthographicSize = zoom;
    }

    private void Update()
    {
        if (canSwitch && Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (activePlayer == ActivePlayer.Kall)
            {
                activePlayer = ActivePlayer.Que;
                kall.GetComponent<CircleMain>().enabled = false;
                que.GetComponent<CircleMain>().enabled = true;
            }
            else
            {
                activePlayer = ActivePlayer.Kall;
                que.GetComponent<CircleMain>().enabled = false;
                kall.GetComponent<CircleMain>().enabled = true;
            }
        }
    }

    void LateUpdate()
    {
        if (target == null)
        {
            if (activePlayer == ActivePlayer.Kall)
            {
                movePos = Vector3.SmoothDamp(transform.position, new Vector3(kall.position.x, kall.position.y + offsetHeight, -10), ref camVelocity, smoothing, Mathf.Infinity, Time.deltaTime);
            } else
            {
                movePos = Vector3.SmoothDamp(transform.position, new Vector3(que.position.x, que.position.y + offsetHeight, -10), ref camVelocity, smoothing, Mathf.Infinity, Time.deltaTime);
            }
            mCam.orthographicSize = Mathf.SmoothDamp(mCam.orthographicSize, zoom, ref zoomVelocity, zoomSmoothing, Mathf.Infinity, Time.deltaTime);
        }
        else
        {
            movePos = Vector3.SmoothDamp(transform.position, new Vector3(target.position.x, target.position.y + tOffsetHeight, -10), ref camVelocity, tSmoothing, Mathf.Infinity, Time.deltaTime);
            mCam.orthographicSize = Mathf.SmoothDamp(mCam.orthographicSize, tZoom, ref zoomVelocity, tZoomSmoothing, Mathf.Infinity, Time.deltaTime);
        }
        transform.position = movePos;
    }
}

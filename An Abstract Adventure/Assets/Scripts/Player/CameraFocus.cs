using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFocus : MonoBehaviour
{
    public Transform targetToSet;
    public float smoothing = 0.5f;
    public float offsetHeight = 0;
    public float zoom = 10;
    public float zoomSmoothing = 0.5f;

    public float startDelay = 1;
    public float focusTime = 2;
    public float endDelay = 1;
    public bool oneTime = true;

    private CameraFollow cameraFollow;
    private CircleMain circleMain;

    void Awake()
    {
        cameraFollow = FindObjectOfType<CameraFollow>();
        circleMain = FindObjectOfType<CircleMain>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(ChangeTarget());
    }

    IEnumerator ChangeTarget() {
        circleMain.enabled = false;
        yield return new WaitForSeconds(startDelay);
        cameraFollow.target = targetToSet;
        cameraFollow.tSmoothing = smoothing;
        cameraFollow.tOffsetHeight = offsetHeight;
        cameraFollow.tZoom = zoom;
        cameraFollow.tZoomSmoothing = zoomSmoothing;
        yield return new WaitForSeconds(focusTime);
        cameraFollow.target = null;
        yield return new WaitForSeconds(endDelay);
        circleMain.enabled = true;
        if (oneTime)
        {
            Destroy(gameObject);
        }
    }
}

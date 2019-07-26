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
    private SquareMain squareMain;

    void Awake()
    {
        cameraFollow = FindObjectOfType<CameraFollow>();
        circleMain = FindObjectOfType<CircleMain>();
        squareMain = FindObjectOfType<SquareMain>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(ChangeTarget());
        }
    }

    IEnumerator ChangeTarget() {
        if (circleMain)
        {
            circleMain.enabled = false;
        }
        if (squareMain)
        {
            squareMain.enabled = false;
        }
        yield return new WaitForSeconds(startDelay);
        cameraFollow.target = targetToSet;
        cameraFollow.tSmoothing = smoothing;
        cameraFollow.tOffsetHeight = offsetHeight;
        cameraFollow.tZoom = zoom;
        cameraFollow.tZoomSmoothing = zoomSmoothing;
        yield return new WaitForSeconds(focusTime);
        cameraFollow.target = null;
        yield return new WaitForSeconds(endDelay);
        if (circleMain)
        {
            circleMain.enabled = true;
        }
        if (squareMain)
        {
            squareMain.enabled = true;
        }
        if (oneTime)
        {
            Destroy(gameObject);
        }
    }
}

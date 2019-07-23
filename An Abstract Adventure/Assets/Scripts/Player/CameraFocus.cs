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
    private CircleMain kallMain;
    private CircleMain queMain;

    void Awake()
    {
        cameraFollow = FindObjectOfType<CameraFollow>();
        kallMain = FindObjectsOfType<CircleMain>()[0];
        queMain = FindObjectsOfType<CircleMain>()[1];
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(ChangeTarget());
        }
    }

    IEnumerator ChangeTarget() {
        if (kallMain)
        {
            kallMain.enabled = false;
        } else if (queMain)
        {
            queMain.enabled = false;
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
        if (kallMain)
        {
            kallMain.enabled = true;
        }
        else if (queMain)
        {
            queMain.enabled = true;
        }
        if (oneTime)
        {
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFocus : MonoBehaviour
{
    public Transform targetToSet;
    public float smoothing = 0.5f;
    public float offsetHeight = 0;
    public float zoom = 70;
    public float zoomSmoothing = 0.5f;

    public float startDelay = 1;
    public float focusTime = 2;
    public float endDelay = 1;
    public bool oneTime = true;

    private CameraFollow cameraFollow;
    private PlayerMain[] playerMain;

    void Awake()
    {
        cameraFollow = FindObjectOfType<CameraFollow>();
        playerMain = FindObjectsOfType<PlayerMain>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(ChangeTarget());
        }
    }

    IEnumerator ChangeTarget() {
        playerMain[0].enabled = false;
        if (playerMain.Length == 2)
        {
            playerMain[1].enabled = false;
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
        playerMain[0].enabled = true;
        if (playerMain.Length == 2)
        {
            playerMain[1].enabled = true;
        }
        if (oneTime)
        {
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeMove : MonoBehaviour
{
    public float fadeInDelay;
    public float moveDelay;
    public float fadeOutDelay;
    public float endDelay;
    public Transform moveToPoint;

    private BlackFade blackFade;
    private Transform mCam;

    // Start is called before the first frame update
    void Start()
    {
        blackFade = FindObjectOfType<BlackFade>();
        mCam = FindObjectOfType<OldCameraFollow>().transform;
    }

    public void MovePlayer (OldPlayerMain playerMain)
    {
        StartCoroutine(WaitToMove(playerMain));
    }

    IEnumerator WaitToMove(OldPlayerMain playerMain)
    {
        playerMain.enabled = false;
        yield return new WaitForSeconds(fadeInDelay);
        blackFade.fadeBlack = true;
        blackFade.fading = true;
        yield return new WaitForSeconds(moveDelay);
        playerMain.transform.position = moveToPoint.position;
        mCam.transform.position = new Vector3 (moveToPoint.position.x, moveToPoint.position.y + 7, moveToPoint.position.z - 30);
        yield return new WaitForSeconds(fadeOutDelay);
        blackFade.fadeBlack = false;
        blackFade.fading = true;
        yield return new WaitForSeconds(endDelay);
        playerMain.enabled = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereSlam : MonoBehaviour
{
    public bool slamUnlocked;
    public float slamSpeed;
    public float stopTime;

    [HideInInspector] public bool isSlaming;
    [HideInInspector] public bool isSlamPaused;

    private PlayerMain playerMain;

    // Start is called before the first frame update
    void Start()
    {
        playerMain = GetComponent<PlayerMain>();
    }

    void OnTriggerStay(Collider collision)
    {
        // Slam
        if (isSlaming && collision.CompareTag("Slam"))
        {
            collision.gameObject.SetActive(false);
        }
    }

    public IEnumerator Slam()
    {
        playerMain.rb.useGravity = false;
        isSlamPaused = true;
        yield return new WaitForSeconds(stopTime);
        isSlaming = true;
        isSlamPaused = false;
        while (!playerMain.playerGroundDetection.isGrounded)
        {
            yield return null;
        }
        playerMain.rb.useGravity = true;
        isSlaming = false;
    }

    public void StopSlamEarly()
    {
        StopCoroutine(Slam());
        isSlamPaused = false;
        playerMain.rb.useGravity = true;
        isSlaming = false;
    }
}

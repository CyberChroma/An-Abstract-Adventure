using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereBounce : MonoBehaviour
{
    public bool slamBounceUnlocked;
    public float force;

    [HideInInspector] public bool canSlamBounce;
    [HideInInspector] public bool isSlamBouncing;

    private PlayerMain playerMain;

    // Start is called before the first frame update
    void Start()
    {
        playerMain = GetComponent<PlayerMain>();
    }

    public float SlamBounce()
    {
        float slamBounceVel = force * 10;
        playerMain.playerJump.canJump = false;
        StopCoroutine(playerMain.playerJump.StoreJumpInput());
        playerMain.playerJump.jumpInput = false;
        playerMain.playerGroundDetection.isGrounded = false;
        playerMain.playerJump.jumpHeld = false;
        StopCoroutine(playerMain.sphereSlam.Slam());
        playerMain.rb.useGravity = true;
        playerMain.sphereSlam.isSlaming = false;
        canSlamBounce = false;
        isSlamBouncing = true;
        return slamBounceVel;
    }
}

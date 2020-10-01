using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeDashLaunch : MonoBehaviour
{
    public bool dashLaunchUnlocked;
    public float launchForce;

    [HideInInspector] public bool canDashLaunch;

    private PlayerMain playerMain;

    // Start is called before the first frame update
    void Start()
    {
        playerMain = GetComponent<PlayerMain>();
    }

    public float DashLaunch()
    {
        float dashWallJumpVel = 0;
        dashWallJumpVel = launchForce * 10;
        playerMain.playerJump.canJump = false;
        StopCoroutine(playerMain.playerJump.StoreJumpInput());
        playerMain.playerJump.jumpInput = false;
        playerMain.playerGroundDetection.isGrounded = false;
        playerMain.playerJump.jumpHeld = true;
        StopCoroutine(playerMain.cubeWallJump.StoreWallJumpInput());
        playerMain.cubeWallJump.wallJumpInput = false;
        StopCoroutine(playerMain.cubeDash.Dash());
        playerMain.rb.useGravity = true;
        playerMain.playerMove.moveDir = 0;
        playerMain.cubeDash.isDashing = false;
        canDashLaunch = false;
        return dashWallJumpVel;
    }
}

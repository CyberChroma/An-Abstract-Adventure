using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    public float jumpForce;
    public float lowJumpMultiplier;
    public float fallMultiplier;
    public float gravityMultiplier;
    public float terminalVelocity;

    [HideInInspector] public bool canJump;
    [HideInInspector] public bool jumpInput;
    [HideInInspector] public bool jumpHeld;

    private PlayerMain playerMain;

    // Start is called before the first frame update
    void Start()
    {
        playerMain = GetComponent<PlayerMain>();
    }

    public float Jump()
    {
        float jumpVel = 0;
        if (canJump && jumpInput)
        {
            jumpVel = jumpForce * 10;
            canJump = false;
            StopCoroutine(StoreJumpInput());
            jumpInput = false;
            playerMain.playerGroundDetection.isGrounded = false;
            jumpHeld = true;
            if (playerMain.cubeWallJump.wallJumpUnlocked)
            {
                StopCoroutine(playerMain.cubeWallJump.StoreWallJumpInput());
                playerMain.cubeWallJump.wallJumpInput = false;
            }
        }
        return jumpVel;
    }

    public float Fall()
    {
        float fallVel = playerMain.rb.velocity.y;
        if (playerMain.sphereBounce.slamBounceUnlocked && playerMain.sphereBounce.isSlamBouncing)
        {
            fallVel -= gravityMultiplier * 10 * Time.deltaTime;
            if (playerMain.rb.velocity.y < 0)
            {
                playerMain.sphereBounce.isSlamBouncing = false;
            }
        }
        else if (playerMain.activePlayer == ActivePlayer.Sphere && playerMain.sphereGlide.glideUnlocked && !playerMain.playerGroundDetection.isGrounded && playerMain.rb.velocity.y < 0 && jumpHeld)
        {
            fallVel = -playerMain.sphereGlide.fallVelocity;
        }
        else
        {
            fallVel -= gravityMultiplier * 10 * Time.deltaTime;
            if (playerMain.rb.velocity.y >= 0 && !jumpHeld && playerMain.cubeWallJump.currWallJumpVelocity == Vector2.zero)
            {
                fallVel -= lowJumpMultiplier * 10 * Time.deltaTime;
            }
            else if (playerMain.rb.velocity.y < 0)
            {
                fallVel -= fallMultiplier * 10 * Time.deltaTime;
                if (playerMain.activePlayer == ActivePlayer.Cube && playerMain.cubeWallJump.wallJumpUnlocked && playerMain.cubeWallJump.wallContact)
                {
                    fallVel /= 2;
                }
            }
        }
        if (fallVel < -terminalVelocity)
        {
            fallVel = -terminalVelocity;
        }
        return fallVel;
    }

    public IEnumerator StoreJumpInput()
    {
        yield return new WaitForSeconds(playerMain.playerGroundDetection.jumpInputStoreTime);
        jumpInput = false;
    }
}

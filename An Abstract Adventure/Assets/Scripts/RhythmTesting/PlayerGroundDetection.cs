﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundDetection : MonoBehaviour
{
    public float jumpInputStoreTime;
    public float fallJumpDelay;
    [HideInInspector] public bool isGrounded;

    private PlayerMain playerMain;

    // Start is called before the first frame update
    void Start()
    {
        playerMain = GetComponent<PlayerMain>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 9 || collision.gameObject.layer == 10)
        {
            // Wall Jump
            playerMain.cubeWallJump.currWallJumpVelocity = Vector2.zero;

            // Ground Check
            if (!isGrounded && Mathf.Abs(collision.contacts[0].normal.x) < 0.9f && collision.contacts[0].point.y < transform.position.y)
            {
                // Jump
                isGrounded = true;
                playerMain.playerJump.canJump = true;

                // Wall Jump and Dash
                playerMain.cubeDash.canDash = true;

                // Slam Bounce
                if (playerMain.activePlayer == ActivePlayer.Sphere && playerMain.sphereBounce.slamBounceUnlocked && playerMain.sphereSlam.isSlaming)
                {
                    playerMain.sphereBounce.canSlamBounce = true;
                }
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        // Ground Check
        if (isGrounded && (collision.gameObject.layer == 9 || collision.gameObject.layer == 10) && !Physics.Raycast(transform.position, -transform.up, 0.5f, 1 << 8))
        {
            isGrounded = false;
            StopCoroutine(WaitToCancelJump());
            StartCoroutine(WaitToCancelJump());
        }
    }

    IEnumerator WaitToCancelJump()
    {
        int layermask1 = 1 << 9;
        int layermask2 = 1 << 10;
        int finalmask = layermask1 | layermask2;
        yield return new WaitForSeconds(fallJumpDelay);
        if (!Physics.Raycast(transform.position, -Vector3.up, 0.6f, finalmask))
        {
            playerMain.playerJump.canJump = false;
        }
    }

    public void SwitchGroundCheck ()
    {
        int layermask1 = 1 << 9;
        int layermask2 = 1 << 10;
        int finalmask = layermask1 | layermask2;
        if (Physics.Raycast(transform.position, -Vector3.up, 0.6f, finalmask))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
}

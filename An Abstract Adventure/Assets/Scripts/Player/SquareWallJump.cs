﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareWallJump : MonoBehaviour
{
    public float wallJumpHForce;
    public float wallJumpVForce;

    [HideInInspector] public bool canWallJump;

    private Rigidbody2D rb;
    private PlayerGroundCheck playerGroundCheck;
    private PlayerMove playerMove;
    private SquareMain squareMain;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerGroundCheck = GetComponentInChildren<PlayerGroundCheck>();
        playerMove = GetComponent<PlayerMove>();
        squareMain = GetComponent<SquareMain>();
        canWallJump = true;
    }


    public void WallJump()
    {
        if (canWallJump && !playerGroundCheck.isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = Vector2.zero;
            StopAllCoroutines();
            if (playerMove.frontRight)
            {
                rb.AddForce(transform.up * wallJumpVForce * 10 + -transform.right * wallJumpHForce * 10, ForceMode2D.Impulse);
                StartCoroutine(InputOveride(0.5f, -transform.right));
            }
            else
            {
                rb.AddForce(transform.up * wallJumpVForce * 10 + transform.right * wallJumpHForce * 10, ForceMode2D.Impulse);
                StartCoroutine(InputOveride(0.5f, transform.right));

            }
            rb.gravityScale = 1;
            playerMove.frontRight = !playerMove.frontRight;
            canWallJump = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        print(collision.contacts[0].normal);
        if (collision.gameObject.layer == 8 && Mathf.Abs(collision.contacts[0].normal.x) >= 0.9f)
        {
            StartCoroutine(WaitToTestCollision());
        }
    }
    
    IEnumerator WaitToTestCollision()
    {
        yield return new WaitForSeconds(0.01f);
        if (!playerGroundCheck.isGrounded)
        {
            StopAllCoroutines();
            StartCoroutine(InputOveride(0.2f, Vector2.zero));
            StartCoroutine(SlowFall(0.5f));
            canWallJump = true;
        }
        else
        {
            canWallJump = false;
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (canWallJump && collision.gameObject.layer == 8)
        {
            StopAllCoroutines();
            rb.gravityScale = 1;
            canWallJump = false;
            playerMove.moveOverride = false;
        }
    }

    IEnumerator InputOveride(float delay, Vector2 moveDir)
    {
        playerMove.moveOverride = true;
        playerMove.moveDir = moveDir;
        yield return new WaitForSeconds(delay);
        playerMove.moveOverride = false;
    }

    IEnumerator SlowFall(float delay)
    {
        rb.velocity = Vector2.zero;
        rb.gravityScale = -2;
        yield return new WaitForSeconds(delay);
        rb.gravityScale = 1;
    }
}

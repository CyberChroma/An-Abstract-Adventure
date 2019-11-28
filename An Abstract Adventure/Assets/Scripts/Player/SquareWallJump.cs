using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareWallJump : MonoBehaviour
{
    public float wallJumpHForce;
    public float wallJumpVForce;

    [HideInInspector] public bool canWallJump;
    [HideInInspector] public bool fallOverride;

    private Rigidbody2D rb;
    private Animator anim;
    private PlayerGroundCheck playerGroundCheck;
    private PlayerMove playerMove;
    private SquareMain squareMain;
    private bool wallContact;
    private bool otherContact;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        playerGroundCheck = GetComponentInChildren<PlayerGroundCheck>();
        playerMove = GetComponent<PlayerMove>();
        squareMain = GetComponent<SquareMain>();
    }


    public void WallJump()
    {
        if (canWallJump && !playerGroundCheck.isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            if (anim)
            {
                anim.SetTrigger("Jump");
            }
            rb.velocity = Vector2.zero;
            StopAllCoroutines();
            if (playerMove.frontRight)
            {
                rb.AddForce(transform.up * wallJumpVForce * 10 + -transform.right * wallJumpHForce * 10, ForceMode2D.Impulse);
                StartCoroutine(InputOveride(0.3f, -transform.right));
            }
            else
            {
                rb.AddForce(transform.up * wallJumpVForce * 10 + transform.right * wallJumpHForce * 10, ForceMode2D.Impulse);
                StartCoroutine(InputOveride(0.3f, transform.right));
            }
            rb.gravityScale = 1;
            playerMove.Flip();
            canWallJump = false;
            wallContact = false;
            otherContact = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8 && !collision.collider.CompareTag("Slippery"))
        {
            if (Mathf.Abs(collision.contacts[0].normal.x) >= 0.9f)
            {
                StartCoroutine(WaitToTestCollision());
                if (wallContact)
                {
                    otherContact = true;
                }
                else
                {
                    wallContact = true;
                }
            }
            else
            {
                otherContact = true;
            }
        }
    }  

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8 && !collision.collider.CompareTag("Slippery"))
        {
            if (canWallJump)
            {
                StopAllCoroutines();
                rb.gravityScale = 1;
                canWallJump = false;
                playerMove.moveOverride = false;
            }
            if (otherContact)
            {
                otherContact = false;
                if (wallContact)
                {
                    canWallJump = true;
                }
            }
            else
            {
                wallContact = false;
            }
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

    IEnumerator InputOveride(float delay, Vector2 moveDir)
    {
        playerMove.moveOverride = true;
        playerMove.moveDir = moveDir;
        yield return new WaitForSeconds(delay);
        playerMove.moveOverride = false;
    }

    IEnumerator SlowFall(float delay)
    {
        if (!fallOverride)
        {
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0.1f;
        }
        yield return new WaitForSeconds(delay);
        if (!fallOverride)
        {
            rb.gravityScale = 1;
        }
    }
}

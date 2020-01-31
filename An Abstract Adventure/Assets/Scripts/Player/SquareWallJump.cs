using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareWallJump : MonoBehaviour
{
    public float wallJumpHForce;
    public float wallJumpVForce;

    [HideInInspector] public bool canWallJump;
    [HideInInspector] public bool fallOverride;

    private Rigidbody rb;
    private Animator anim;
    private PlayerGroundCheck playerGroundCheck;
    private PlayerMove playerMove;
    private SquareMain squareMain;
    private bool wallContact;
    private bool otherContact;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
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
                anim.SetTrigger("WallJump");
            }
            rb.velocity = Vector3.zero;
            StopAllCoroutines();
            playerMove.frontDir *= -1;
            rb.AddForce(transform.up * wallJumpVForce * 10 + Vector3.right * playerMove.frontDir * wallJumpHForce * 10, ForceMode.Impulse);
            StartCoroutine(InputOveride(0.3f, Vector3.right * playerMove.frontDir));
            rb.useGravity = true;
            canWallJump = false;
            wallContact = false;
            otherContact = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
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

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == 8 && !collision.collider.CompareTag("Slippery"))
        {
            if (canWallJump)
            {
                StopAllCoroutines();
                rb.useGravity = true;
                canWallJump = false;
                anim.SetBool("IsFalling", true);
                playerMove.moveOverride = false;
            }
            if (otherContact)
            {
                otherContact = false;
                if (wallContact)
                {
                    canWallJump = true;
                    if (anim)
                    {
                        anim.SetBool("IsFalling", false);
                        anim.SetTrigger("WallSlide");
                    }
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
            StartCoroutine(InputOveride(0.2f, Vector3.zero));
            StartCoroutine(SlowFall(0.25f));
            if (anim)
            {
                anim.SetTrigger("WallGrab");
                anim.SetBool("IsFalling", false);
            }
            canWallJump = true;
        }
        else
        {
            canWallJump = false;
        }
    }

    IEnumerator InputOveride(float delay, Vector3 moveDir)
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
            rb.velocity = Vector3.zero;
            rb.useGravity = false;
        }
        yield return new WaitForSeconds(delay);
        if (!fallOverride)
        {
            rb.useGravity = true;
        }
    }
}

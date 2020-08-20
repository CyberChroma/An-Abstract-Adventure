using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetherCubeWallJump : MonoBehaviour
{
    public float wallJumpHForce;
    public float wallJumpVForce;

    [HideInInspector] public bool canWallJump;
    [HideInInspector] public bool fallOverride;

    private Rigidbody rb;
    private TetherPlayerMove tetherPlayerMove;
    private bool wallContact;
    private bool otherContact;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        tetherPlayerMove = GetComponentInChildren<TetherPlayerMove>();
    }


    public void WallJump()
    {
        if (canWallJump && !tetherPlayerMove.isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = Vector3.zero;
            StopAllCoroutines();
            tetherPlayerMove.frontDir *= -1;
            rb.AddForce(transform.up * wallJumpVForce * 10 + Vector3.right * tetherPlayerMove.frontDir * wallJumpHForce * 10, ForceMode.Impulse);
            StartCoroutine(InputOveride(0.5f, Vector3.right * tetherPlayerMove.frontDir));
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
                //playerMove.moveOverride = false;
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
        if (!tetherPlayerMove.isGrounded)
        {
            StopAllCoroutines();
            StartCoroutine(InputOveride(0.2f, Vector3.zero));
            StartCoroutine(SlowFall(0.25f));
            canWallJump = true;
        }
        else
        {
            canWallJump = false;
        }
    }

    IEnumerator InputOveride(float delay, Vector3 moveDir)
    {
        //playerMove.moveOverride = true;
        //playerMove.moveDir = moveDir;
        yield return new WaitForSeconds(delay);
        //playerMove.moveOverride = false;
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

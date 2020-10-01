using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldPlayerJump : MonoBehaviour
{
    public float jumpForce;
    public float lowJumpMultiplier;
    public float fallMultiplier;
    public float gravityMultiplier;
    public float terminalVelocity;

    [HideInInspector] public bool disableJump;

    private Rigidbody rb;
    private Animator anim;
    private OldPlayerGroundCheck playerGroundCheck;
    private OldPlayerDoubleJump playerDoubleJump;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        playerGroundCheck = GetComponent<OldPlayerGroundCheck>();
        playerDoubleJump = GetComponent<OldPlayerDoubleJump>();
    }

    void FixedUpdate()
    {
        if (rb.useGravity)
        {
            rb.AddForce(transform.up * -gravityMultiplier * 10);
            Fall();
        }
    }

    public void Jump()
    {
        if (!disableJump && playerGroundCheck.isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            if (anim)
            {
                anim.SetTrigger("Jump");
                anim.SetBool("IsFalling", true);
            }
            rb.velocity = Vector3.zero;
            rb.AddForce(transform.up * jumpForce * 10, ForceMode.Impulse);
            playerGroundCheck.isGrounded = false;
            if (playerDoubleJump)
            {
                playerDoubleJump.canDoubleJump = true;
            }
        }
    }

    void Fall ()
    {
        if (!playerGroundCheck.isGrounded) {
            if (rb.velocity.y >= 0 && !Input.GetKey(KeyCode.Space))
            {
                rb.AddForce(transform.up * -lowJumpMultiplier * 10);
            }
            else if (rb.velocity.y < 0)
            {
                rb.AddForce(transform.up * -fallMultiplier * 10);
            }
            if (rb.velocity.y < -terminalVelocity)
            {
                rb.velocity = new Vector3(rb.velocity.x, -terminalVelocity);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    public float jumpForce;
    public float lowJumpMultiplier;
    public float fallMultiplier;
    public float gravityMultiplier;

    private Rigidbody2D rb;
    private PlayerGroundCheck playerGroundCheck;
    private PlayerDoubleJump playerDoubleJump;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerGroundCheck = GetComponentInChildren<PlayerGroundCheck>();
        playerDoubleJump = GetComponentInChildren<PlayerDoubleJump>();

    }

    void FixedUpdate()
    {
        rb.AddForce(transform.up * -gravityMultiplier * 10);
        Fall();
    }

    public void Jump()
    {
        if (playerGroundCheck.isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.AddForce(transform.up * jumpForce * 10, ForceMode2D.Impulse);
                playerGroundCheck.isGrounded = false;
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
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleGlide : MonoBehaviour
{
    public float glideFallSpeed;

    private bool canGlide;
    private Rigidbody2D rb;
    private PlayerGroundCheck playerGroundCheck;

    void Start()
    {
        canGlide = false;
        rb = GetComponent<Rigidbody2D>();
        playerGroundCheck = GetComponentInChildren<PlayerGroundCheck>();
    }

    public void Glide()
    {
        if (!canGlide && playerGroundCheck.isGrounded)
        {
            canGlide = true;
        }
        if (Input.GetKey(KeyCode.U))
        {
            if (rb.velocity.y < glideFallSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, glideFallSpeed);
            }
        }
        else if (Input.GetKeyUp(KeyCode.U))
        {
            canGlide = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDoubleJump : MonoBehaviour
{
    public float doubleJumpForce;

    [HideInInspector] public bool canDoubleJump;
    [HideInInspector] public bool disableDoubleJump;

    private Rigidbody2D rb;
    private SquareWallJump squareWallJump;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        squareWallJump = GetComponent<SquareWallJump>();
    }

    public void DoubleJump()
    {
        if (!disableDoubleJump && canDoubleJump && Input.GetKeyDown(KeyCode.Space) && !(squareWallJump && squareWallJump.canWallJump))
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(transform.up * doubleJumpForce * 10, ForceMode2D.Impulse);
            canDoubleJump = false;
        }
    }
}

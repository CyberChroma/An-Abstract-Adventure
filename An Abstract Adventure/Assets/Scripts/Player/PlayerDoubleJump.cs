using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDoubleJump : MonoBehaviour
{
    public float doubleJumpForce;

    [HideInInspector] public bool canDoubleJump;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void DoubleJump()
    {
        if (canDoubleJump && Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(transform.up * doubleJumpForce * 10, ForceMode2D.Impulse);
            canDoubleJump = false;
        }
    }
}

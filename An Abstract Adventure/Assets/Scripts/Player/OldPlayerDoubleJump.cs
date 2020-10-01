using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldPlayerDoubleJump : MonoBehaviour
{
    public float doubleJumpForce;

    [HideInInspector] public bool canDoubleJump;
    [HideInInspector] public bool disableDoubleJump;

    private Rigidbody rb;
    private Animator anim;
    private OldSquareWallJump squareWallJump;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        squareWallJump = GetComponent<OldSquareWallJump>();
    }

    public void DoubleJump()
    {
        if (!disableDoubleJump && canDoubleJump && Input.GetKeyDown(KeyCode.Space) && !(squareWallJump && squareWallJump.canWallJump))
        {
            if (anim)
            {
                anim.SetTrigger("DoubleJump");
            }
            rb.velocity = Vector3.zero;
            rb.AddForce(transform.up * doubleJumpForce * 10, ForceMode.Impulse);
            canDoubleJump = false;
        }
    }
}

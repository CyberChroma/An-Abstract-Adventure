using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareCrouch : MonoBehaviour
{
    private bool setToStand;
    private bool crouching;
    private bool canStick;
    private bool sticking;
    private Transform mainSprite;
    private CapsuleCollider2D cc;
    private Rigidbody2D rb;
    private Animator anim;
    private PlayerLineUp playerLineUp;
    private PlayerAttack playerAttack;
    private PlayerGroundCheck playerGroundCheck;
    private SquareWallJump squareWallJump;
    private SquareShurikenThrow squareShurikenThrow;

    // Start is called before the first frame update
    void Start()
    {
        setToStand = false;
        mainSprite = transform.Find("Sprites");
        cc = GetComponent<CapsuleCollider2D>();
        playerLineUp = GetComponent<PlayerLineUp>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        playerAttack = GetComponent<PlayerAttack>();
        squareWallJump = GetComponent<SquareWallJump>();
        squareShurikenThrow = GetComponent<SquareShurikenThrow>();
        playerGroundCheck = GetComponentInChildren<PlayerGroundCheck>();
    }

    public void Crouch()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (playerGroundCheck.isGrounded && !crouching)
            {
                anim.SetBool("IsCrouching", true);
                cc.size = new Vector2(1.5f, 0.45f);
                cc.offset = new Vector2(0, -0.25f);
                playerLineUp.disableAiming = true;
                crouching = true;
                setToStand = false;
            }
            else if (!canStick)
            {
                canStick = true;
            }
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            if (crouching)
            {
                if (Physics2D.Raycast(transform.position, Vector3.up, 0.5f, 1 << 8))
                {
                    setToStand = true;
                }
                else
                {
                    Stand();
                }
            }
            if (sticking)
            {
                canStick = false;
                Unstick();
            }
        }
        if (!playerGroundCheck.isGrounded || (setToStand && !Physics2D.Raycast(transform.position, transform.up, 0.5f, 1 << 8)))
        {
            Stand();
        }
    }

    void Stand ()
    {
        anim.SetBool("IsCrouching", false);
        cc.size = Vector2.one * 0.95f;
        cc.offset = new Vector2(0, 0);
        playerLineUp.disableAiming = false;
        crouching = false;
        setToStand = false;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (canStick && !sticking && collision.gameObject.layer == 8 && !collision.collider.CompareTag("Slippery"))
        {
            Stick();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (sticking && collision.gameObject.layer == 8 && !collision.collider.CompareTag("Slippery"))
        {
            Unstick();
        }
    }

    void Stick ()
    {
        rb.gravityScale = 0;
        rb.velocity = new Vector2(rb.velocity.x, 0);
        playerAttack.airBoostOverride = true;
        squareWallJump.fallOverride = true;
        squareShurikenThrow.airBoostOverride = true;
        sticking = true;
        anim.SetBool("IsWallSticking", true);
    }

    void Unstick()
    {
        rb.gravityScale = 1;
        playerAttack.airBoostOverride = false;
        squareWallJump.fallOverride = false;
        squareShurikenThrow.airBoostOverride = false;
        sticking = false;
        anim.SetBool("IsWallSticking", false);
    }
}

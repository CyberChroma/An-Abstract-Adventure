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
    private CapsuleCollider cc;
    private Rigidbody rb;
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
        cc = GetComponent<CapsuleCollider>();
        playerLineUp = GetComponent<PlayerLineUp>();
        rb = GetComponent<Rigidbody>();
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
                cc.center = new Vector2(0, -0.25f);
                cc.radius = 0.25f;
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
                if (Physics.Raycast(transform.position, transform.up, 0.5f, 1 << 8))
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
        if (!playerGroundCheck.isGrounded || (setToStand && !Physics.Raycast(transform.position, transform.up, 0.5f, 1 << 8)))
        {
            Stand();
        }
    }

    void Stand ()
    {
        anim.SetBool("IsCrouching", false);
        cc.center = Vector3.zero;
        cc.radius = 0.5f;
        playerLineUp.disableAiming = false;
        crouching = false;
        setToStand = false;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (canStick && !sticking && collision.gameObject.layer == 8 && !collision.collider.CompareTag("Slippery"))
        {
            Stick();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (sticking && collision.gameObject.layer == 8 && !collision.collider.CompareTag("Slippery"))
        {
            Unstick();
        }
    }

    void Stick ()
    {
        rb.useGravity = false;
        rb.velocity = new Vector3(rb.velocity.x, 0, 0);
        playerAttack.airBoostOverride = true;
        squareWallJump.fallOverride = true;
        squareShurikenThrow.airBoostOverride = true;
        sticking = true;
        anim.SetBool("IsWallSticking", true);
    }

    void Unstick()
    {
        rb.useGravity = true;
        playerAttack.airBoostOverride = false;
        squareWallJump.fallOverride = false;
        squareShurikenThrow.airBoostOverride = false;
        sticking = false;
        anim.SetBool("IsWallSticking", false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareStick : MonoBehaviour
{
    private bool canStick;
    private bool sticking;
    private Rigidbody2D rb;
    private PlayerAttack playerAttack;
    private SquareWallJump squareWallJump;
    private SquareShurikenThrow squareShurikenThrow;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAttack = GetComponent<PlayerAttack>();
        squareWallJump = GetComponent<SquareWallJump>();
        squareShurikenThrow = GetComponent<SquareShurikenThrow>();
    }

    public void Stick ()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            canStick = true;
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            canStick = false;
            Unstick();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (canStick && !sticking && collision.gameObject.layer == 8 && !collision.collider.CompareTag("Slippery"))
        {
            rb.gravityScale = 0;
            rb.velocity = new Vector2(rb.velocity.x, 0);
            playerAttack.airBoostOverride = true;
            squareWallJump.fallOverride = true;
            squareShurikenThrow.airBoostOverride = true;
            sticking = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (sticking && collision.gameObject.layer == 8 && !collision.collider.CompareTag("Slippery"))
        {
            Unstick();
        }
    }

    void Unstick ()
    {
        rb.gravityScale = 1;
        playerAttack.airBoostOverride = false;
        squareWallJump.fallOverride = false;
        squareShurikenThrow.airBoostOverride = false;
        sticking = false;
    }
}

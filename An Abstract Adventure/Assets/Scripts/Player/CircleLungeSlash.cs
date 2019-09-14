using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleLungeSlash : MonoBehaviour
{
    public float lungeTime;
    public float lungePower;

    private bool lunging;
    private Vector2 lastVelocity;
    private Rigidbody2D rb;
    private PlayerMove playerMove;
    private PlayerDoubleJump playerDoubleJump;
    private PlayerLineUp playerLineUp;
    private PlayerGroundCheck playerGroundCheck;

    // Start is called before the first frame update
    void Awake()
    {
        lunging = false;
        rb = GetComponent<Rigidbody2D>();
        playerMove = GetComponent<PlayerMove>();
        playerDoubleJump = GetComponent<PlayerDoubleJump>();
        playerLineUp = GetComponent<PlayerLineUp>();
        playerGroundCheck = GetComponentInChildren<PlayerGroundCheck>();
    }

    public void LungeSlash()
    {
        if (!playerLineUp.canAim && playerGroundCheck.isGrounded)
        {
            playerLineUp.canAim = true;
        }
        if (playerLineUp.released)
        {
            StartCoroutine(WaitToLunge());
        }
        lastVelocity = rb.velocity;
    }

    IEnumerator WaitToLunge()
    {
        playerLineUp.released = false;
        playerMove.moveOverride = true;
        playerMove.noDrag = true;
        if (playerGroundCheck.isGrounded)
        {
            playerGroundCheck.isGrounded = false;
            playerDoubleJump.canDoubleJump = true;
        }
        playerLineUp.canAim = false;
        rb.velocity = Vector2.zero;
        rb.AddForce(playerLineUp.arrow.transform.up.normalized * lungePower, ForceMode2D.Impulse);
        playerMove.moveDir = Vector2.zero;
        rb.gravityScale = 0;
        lunging = true;
        yield return new WaitForSeconds(lungeTime);
        lunging = false;
        playerLineUp.canAim = false;
        rb.gravityScale = 1;
        playerMove.moveDir = new Vector2(rb.velocity.normalized.x * 3, 0);
        playerMove.noDrag = false;
        playerMove.moveOverride = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (lunging && collision.gameObject.layer == 8)
        {
            rb.velocity = Vector2.Reflect(lastVelocity, collision.contacts[0].normal);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleLungeSlash : MonoBehaviour
{
    public float lungeTime;
    public float lungePower;
    public float lungeDelay;

    private bool canLunge;
    private bool lunging;
    private Vector3 lastVelocity;
    private Rigidbody rb;
    private PlayerMove playerMove;
    private PlayerDoubleJump playerDoubleJump;
    private PlayerLineUp playerLineUp;
    private PlayerGroundCheck playerGroundCheck;
    private PlayerSwim playerSwim;

    // Start is called before the first frame update
    void Awake()
    {
        lunging = false;
        rb = GetComponent<Rigidbody>();
        playerMove = GetComponent<PlayerMove>();
        playerDoubleJump = GetComponent<PlayerDoubleJump>();
        playerLineUp = GetComponent<PlayerLineUp>();
        playerGroundCheck = GetComponentInChildren<PlayerGroundCheck>();
        playerSwim = GetComponent<PlayerSwim>();
    }

    public void LungeSlash()
    {
        if (!playerLineUp.canAim && canLunge && (playerGroundCheck.isGrounded || playerSwim.swimming))
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
        canLunge = false;
        playerLineUp.released = false;
        playerMove.moveOverride = true;
        playerMove.noDrag = true;
        if (playerGroundCheck.isGrounded)
        {
            playerGroundCheck.isGrounded = false;
            playerDoubleJump.canDoubleJump = true;
        }
        playerLineUp.canAim = false;
        rb.velocity = Vector3.zero;
        rb.AddForce(playerLineUp.arrow.transform.up.normalized * lungePower, ForceMode.Impulse);
        playerMove.moveDir = Vector3.zero;
        rb.useGravity = false;
        lunging = true;
        yield return new WaitForSeconds(lungeTime);
        lunging = false;
        playerLineUp.canAim = false;
        rb.useGravity = true;
        if (!playerSwim.swimming)
        {
            playerMove.moveDir = new Vector3(rb.velocity.normalized.x * 3, 0, 0);
        }
        playerMove.noDrag = false;
        playerMove.moveOverride = false;
        yield return new WaitForSeconds(lungeDelay);
        canLunge = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (lunging && collision.gameObject.layer == 8)
        {
            rb.velocity = Vector3.Reflect(lastVelocity, collision.contacts[0].normal);
        }
    }
}

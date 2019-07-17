using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
    public float fallDelay;

    [HideInInspector] public bool isGrounded;

    private PlayerDoubleJump playerDoubleJump;

    void Start()
    {
        playerDoubleJump = GetComponentInParent<PlayerDoubleJump>();
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (!isGrounded && collision.gameObject.layer == 8)
        {
            isGrounded = true;
            playerDoubleJump.canDoubleJump = false;
        } 
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (!isGrounded && collision.gameObject.layer == 8)
        {
            StopAllCoroutines();
            StartCoroutine(WaitToFall());
        }
    }

    IEnumerator WaitToFall ()
    {
        yield return new WaitForSeconds(fallDelay);
        if (isGrounded)
        {
            isGrounded = false;
            playerDoubleJump.canDoubleJump = true;
        }
    }
}

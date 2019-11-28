using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
    public float fallDelay;

    [HideInInspector] public bool isGrounded;

    private Animator anim;
    private PlayerDoubleJump playerDoubleJump;

    void Awake()
    {
        anim = GetComponentInParent<PlayerMove>().GetComponentInChildren<Animator>();
        playerDoubleJump = GetComponentInParent<PlayerDoubleJump>();
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (!isGrounded && collision.gameObject.layer == 8)
        {
            if (anim)
            {
                anim.SetBool("IsFalling", false);
                anim.SetTrigger("Land");
            }
            isGrounded = true;
            playerDoubleJump.canDoubleJump = false;
        } 
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (isGrounded && collision.gameObject.layer == 8)
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
            if (anim)
            {
                anim.SetBool("IsFalling", true);
            }
            isGrounded = false;
            playerDoubleJump.canDoubleJump = true;
        }
    }
}

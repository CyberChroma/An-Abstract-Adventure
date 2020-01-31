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

    void OnCollisionEnter(Collision collision)
    {
        if (!isGrounded && collision.gameObject.layer == 8 && Mathf.Abs(collision.contacts[0].normal.x) <= 0.5f)
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

    void OnCollisionExit(Collision collision)
    {
        if (isGrounded && collision.gameObject.layer == 8 && !Physics.Raycast(transform.position, -transform.up, 0.5f, 1 << 8))
        {
            StopAllCoroutines();
            StartCoroutine(WaitToFall());
        }
    }

    IEnumerator WaitToFall ()
    {
        yield return new WaitForSeconds(fallDelay);
        if (isGrounded && !Physics.Raycast(transform.position, -Vector3.up, 0.6f, 1 << 8))
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

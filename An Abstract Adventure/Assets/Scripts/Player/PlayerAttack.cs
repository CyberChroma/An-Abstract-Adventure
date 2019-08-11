using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float attackTime;
    public float attackWait;

    private bool canAttack = true;
    private Transform attackCollider;
    private Rigidbody2D rb;
    private PlayerGroundCheck playerGroundCheck;

    private void Start()
    {
        attackCollider = transform.Find("Attack Hit Box");
        attackCollider.gameObject.SetActive(false);
        rb = GetComponent<Rigidbody2D>();
        playerGroundCheck = GetComponentInChildren<PlayerGroundCheck>();
    }

    public void Attack ()
    {
        if (canAttack && Input.GetKeyDown(KeyCode.U))
        {
            StartCoroutine(WaitToAttack());
        }
    }

    IEnumerator WaitToAttack ()
    {
        canAttack = false;
        attackCollider.gameObject.SetActive(true);
        if (!playerGroundCheck.isGrounded && rb.velocity.y < 1)
        {
            rb.velocity = Vector3.zero;
            rb.AddForce(transform.up * 20, ForceMode2D.Impulse);
        }
        yield return new WaitForSeconds(attackTime);
        attackCollider.gameObject.SetActive(false);
        yield return new WaitForSeconds(attackWait);
        canAttack = true;
    }
}

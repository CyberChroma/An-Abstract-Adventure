using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float attackTime;
    public float attackWait;
    public float airBoost;

    [HideInInspector] public bool disableAttack;
    [HideInInspector] public bool airBoostOverride;

    private bool canAttack = true;
    private Transform attackCollider;
    private Rigidbody2D rb;
    private Animator anim;
    private PlayerGroundCheck playerGroundCheck;
    private PlayerSwim playerSwim;

    private void Start()
    {
        attackCollider = transform.Find("Attack Hit Box");
        attackCollider.gameObject.SetActive(false);
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        playerGroundCheck = GetComponentInChildren<PlayerGroundCheck>();
        playerSwim = GetComponent<PlayerSwim>();
    }

    public void Attack ()
    {
        if (canAttack && !disableAttack && Time.timeScale != 0 && Input.GetKeyDown(KeyCode.U))
        {
            StartCoroutine(WaitToAttack());
        }
    }

    IEnumerator WaitToAttack ()
    {
        canAttack = false;
        attackCollider.gameObject.SetActive(true);
        if (!airBoostOverride && !playerGroundCheck.isGrounded && !playerSwim.swimming && rb.velocity.y < 1)
        {
            rb.velocity = Vector3.zero;
            rb.AddForce(transform.up * airBoost, ForceMode2D.Impulse);
        }
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(attackTime);
        attackCollider.gameObject.SetActive(false);
        yield return new WaitForSeconds(attackWait);
        canAttack = true;
    }
}

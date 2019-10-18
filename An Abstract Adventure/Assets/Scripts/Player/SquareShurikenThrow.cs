using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareShurikenThrow : MonoBehaviour
{
    public float throwDelay;
    public float airBoost;
    public GameObject shuriken;

    [HideInInspector] public bool airBoostOverride;

    private PlayerLineUp playerLineUp;
    private PlayerGroundCheck playerGroundCheck;
    private Rigidbody2D rb;
    private Transform shurikenParent;
    private PlayerSwim playerSwim;

    void Awake()
    {
        playerLineUp = GetComponent<PlayerLineUp>();
        playerGroundCheck = GetComponentInChildren<PlayerGroundCheck>();
        rb = GetComponent<Rigidbody2D>();
        shurikenParent = GameObject.Find("Shurikens").transform;
        playerSwim = GetComponent<PlayerSwim>();
    }

    public void ShurikenThrow()
    {
        if (playerLineUp.released)
        {
            StartCoroutine(WaitToThrow());
        }
    }

    IEnumerator WaitToThrow()
    {
        playerLineUp.released = false;
        playerLineUp.canAim = false;
        Instantiate(shuriken, transform.position, playerLineUp.arrow.transform.rotation, shurikenParent);
        if (!playerGroundCheck.isGrounded && !airBoostOverride && !playerSwim.swimming)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(transform.up * airBoost, ForceMode2D.Impulse);
        }
        yield return new WaitForSeconds(throwDelay);
        playerLineUp.canAim = true;
    }
}

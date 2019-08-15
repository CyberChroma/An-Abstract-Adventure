﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareShurikenThrow : MonoBehaviour
{

    public float throwDelay;
    public float airBoost;
    public GameObject shuriken;

    private PlayerLineUp playerLineUp;
    private PlayerGroundCheck playerGroundCheck;
    private Rigidbody2D rb;
    private Transform shurikenParent;

    void Awake()
    {
        playerLineUp = GetComponent<PlayerLineUp>();
        playerGroundCheck = GetComponentInChildren<PlayerGroundCheck>();
        rb = GetComponent<Rigidbody2D>();
        shurikenParent = GameObject.Find("Shurikens").transform;
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
        if (!playerGroundCheck.isGrounded)
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(transform.up * airBoost, ForceMode2D.Impulse);
        }
        yield return new WaitForSeconds(throwDelay);
        playerLineUp.canAim = true;
    }
}

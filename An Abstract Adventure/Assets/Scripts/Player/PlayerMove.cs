﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed;
    public float moveSmoothness;
    public float stopSmoothness;

    [HideInInspector] public bool active;
    [HideInInspector] public bool frontRight;
    [HideInInspector] public Vector2 moveDir = Vector2.zero;
    [HideInInspector] public bool moveOverride;

    private Rigidbody2D rb;
    private PlayerGroundCheck playerGroundCheck;
    private GameObject attackCollider;

    void Awake()
    {
        frontRight = true;
        rb = GetComponent<Rigidbody2D>();
        playerGroundCheck = GetComponentInChildren<PlayerGroundCheck>();
        attackCollider = transform.Find("Attack Hit Box").gameObject;
    }

    private void FixedUpdate()
    {
        if (!active && moveDir != Vector2.zero)
        {
            moveDir = Vector2.Lerp(moveDir, Vector2.zero, stopSmoothness * Time.deltaTime);
            rb.AddForce (moveDir * speed  * 10 * Time.deltaTime, ForceMode2D.Impulse);
        }
    }

    public void Move ()
    {
        if (active)
        {
            if (!moveOverride)
            {
                if (Input.GetKey(KeyCode.D))
                {
                    if (!attackCollider.activeSelf && !frontRight)
                    {
                        transform.localScale = new Vector2(1, 1);
                        frontRight = true;
                        if (playerGroundCheck.isGrounded)
                        {
                            moveDir = Vector2.Lerp(moveDir, transform.right, moveSmoothness * Time.deltaTime);
                        }
                        else
                        {
                            moveDir = Vector2.zero;
                        }
                    }
                    moveDir = Vector2.Lerp(moveDir, transform.right, moveSmoothness * Time.deltaTime);
                }
                else if (Input.GetKey(KeyCode.A))
                {
                    if (!attackCollider.activeSelf && frontRight)
                    {
                        transform.localScale = new Vector2(-1, 1);
                        frontRight = false;
                        if (playerGroundCheck.isGrounded)
                        {
                            moveDir = Vector2.Lerp(moveDir, -transform.right, moveSmoothness * Time.deltaTime);
                        }
                        else
                        {
                            moveDir = Vector2.zero;
                        }
                    }
                    moveDir = Vector2.Lerp(moveDir, -transform.right, moveSmoothness * Time.deltaTime);
                }
                else if (moveDir != Vector2.zero)
                {
                    moveDir = Vector2.Lerp(moveDir, Vector2.zero, stopSmoothness * Time.deltaTime);
                }
            }
            rb.AddForce(moveDir * speed * 10 * Time.deltaTime, ForceMode2D.Impulse);
        }
    }
}

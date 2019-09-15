using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleGlide : MonoBehaviour
{
    public float glideFallSpeed;

    private Rigidbody2D rb;
    private PlayerAttack playerAttack;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAttack = GetComponent<PlayerAttack>();
    }

    public void Glide()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            playerAttack.disableAttack = true;
            if (rb.velocity.y < glideFallSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, glideFallSpeed);
            }
        }
        else
        {
            playerAttack.disableAttack = false;
        }
    }
}

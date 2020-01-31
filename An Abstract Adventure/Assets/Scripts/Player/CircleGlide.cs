using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleGlide : MonoBehaviour
{
    public float glideFallSpeed;

    private Rigidbody rb;
    private PlayerAttack playerAttack;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerAttack = GetComponent<PlayerAttack>();
    }

    public void Glide()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            playerAttack.disableAttack = true;
            if (rb.velocity.y < glideFallSpeed)
            {
                rb.velocity = new Vector3(rb.velocity.x, glideFallSpeed, 0);
            }
        }
        else
        {
            playerAttack.disableAttack = false;
        }
    }
}

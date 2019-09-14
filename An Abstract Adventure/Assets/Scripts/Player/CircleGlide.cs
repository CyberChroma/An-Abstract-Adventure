using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleGlide : MonoBehaviour
{
    public float glideFallSpeed;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Glide()
    {
        if (Input.GetKey(KeyCode.U) && rb.velocity.y < glideFallSpeed)
        {
            rb.velocity = new Vector2(rb.velocity.x, glideFallSpeed);
        }
    }
}

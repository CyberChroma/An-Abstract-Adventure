using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityMultiplier : MonoBehaviour
{
    public float multiplier;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.AddForce(Vector3.down * multiplier * 10);
    }
}

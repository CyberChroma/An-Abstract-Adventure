using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetherFollow : MonoBehaviour
{
    public float maxDistance;
    public float catchUpSpeed;
    public Transform tetherTarget;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 dir = tetherTarget.position - transform.position;
        if (dir.magnitude > maxDistance)
        {
            rb.AddForce(dir * catchUpSpeed);
        }
    }
}

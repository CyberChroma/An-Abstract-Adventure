using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldPlayerWind : MonoBehaviour
{
    private Rigidbody rb;
    private Push push;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (push)
        {
            rb.AddForce(push.dir * push.speed * 10 * Time.deltaTime, ForceMode.Impulse);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Push"))
        {
            push = collision.collider.GetComponent<Push>();
        }  
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Push"))
        {
            push = null;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Push"))
        {
            push = collision.GetComponent<Push>();
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Push"))
        {
            push = null;
        }
    }
}

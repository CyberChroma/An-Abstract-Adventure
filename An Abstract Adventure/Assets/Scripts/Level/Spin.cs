using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    public float speed;
    public bool right;

    // Update is called once per frame
    void Update()
    {
        if (right)
        {
            transform.Rotate(Vector3.forward * speed * 10 * Time.deltaTime);
        }
        else
        {
            transform.Rotate(Vector3.forward * -speed * 10 * Time.deltaTime);
        }
    }
}

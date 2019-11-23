using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform target;
    public float smoothing;
    public float offsetHeight;
    public bool startOnPos;

    private Vector3 movePos;
    private Vector3 camVelocity;

    void OnEnable()
    {
        if (startOnPos == true) {
            transform.position = target.position;
        }
    }

    void FixedUpdate()
    {
        if (target != null)
        {
            movePos = Vector3.SmoothDamp(transform.position, new Vector3(target.position.x, target.position.y + offsetHeight, 0), ref camVelocity, smoothing, Mathf.Infinity, Time.deltaTime);
        }
        transform.position = movePos;
    }
}

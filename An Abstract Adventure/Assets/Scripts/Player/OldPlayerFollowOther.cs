using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldPlayerFollowOther : MonoBehaviour
{
    public Transform toFollow;

    [HideInInspector] public bool following;

    private Rigidbody rb;
    private float startZ;
    
    // Start is called before the first frame update
    void Start()
    {
        startZ = transform.position.z;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (following)
        {
            transform.position = new Vector3(toFollow.position.x, toFollow.position.y, startZ);
        }
    }
}

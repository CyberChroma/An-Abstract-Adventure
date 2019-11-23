using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowAndShrink : MonoBehaviour
{
    public float growSpeed;

    [HideInInspector] public bool growing;

    private void Start()
    {
        transform.localScale = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (growing)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.one, growSpeed * Time.deltaTime);
        }
        else
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.zero, growSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.localScale, Vector3.zero) <= 0.01f)
            {
                transform.localScale = Vector3.zero;
            }
        }
    }
}

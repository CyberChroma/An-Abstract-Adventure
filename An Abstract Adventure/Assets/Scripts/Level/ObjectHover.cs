using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHover : MonoBehaviour
{
    public float amount;
    public float speed;
    public Transform objectToMove;
    public bool timeUnscaled;

    private Vector3 startingPos;
    private float timeOffset;

    // Start is called before the first frame update
    void Start()
    {
        startingPos = objectToMove.localPosition;
        timeOffset = Random.Range(-5f, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (amount != 0)
        {
            if (timeUnscaled)
            {
                objectToMove.localPosition = startingPos + Vector3.up * Mathf.Cos(Time.unscaledTime * speed + timeOffset) / (1 / amount);
            }
            else
            {
                objectToMove.localPosition = startingPos + Vector3.up * Mathf.Cos(Time.time * speed + timeOffset) / (1 / amount);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    public float speed;
    public float delayBetween;
    public float randomOffset;
    public bool repeat;
    public Transform[] targets;

    [HideInInspector] public Vector2 velocity;

    private int currTarget;
    private bool moving;
    private Vector2 lastPos;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        currTarget = 0;
        moving = true;
        lastPos = rb.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (moving)
        {
            if (Vector3.Distance(rb.position, targets[currTarget].position) < 0.1f)
            {
                rb.MovePosition(targets[currTarget].position);
                StartCoroutine(WaitToMove());
            }
            else
            {
                rb.MovePosition(Vector2.MoveTowards(rb.position, targets[currTarget].position, speed * Time.deltaTime));
            }
        }
        if (Time.deltaTime != 0)
        {
            velocity = (rb.position - lastPos) / Time.deltaTime;
        }
        else
        {
            velocity = Vector2.zero;
        }
        lastPos = rb.position;
    }

    IEnumerator WaitToMove()
    {
        moving = false;
        yield return new WaitForSeconds(delayBetween + Random.Range(-randomOffset, randomOffset));
        currTarget++;
        if (currTarget > targets.Length - 1)
        {
            if (repeat)
            {
                currTarget = 0;
            }
            else
            {
                enabled = false;
            }
        }
        moving = true;
    }
}

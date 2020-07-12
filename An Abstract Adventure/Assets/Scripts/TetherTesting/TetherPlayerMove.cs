using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetherPlayerMove : MonoBehaviour
{
    [Header("Move")]
    public float speed;
    public float moveSmoothness;
    public float rotSmoothing;
    public float horizontalDrag;
    public bool active;

    [Header("Jump")]
    public float jumpForce;
    public float lowJumpMultiplier;
    public float fallMultiplier;
    public float gravityMultiplier;
    public float terminalVelocity;

    [Header("Grounded")]
    public float fallDelay;

    [HideInInspector] public Rigidbody rb;
    private Vector3 moveDir = Vector3.zero;
    private int frontDir;
    private bool isGrounded;

    void Awake()
    {
        frontDir = 1;
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (active)
        {
            Move();
        }
        else if (moveDir != Vector3.zero)
        {
            moveDir = Vector3.Lerp(moveDir, Vector3.zero, moveSmoothness * Time.deltaTime);
            rb.AddForce(moveDir * speed * 10 * Time.deltaTime, ForceMode.Impulse);
        }
        if (horizontalDrag != 0)
        {
            rb.velocity = new Vector3(rb.velocity.x * (1 / horizontalDrag), rb.velocity.y, 0);
        }
        if (rb.useGravity)
        {
            rb.AddForce(transform.up * -gravityMultiplier * 10);
            Fall();
        }
    }

    private void Update()
    {
        if (active)
        {
            Jump();
        }
    }

    void Move()
    {
        if (Input.GetKey(KeyCode.D))
        {
            if (frontDir == -1)
            {
                frontDir *= -1;
                moveDir = Vector3.Lerp(moveDir, Vector3.right, moveSmoothness * Time.deltaTime);
            }
            moveDir = Vector3.Lerp(moveDir, Vector3.right, moveSmoothness * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            if (frontDir == 1)
            {
                frontDir *= -1;
                moveDir = Vector3.Lerp(moveDir, -Vector3.right, moveSmoothness * Time.deltaTime);
            }
            moveDir = Vector3.Lerp(moveDir, -Vector3.right, moveSmoothness * Time.deltaTime);
        }
        else if (moveDir != Vector3.zero)
        {
            moveDir = Vector3.Lerp(moveDir, Vector3.zero, moveSmoothness * Time.deltaTime);
        }
        rb.AddForce(moveDir * speed * 10 * Time.deltaTime, ForceMode.Impulse);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(new Vector3(0, 270 + frontDir * 90, 0)), rotSmoothing * Time.deltaTime);
    }

    void Jump()
    {
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = Vector3.zero;
            rb.AddForce(transform.up * jumpForce * 10, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    void Fall()
    {
        if (rb.velocity.y >= 0 && !Input.GetKey(KeyCode.Space))
        {
            rb.AddForce(transform.up * -lowJumpMultiplier * 10);
        }
        else if (rb.velocity.y < 0)
        {
            rb.AddForce(transform.up * -fallMultiplier * 10);
        }
        if (rb.velocity.y < -terminalVelocity)
        {
            rb.velocity = new Vector3(rb.velocity.x, -terminalVelocity);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!isGrounded && collision.gameObject.layer == 8 && Mathf.Abs(collision.contacts[0].normal.x) < 0.9f)
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (isGrounded && collision.gameObject.layer == 8 && !Physics.Raycast(transform.position, -transform.up, 0.5f, 1 << 8))
        {
            StopAllCoroutines();
            StartCoroutine(WaitToFall());
        }
    }

    IEnumerator WaitToFall()
    {
        yield return new WaitForSeconds(fallDelay);
        if (isGrounded && !Physics.Raycast(transform.position, -Vector3.up, 0.6f, 1 << 8))
        {
            isGrounded = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetherPlayerMove : MonoBehaviour
{
    public enum Mode
    {
        Active,
        Following
    }
    public Mode mode;

    [Header("Move")]
    public float speed;
    public float moveSmoothness;
    public float rotSmoothing;

    [Header("Jump")]
    public float jumpForce;
    public float lowJumpMultiplier;
    public float fallMultiplier;
    public float gravityMultiplier;
    public float terminalVelocity;

    [Header("Grounded")]
    public float jumpInputStoreTime;
    public float fallJumpDelay;

    [HideInInspector] public Rigidbody rb;
    private float moveDir;
    private int frontDir;
    private bool isGrounded;
    private bool jumpInput;
    private bool canJump;
    private Vector3 combinedVelocity;

    void Awake()
    {
        frontDir = 1;
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        combinedVelocity = Vector3.zero;
        if (mode == Mode.Active)
        {
            combinedVelocity.x = Move();
        }
        else
        {
            combinedVelocity.x = 0;
            //Follow Active Player
        }
        Turn();
        combinedVelocity.y = Jump();
        if (!isGrounded && combinedVelocity.y == 0 && rb.useGravity)
        {
            combinedVelocity.y = Fall();
        }
        combinedVelocity.z = 0;
        rb.velocity = combinedVelocity;
    }

    private void Update()
    {
        if (mode == Mode.Active)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                jumpInput = true;
                StopCoroutine(StoreJumpInput());
                StartCoroutine(StoreJumpInput());
            }
        }
    }

    float Move()
    {
        if (Input.GetKey(KeyCode.D))
        {
            if (frontDir == -1)
            {
                frontDir = 1;
                moveDir = Mathf.Lerp(moveDir, 1, moveSmoothness * Time.deltaTime);
            }
            moveDir = Mathf.Lerp(moveDir, 1, moveSmoothness * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            if (frontDir == 1)
            {
                frontDir = -1;
                moveDir = Mathf.Lerp(moveDir, -1, moveSmoothness * Time.deltaTime);
            }
            moveDir = Mathf.Lerp(moveDir, -1, moveSmoothness * Time.deltaTime);
        }
        else if (moveDir != 0)
        {
            moveDir = Mathf.Lerp(moveDir, 0, moveSmoothness * Time.deltaTime);
        }
        float moveVel = moveDir * speed * 10 * Time.deltaTime;
        return moveVel;
    }

    void Turn ()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(new Vector3(0, 270 + frontDir * 90, 0)), rotSmoothing * Time.deltaTime);
    }

    float Fall()
    {
        float fallVel = rb.velocity.y;
        fallVel -= gravityMultiplier * 10 * Time.deltaTime;
        if (rb.velocity.y >= 0 && !Input.GetKey(KeyCode.Space))
        {
            fallVel -= lowJumpMultiplier * 10 * Time.deltaTime;
        }
        else if (rb.velocity.y < 0)
        {
            fallVel -= fallMultiplier * 10 * Time.deltaTime;
        }
        if (rb.velocity.y < -terminalVelocity)
        {
            fallVel = -terminalVelocity;
        }
        return fallVel;
    }

    float Jump()
    {
        float jumpVel = 0;
        if (canJump && jumpInput)
        {
            jumpVel = jumpForce * 10;
            canJump = false;
            jumpInput = false;
            isGrounded = false;
        }
        return jumpVel;
    }

    IEnumerator StoreJumpInput ()
    {
        yield return new WaitForSeconds(jumpInputStoreTime);
        jumpInput = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!isGrounded && collision.gameObject.layer == 8 && Mathf.Abs(collision.contacts[0].normal.x) < 0.9f)
        {
            isGrounded = true;
            canJump = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (isGrounded && collision.gameObject.layer == 8 && !Physics.Raycast(transform.position, -transform.up, 0.5f, 1 << 8))
        {
            isGrounded = false;
            StopCoroutine(WaitToCancelJump());
            StartCoroutine(WaitToCancelJump());
        }
    }

    IEnumerator WaitToCancelJump()
    {
        yield return new WaitForSeconds(fallJumpDelay);
        if (!Physics.Raycast(transform.position, -Vector3.up, 0.6f, 1 << 8))
        {
            canJump = false;
        }
    }
}

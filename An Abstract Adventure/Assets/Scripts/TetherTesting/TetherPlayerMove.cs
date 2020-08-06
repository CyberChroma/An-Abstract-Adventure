﻿using System.Collections;
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

    [Header("Ground Detection")]
    public float jumpInputStoreTime;
    public float fallJumpDelay;

    [Header("Tether Follow Pull")]
    public TetherPlayerMove otherPlayerMove;
    public float followDelay;
    public float followAcceleration;
    public float followDrag;
    public float followSmoothing;
    public float maxFollowSpeed;

    [Header("Tether Active Pull")]
    public float activePullStartDis;
    public float activePullAcceleration;
    public float activePullSmoothing;

    // Inputs
    private bool inputML;
    private bool inputMR;
    private bool inputJD;
    private bool inputJU;

    private float moveDir;
    private int frontDir;
    private bool isGrounded;
    private bool jumpInput;
    private bool canJump;
    private bool jumpHeld;

    private Vector3 pastPos;
    private Vector3 combinedVelocity;
    private Rigidbody rb;
    private Vector3 currAcceleration;
    private Vector3 currActivePullAcc;

    void Awake()
    {
        frontDir = 1;
        rb = GetComponent<Rigidbody>();
        pastPos = transform.position;
    }

    void FixedUpdate()
    {
        combinedVelocity = Vector3.zero;
        Vector3 dir = otherPlayerMove.rb.position - rb.position;
        dir.z = 0;
        if (mode == Mode.Active)
        {
            StartCoroutine(otherPlayerMove.FollowPastPosition(rb.position));
            pastPos = rb.position;
            GetKeyInput();
            combinedVelocity.x = Move();
            combinedVelocity.y = Jump();
            if (!isGrounded && rb.useGravity && combinedVelocity.y == 0)
            {
                combinedVelocity.y = Fall();
            }

            if (dir.magnitude - activePullStartDis > 0)
            {
                dir = dir.normalized * Mathf.Pow(dir.magnitude - activePullStartDis, 2);
            }
            else
            {
                dir = Vector3.zero;
            }
            currActivePullAcc = Vector3.Lerp(currActivePullAcc, dir * activePullAcceleration, activePullSmoothing);
            currActivePullAcc.z = 0;
            combinedVelocity += currActivePullAcc * Time.deltaTime;
        }
        else if (Time.deltaTime != 0)
        {
            if (pastPos == rb.position)
            {
                currAcceleration = Vector3.zero;
            }
            else
            {
                if (dir.magnitude <= 0.1f)
                {
                    dir = Vector3.zero;
                }
                else
                {
                    dir = dir.normalized * Mathf.Pow(2, dir.magnitude);
                }
                currAcceleration = Vector3.Lerp(currAcceleration, dir * followAcceleration, followSmoothing);
                currAcceleration.z = 0;
            }
            combinedVelocity = currAcceleration * Time.deltaTime + rb.velocity;
            if (rb.useGravity)
            {
                combinedVelocity.y = FollowFall(combinedVelocity.y);
            }
            if (followDrag > 0)
            {
                combinedVelocity /= 1 + followDrag;
            }
            if (combinedVelocity.magnitude > maxFollowSpeed)
            {
                combinedVelocity = combinedVelocity.normalized * maxFollowSpeed;
                currAcceleration = (combinedVelocity - rb.velocity) / Time.deltaTime;
            }
        }
        Turn();
        combinedVelocity.z = 0;
        rb.velocity = combinedVelocity;
    }

    void Update()
    {
        if (mode == Mode.Active)
        {
            GetKeyDownInput();
            GetKeyUpInput();
            if (inputJD)
            {
                jumpInput = true;
                StopCoroutine(StoreJumpInput());
                StartCoroutine(StoreJumpInput());
            }
            if (inputJU)
            {
                jumpHeld = false;
            }
        }
    }

    void GetKeyInput ()
    {
        inputML = Input.GetKey(KeyCode.A);
        inputMR = Input.GetKey(KeyCode.D);
    }

    void GetKeyDownInput ()
    {
        inputJD = Input.GetKeyDown(KeyCode.Space);
    }

    void GetKeyUpInput()
    {
        inputJU = Input.GetKeyUp(KeyCode.Space);
    }

    float Move()
    {
        if (inputMR)
        {
            if (frontDir == -1)
            {
                frontDir = 1;
                moveDir = Mathf.Lerp(moveDir, 1, moveSmoothness * Time.deltaTime);
            }
            moveDir = Mathf.Lerp(moveDir, 1, moveSmoothness * Time.deltaTime);
        }
        else if (inputML)
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

    float FollowFall (float addTo)
    {
        float fallVel = addTo;
        fallVel -= (gravityMultiplier + fallMultiplier) * 10 * Time.deltaTime;
        if (rb.velocity.y < -maxFollowSpeed)
        {
            fallVel = -maxFollowSpeed;
        }
        return fallVel;
    }

    float Fall()
    {
        float fallVel = rb.velocity.y;
        fallVel -= gravityMultiplier * 10 * Time.deltaTime;
        if (rb.velocity.y >= 0 && !jumpHeld)
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
            jumpHeld = true;
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

    IEnumerator FollowPastPosition(Vector3 storedPosition)
    {
        yield return new WaitForSeconds(followDelay);
        pastPos = storedPosition;
    }
}
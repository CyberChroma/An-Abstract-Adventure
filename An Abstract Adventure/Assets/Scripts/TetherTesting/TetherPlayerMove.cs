using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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
    [HideInInspector] public int frontDir;
    private bool moveOverride;
    private float moveDir;

    [Header("Jump")]
    public float jumpForce;
    public float lowJumpMultiplier;
    public float fallMultiplier;
    public float gravityMultiplier;
    public float terminalVelocity;

    private bool jumpInput;
    private bool canJump;
    private bool jumpHeld;

    [Header("Ground Detection")]
    public float jumpInputStoreTime;
    public float fallJumpDelay;

    [HideInInspector] public bool isGrounded;

    [Header("Tether Follow Pull")]
    public TetherPlayerMove otherPlayerMove;
    public float followDelay;
    public float followAcceleration;
    public float followDrag;
    public float followSmoothing;
    public float maxFollowSpeed;

    private Vector3 pastPos;
    private Vector3 combinedVelocity;
    private Vector3 currAcceleration;
    private Vector3 currActivePullAcc;

    [Header("Tether Active Pull")]
    public float activePullStartDis;
    public float activePullAcceleration;
    public float activePullSmoothing;

    // CUBE ABILITIES
    [Header("Wall Jump")]
    [HideInInspector] public bool wallJumpUnlocked;
    [HideInInspector] public float wallJumpHForce;
    [HideInInspector] public float wallJumpVForce;
    [HideInInspector] public float wallJumpInputStoreTime;
    [HideInInspector] public float wallJumpDragH;
    [HideInInspector] public float wallJumpDragV;

    private bool wallJumpInput;
    private int wallDir;
    private GameObject wallContact;
    private Vector2 currWallJumpVelocity;

    // SPHERE ABILITIES
    [Header("Glide")]
    [HideInInspector] public bool glideUnlocked;
    [HideInInspector] public float glideDrag;

    private bool canGlide;

    // Inputs
    private bool inputML;
    private bool inputMR;
    private bool inputJD;
    private bool inputJU;

    // Component References
    private Rigidbody rb;
    //private Animator anim;

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
            if (wallJumpUnlocked)
            {
                combinedVelocity += WallJump();
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
                if (wallJumpUnlocked)
                {
                    wallJumpInput = true;
                    StopCoroutine(StoreWallJumpInput());
                    StartCoroutine(StoreWallJumpInput());
                }
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
        if (!moveOverride)
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
        if (rb.velocity.y >= 0 && !jumpHeld && currWallJumpVelocity == Vector2.zero)
        {
            fallVel -= lowJumpMultiplier * 10 * Time.deltaTime;
        }
        else if (rb.velocity.y < 0)
        {
            fallVel -= fallMultiplier * 10 * Time.deltaTime;
            if (wallJumpUnlocked && wallContact)
            {
                fallVel /= 2;
            }
        }
        if (fallVel < -terminalVelocity)
        {
            fallVel = -terminalVelocity;
        }
        return fallVel;
    }

    float FollowFall(float addTo)
    {
        float fallVel = addTo;
        fallVel -= (gravityMultiplier + fallMultiplier) * 10 * Time.deltaTime;
        if (rb.velocity.y < -maxFollowSpeed)
        {
            fallVel = -maxFollowSpeed;
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
            if (wallJumpUnlocked)
            {
                StopCoroutine(StoreWallJumpInput());
                wallJumpInput = false;
            }
        }
        return jumpVel;
    }

    IEnumerator StoreJumpInput ()
    {
        yield return new WaitForSeconds(jumpInputStoreTime);
        jumpInput = false;
    }

    IEnumerator StoreWallJumpInput()
    {
        yield return new WaitForSeconds(wallJumpInputStoreTime);
        wallJumpInput = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Ground Check
        if (!isGrounded && collision.gameObject.layer == 8 && Mathf.Abs(collision.contacts[0].normal.x) < 0.9f && collision.contacts[0].point.y < transform.position.y)
        {
            isGrounded = true;
            canJump = true;
            currWallJumpVelocity = Vector2.zero;
        }

        // Wall Jump
        if (collision.gameObject.layer == 8 && !collision.collider.CompareTag("Slippery"))
        {
            if (Mathf.Abs(collision.contacts[0].normal.x) >= 0.9f)
            {
                wallContact = collision.gameObject;
                if (collision.contacts[0].point.x > transform.position.x)
                {
                    wallDir = 1;
                }
                else
                {
                    wallDir = -1;
                }
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        // Ground Check
        if (isGrounded && collision.gameObject.layer == 8 && !Physics.Raycast(transform.position, -transform.up, 0.5f, 1 << 8))
        {
            isGrounded = false;
            StopCoroutine(WaitToCancelJump());
            StartCoroutine(WaitToCancelJump());
        }

        // Wall Jump
        if (collision.gameObject == wallContact)
        {
            rb.useGravity = true;
            moveOverride = false;
            wallContact = null;
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

    Vector3 WallJump()
    {
        Vector3 wallJumpVelocity = Vector3.zero;
        if (wallContact != null && !isGrounded && wallJumpInput)
        {
            frontDir = -wallDir;
            currWallJumpVelocity = transform.up * wallJumpVForce * 10 + Vector3.right * frontDir * wallJumpHForce * 10;
            rb.useGravity = true;
            wallJumpInput = false;
            wallContact = null;
            StopCoroutine(StoreJumpInput());
            jumpInput = false;
            StopCoroutine("InputOveride");
            StartCoroutine(InputOveride(0.25f, 0));
            wallJumpVelocity = currWallJumpVelocity;
        }
        else if (currWallJumpVelocity != Vector2.zero && wallJumpDragH > 0 && wallJumpDragV > 0)
        {
            float lastWallJumpVelocityY = currWallJumpVelocity.y;
            currWallJumpVelocity.x /= 1 + wallJumpDragH;
            currWallJumpVelocity.y /= 1 + wallJumpDragV;
            if (currWallJumpVelocity.magnitude <= 0.5f)
            {
                currWallJumpVelocity = Vector2.zero;
            }
            wallJumpVelocity.x = currWallJumpVelocity.x;
            wallJumpVelocity.y = lastWallJumpVelocityY - currWallJumpVelocity.y;
        }
        return wallJumpVelocity;
    }

    IEnumerator InputOveride(float delay, float setMoveDir)
    {
        moveOverride = true;
        moveDir = setMoveDir;
        yield return new WaitForSeconds(delay);
        moveOverride = false;
    }
}

[CustomEditor(typeof(TetherPlayerMove))]
public class TetherPlayerMove_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        TetherPlayerMove tetherPlayerMove = (TetherPlayerMove)target;

        if (tetherPlayerMove.name.Contains("Cube"))
        {
            EditorGUILayout.LabelField("", EditorStyles.whiteLabel);
            EditorGUILayout.LabelField("Wall Jump", EditorStyles.boldLabel);
            tetherPlayerMove.wallJumpUnlocked = EditorGUILayout.Toggle("Wall Jump Unlocked", tetherPlayerMove.wallJumpUnlocked);
            if (tetherPlayerMove.wallJumpUnlocked)
            {
                tetherPlayerMove.wallJumpHForce = EditorGUILayout.FloatField("Wall Jump H Force", tetherPlayerMove.wallJumpHForce);
                tetherPlayerMove.wallJumpVForce = EditorGUILayout.FloatField("Wall Jump V Force", tetherPlayerMove.wallJumpVForce);
                tetherPlayerMove.wallJumpInputStoreTime = EditorGUILayout.FloatField("Wall Jump Input Store Time", tetherPlayerMove.wallJumpInputStoreTime);
                tetherPlayerMove.wallJumpDragH = EditorGUILayout.FloatField("Wall Jump Drag H", tetherPlayerMove.wallJumpDragH);
                tetherPlayerMove.wallJumpDragV = EditorGUILayout.FloatField("Wall Jump Drag V", tetherPlayerMove.wallJumpDragV);
            }
        }
        else if (tetherPlayerMove.name.Contains("Sphere"))
        {
            EditorGUILayout.LabelField("Glide", EditorStyles.boldLabel);
        }
    }
}

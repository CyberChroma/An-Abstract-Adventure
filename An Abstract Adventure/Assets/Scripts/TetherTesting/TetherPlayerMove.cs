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
    [HideInInspector] public bool canJump;
    private bool jumpInput;
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

    [Header("Dash")]
    [HideInInspector] public bool dashUnlocked;
    [HideInInspector] public float dashDis;
    [HideInInspector] public float dashStopTime;
    [HideInInspector] public float dashSpeedMultiplier;
    private bool isDashing;
    private bool canDash;
    private Vector3 moveToSpot;

    [Header("Dash Wall Jump")]
    [HideInInspector] public bool dashWallJumpUnlocked;
    [HideInInspector] public float dashWallJumpForce;
    private bool canDashWallJump;

    // SPHERE ABILITIES
    [Header("Glide")]
    [HideInInspector] public bool glideUnlocked;
    [HideInInspector] public float glideFallVelocity;

    [Header("Slam")]
    [HideInInspector] public bool slamUnlocked;
    [HideInInspector] public float slamSpeed;
    [HideInInspector] public float slamStopTime;
    private bool isSlaming;
    private bool isSlamPaused;

    // Inputs
    private bool inputML;
    private bool inputMR;
    private bool inputJD;
    private bool inputJU;
    private bool inputAD;

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
            if (dashWallJumpUnlocked && canDashWallJump)
            {
                combinedVelocity.y = DashWallJump();
            }
            else if (dashUnlocked && isDashing)
            {
                combinedVelocity = (moveToSpot - rb.position) * dashSpeedMultiplier;
            }
            else if (slamUnlocked && !isGrounded && (isSlaming || isSlamPaused))
            {
                if (isSlaming)
                {
                    combinedVelocity = Vector3.down * slamSpeed * 10;
                }
                else if (isSlamPaused)
                {
                    combinedVelocity = Vector3.zero;
                }
            }
            else
            {
                combinedVelocity.x = Move();
                combinedVelocity.y = Jump();
                if (!isGrounded && rb.useGravity && combinedVelocity.y == 0)
                {
                    combinedVelocity.y = Fall();
                }
                if (wallJumpUnlocked)
                {
                    if (wallContact && !isGrounded && wallJumpInput)
                    {
                        combinedVelocity = WallJump();
                    }
                    else if (currWallJumpVelocity != Vector2.zero && wallJumpDragH > 0 && wallJumpDragV > 0)

                    {
                        combinedVelocity += WallJumpFall();
                    }
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
            if (inputAD)
            {
                if (dashUnlocked && canDash)
                {
                    StartCoroutine(Dash());
                }
                if (slamUnlocked && !isSlamPaused && !isSlaming)
                {
                    StartCoroutine(Slam());
                }
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
        inputAD = Input.GetKeyDown(KeyCode.Mouse1);
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
        if (glideUnlocked && !isGrounded && rb.velocity.y < 0 && jumpHeld)
        {
            fallVel = -glideFallVelocity;
        }
        else
        {
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
            StopCoroutine(StoreJumpInput());
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
        if (collision.gameObject.layer == 8) {
            // Ground Check
            if (!isGrounded && Mathf.Abs(collision.contacts[0].normal.x) < 0.9f && collision.contacts[0].point.y < transform.position.y)
            {
                isGrounded = true;
                canJump = true;
                currWallJumpVelocity = Vector2.zero;
                canDash = true;
            }

            // Wall Jump
            if (!collision.collider.CompareTag("Slippery"))
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

                    //Dash Wall Jump
                    if (dashWallJumpUnlocked && isDashing)
                    {
                        canDashWallJump = true;
                    }
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
        frontDir = -wallDir;
        currWallJumpVelocity = transform.up * wallJumpVForce * 10 + Vector3.right * frontDir * wallJumpHForce * 10;
        rb.useGravity = true;
        wallJumpInput = false;
        wallContact = null;
        canDash = true;
        StopCoroutine(StoreJumpInput());
        jumpInput = false;
        StopCoroutine("InputOveride");
        StartCoroutine(InputOveride(0.25f, 0));
        wallJumpVelocity = currWallJumpVelocity;
        return wallJumpVelocity;
    }

    Vector3 WallJumpFall ()
    {
        Vector3 wallJumpVelocity = Vector3.zero;
        float lastWallJumpVelocityY = currWallJumpVelocity.y;
        currWallJumpVelocity.x /= 1 + wallJumpDragH;
        currWallJumpVelocity.y /= 1 + wallJumpDragV;
        if (currWallJumpVelocity.magnitude <= 0.5f)
        {
            currWallJumpVelocity = Vector2.zero;
        }
        wallJumpVelocity.x = currWallJumpVelocity.x;
        wallJumpVelocity.y = lastWallJumpVelocityY - currWallJumpVelocity.y;
        return wallJumpVelocity;
    }

    IEnumerator InputOveride(float delay, float setMoveDir)
    {
        moveOverride = true;
        moveDir = setMoveDir;
        yield return new WaitForSeconds(delay);
        moveOverride = false;
    }

    IEnumerator Dash()
    {
        isDashing = true;
        canDash = false;
        moveToSpot = new Vector3(transform.position.x + dashDis * frontDir, transform.position.y, transform.position.z);
        rb.useGravity = false;
        currWallJumpVelocity = Vector2.zero;
        yield return new WaitForSeconds(dashStopTime);
        rb.useGravity = true;
        isDashing = false;
        moveDir = frontDir;
        if (isGrounded)
        {
            yield return new WaitForSeconds(0.1f);
            canDash = true;
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        // Dash
        if (isDashing && collision.CompareTag("Dash"))
        {
            collision.gameObject.SetActive(false);
        }

        // Slam
        if (isSlaming && collision.CompareTag("Slam"))
        {
            collision.gameObject.SetActive(false);
        }
    }

    float DashWallJump()
    {
        float dashWallJumpVel = 0;
        dashWallJumpVel = dashWallJumpForce * 10;
        canJump = false;
        StopCoroutine(StoreJumpInput());
        jumpInput = false;
        isGrounded = false;
        jumpHeld = true;
        StopCoroutine(StoreWallJumpInput());
        wallJumpInput = false;
        StopCoroutine(Dash());
        rb.useGravity = true;
        moveDir = 0;
        isDashing = false;
        canDashWallJump = false;
        return dashWallJumpVel;
    }

    IEnumerator Slam()
    {
        rb.useGravity = false;
        isSlamPaused = true;
        yield return new WaitForSeconds(slamStopTime);
        isSlaming = true;
        isSlamPaused = false;
        while (!isGrounded)
        {
            yield return null;
        }
        rb.useGravity = true;
        isSlaming = false;
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

            EditorGUILayout.LabelField("", EditorStyles.whiteLabel);
            EditorGUILayout.LabelField("Dash", EditorStyles.boldLabel);
            tetherPlayerMove.dashUnlocked = EditorGUILayout.Toggle("Dash Unlocked", tetherPlayerMove.dashUnlocked);
            if (tetherPlayerMove.dashUnlocked)
            {
                tetherPlayerMove.dashDis = EditorGUILayout.FloatField("Dash Dis", tetherPlayerMove.dashDis);
                tetherPlayerMove.dashStopTime = EditorGUILayout.FloatField("Dash Stop Time", tetherPlayerMove.dashStopTime);
                tetherPlayerMove.dashSpeedMultiplier = EditorGUILayout.FloatField("Dash Speed Multiplier", tetherPlayerMove.dashSpeedMultiplier);

                EditorGUILayout.LabelField("", EditorStyles.whiteLabel);
                EditorGUILayout.LabelField("Dash Wall Jump", EditorStyles.boldLabel);
                tetherPlayerMove.dashWallJumpUnlocked = EditorGUILayout.Toggle("Dash Wall Jump Unlocked", tetherPlayerMove.dashWallJumpUnlocked);
                if (tetherPlayerMove.dashWallJumpUnlocked)
                {
                    tetherPlayerMove.dashWallJumpForce = EditorGUILayout.FloatField("Dash Wall Jump Force", tetherPlayerMove.dashWallJumpForce);
                }
            }
        }
        else if (tetherPlayerMove.name.Contains("Sphere"))
        {
            EditorGUILayout.LabelField("", EditorStyles.whiteLabel);
            EditorGUILayout.LabelField("Glide", EditorStyles.boldLabel);
            tetherPlayerMove.glideUnlocked = EditorGUILayout.Toggle("Glide Unlocked", tetherPlayerMove.glideUnlocked);
            if (tetherPlayerMove.glideUnlocked)
            {
                tetherPlayerMove.glideFallVelocity = EditorGUILayout.FloatField("Glide Fall Velocity", tetherPlayerMove.glideFallVelocity);
            }

            EditorGUILayout.LabelField("", EditorStyles.whiteLabel);
            EditorGUILayout.LabelField("Slam", EditorStyles.boldLabel);
            tetherPlayerMove.slamUnlocked = EditorGUILayout.Toggle("Slam Unlocked", tetherPlayerMove.slamUnlocked);
            if (tetherPlayerMove.slamUnlocked)
            {
                tetherPlayerMove.slamSpeed = EditorGUILayout.FloatField("Slam Speed", tetherPlayerMove.slamSpeed);
                tetherPlayerMove.slamStopTime = EditorGUILayout.FloatField("Slam Stop Time", tetherPlayerMove.slamStopTime);
            }
        }
    }
}

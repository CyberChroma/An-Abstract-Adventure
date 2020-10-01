using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActivePlayer
{
    Cube,
    Sphere
}

public class RhythmPlayerMove : MonoBehaviour
{
    [Header("Move")]
    public float speed;
    public float moveSmoothness;
    public float rotSmoothing;
    [HideInInspector] public int frontDir;
    private bool moveOverride;
    private float moveDir;
    private Vector3 combinedVelocity;

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

    [Header("Switching")]
    public ActivePlayer activePlayer;
    public float switchTime;
    public Collider cubeCollider;
    public Collider sphereCollider;
    public Camera cubeCamera;
    public Camera sphereCamera;

    // CUBE ABILITIES
    [Header("Wall Jump")]
    public bool wallJumpUnlocked;
    public float wallJumpHForce;
    public float wallJumpVForce;
    public float wallJumpInputStoreTime;
    public float wallJumpDragH;
    public float wallJumpDragV;
    private bool wallJumpInput;
    private int wallDir;
    private GameObject wallContact;
    private Vector2 currWallJumpVelocity;

    [Header("Dash")]
    public bool dashUnlocked;
    public float dashDis;
    public float dashStopTime;
    public float dashSpeedMultiplier;
    private bool isDashing;
    private bool canDash;
    private Vector3 moveToSpot;

    [Header("Dash Wall Jump")]
    public bool dashWallJumpUnlocked;
    public float dashWallJumpForce;
    private bool canDashWallJump;

    // SPHERE ABILITIES
    [Header("Glide")]
    public bool glideUnlocked;
    public float glideFallVelocity;

    [Header("Slam")]
    public bool slamUnlocked;
    public float slamSpeed;
    public float slamStopTime;
    private bool isSlaming;
    private bool isSlamPaused;

    [Header("Slam Bounce")]
    public bool slamBounceUnlocked;
    public float slamBounceForce;
    private bool canSlamBounce;
    private bool isSlamBouncing;

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
    }

    private void Start()
    {
        if (activePlayer == ActivePlayer.Cube)
        {
            sphereCollider.enabled = false;
            cubeCollider.enabled = true;
            sphereCamera.enabled = false;
            cubeCamera.enabled = true;
        }
        else
        {
            cubeCollider.enabled = false;
            sphereCollider.enabled = true;
            cubeCamera.enabled = false;
            sphereCamera.enabled = true;
        }
        StartCoroutine(WaitToSwitch());
    }

    void FixedUpdate()
    {
        GetKeyInput();
        combinedVelocity = Vector3.zero;
        if (activePlayer == ActivePlayer.Cube && dashWallJumpUnlocked && canDashWallJump)
        {
            combinedVelocity.y = DashWallJump();
        }
        else if (activePlayer == ActivePlayer.Cube && dashUnlocked && isDashing)
        {
            combinedVelocity = (moveToSpot - rb.position) * dashSpeedMultiplier;
        }
        else if (activePlayer == ActivePlayer.Sphere && slamBounceUnlocked && canSlamBounce)
        {
            combinedVelocity.y = SlamBounce();
        }
        else if (activePlayer == ActivePlayer.Sphere && slamUnlocked && !isGrounded && (isSlaming || isSlamPaused))
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
            if (activePlayer == ActivePlayer.Cube && wallJumpUnlocked)
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
        }
        //Turn();
        combinedVelocity.z = 0;
        rb.velocity = combinedVelocity;
    }

    void Update()
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

    void GetKeyInput()
    {
        inputML = Input.GetKey(KeyCode.A);
        inputMR = Input.GetKey(KeyCode.D);
    }

    void GetKeyDownInput()
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

    void Turn()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(new Vector3(0, 270 + frontDir * 90, 0)), rotSmoothing * Time.deltaTime);
    }

    float Fall()
    {
        float fallVel = rb.velocity.y;
        if (slamBounceUnlocked && isSlamBouncing)
        {
            fallVel -= gravityMultiplier * 10 * Time.deltaTime;
            if (rb.velocity.y < 0)
            {
                isSlamBouncing = false;
            }
        }
        else if (glideUnlocked && !isGrounded && rb.velocity.y < 0 && jumpHeld)
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

    IEnumerator StoreJumpInput()
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
        if (collision.gameObject.layer == 8)
        {
            // Ground Check
            if (!isGrounded && Mathf.Abs(collision.contacts[0].normal.x) < 0.9f && collision.contacts[0].point.y < transform.position.y)
            {
                isGrounded = true;
                canJump = true;
                currWallJumpVelocity = Vector2.zero;
                canDash = true;

                // Slam Bounce
                if (slamBounceUnlocked && isSlaming)
                {
                    canSlamBounce = true;
                }
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

    IEnumerator WaitToSwitch()
    {
        yield return new WaitForSeconds(switchTime);
        Switch();
    }

    void Switch ()
    {
        if (activePlayer == ActivePlayer.Cube)
        {
            cubeCollider.enabled = false;
            sphereCollider.enabled = true;
            cubeCamera.enabled = false;
            sphereCamera.enabled = true;
            activePlayer = ActivePlayer.Sphere;
        }
        else
        {
            sphereCollider.enabled = false;
            cubeCollider.enabled = true;
            sphereCamera.enabled = false;
            cubeCamera.enabled = true;
            activePlayer = ActivePlayer.Cube;
        }
        StartCoroutine(WaitToSwitch());
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

    Vector3 WallJumpFall()
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

    float SlamBounce()
    {
        float slamBounceVel = 0;
        slamBounceVel = slamBounceForce * 10;
        canJump = false;
        StopCoroutine(StoreJumpInput());
        jumpInput = false;
        isGrounded = false;
        jumpHeld = false;
        StopCoroutine(Slam());
        rb.useGravity = true;
        isSlaming = false;
        canSlamBounce = false;
        isSlamBouncing = true;
        return slamBounceVel;
    }
}

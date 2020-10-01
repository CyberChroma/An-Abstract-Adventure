using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActivePlayer
{
    Cube,
    Sphere
}

public class PlayerMain : MonoBehaviour
{
    public ActivePlayer activePlayer;
    [HideInInspector] public PlayerInput playerInput;
    [HideInInspector] public PlayerMove playerMove;
    [HideInInspector] public PlayerJump playerJump;
    [HideInInspector] public PlayerGroundDetection playerGroundDetection;
    [HideInInspector] public PlayerRhythmSwitch playerRhythmSwitch;
    [HideInInspector] public CubeWallJump cubeWallJump;
    [HideInInspector] public CubeDash cubeDash;
    [HideInInspector] public CubeDashLaunch cubeDashLaunch;
    [HideInInspector] public SphereGlide sphereGlide;
    [HideInInspector] public SphereSlam sphereSlam;
    [HideInInspector] public SphereBounce sphereBounce;

    private Vector3 combinedVelocity;

    // Component References
    [HideInInspector] public Rigidbody rb;
    //private Animator anim;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerMove = GetComponent<PlayerMove>();
        playerJump = GetComponent<PlayerJump>();
        playerGroundDetection = GetComponent<PlayerGroundDetection>();
        playerRhythmSwitch = GetComponent<PlayerRhythmSwitch>();
        cubeWallJump = GetComponent<CubeWallJump>();
        cubeDash = GetComponent<CubeDash>();
        cubeDashLaunch = GetComponent<CubeDashLaunch>();
        sphereGlide = GetComponent<SphereGlide>();
        sphereSlam = GetComponent<SphereSlam>();
        sphereBounce = GetComponent<SphereBounce>();
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        playerInput.GetKeyInput();
        combinedVelocity = Vector3.zero;
        if (activePlayer == ActivePlayer.Cube && cubeDashLaunch.dashLaunchUnlocked && cubeDashLaunch.canDashLaunch)
        {
            combinedVelocity.y = cubeDashLaunch.DashLaunch();
        }
        else if (activePlayer == ActivePlayer.Cube && cubeDash.dashUnlocked && cubeDash.isDashing)
        {
            combinedVelocity = (cubeDash.moveToSpot - rb.position) * cubeDash.speedMultiplier;
        }
        else if (activePlayer == ActivePlayer.Sphere && sphereBounce.slamBounceUnlocked && sphereBounce.canSlamBounce)
        {
            combinedVelocity.y = sphereBounce.SlamBounce();
        }
        else if (activePlayer == ActivePlayer.Sphere && sphereSlam.slamUnlocked && !playerGroundDetection.isGrounded && (sphereSlam.isSlaming || sphereSlam.isSlamPaused))
        {
            if (sphereSlam.isSlaming)
            {
                combinedVelocity = Vector3.down * sphereSlam.slamSpeed * 10;
            }
            else if (sphereSlam.isSlamPaused)
            {
                combinedVelocity = Vector3.zero;
            }
        }
        else
        {
            combinedVelocity.x = playerMove.Move();
            combinedVelocity.y = playerJump.Jump();
            if (!playerGroundDetection.isGrounded && rb.useGravity && combinedVelocity.y == 0)
            {
                combinedVelocity.y = playerJump.Fall();
            }
            if (cubeWallJump.wallJumpUnlocked)
            {
                if (activePlayer == ActivePlayer.Cube && cubeWallJump.wallContact && !playerGroundDetection.isGrounded && cubeWallJump.wallJumpInput)
                {
                    combinedVelocity = cubeWallJump.WallJump();
                }
                else if (cubeWallJump.currWallJumpVelocity != Vector2.zero && cubeWallJump.dragH > 0 && cubeWallJump.dragV > 0)

                {
                    combinedVelocity += cubeWallJump.WallJumpFall();
                }
            }
        }
        //playerMove.Turn();
        combinedVelocity.z = 0;
        rb.velocity = combinedVelocity;
    }

    void Update()
    {
        playerInput.GetKeyDownInput();
        playerInput.GetKeyUpInput();
        if (playerInput.inputJD)
        {
            playerJump.jumpInput = true;
            StopCoroutine(playerJump.StoreJumpInput());
            StartCoroutine(playerJump.StoreJumpInput());
            if (activePlayer == ActivePlayer.Cube && cubeWallJump.wallJumpUnlocked)
            {
                cubeWallJump.wallJumpInput = true;
                StopCoroutine(cubeWallJump.StoreWallJumpInput());
                StartCoroutine(cubeWallJump.StoreWallJumpInput());
            }
        }
        if (playerInput.inputJU)
        {
            playerJump.jumpHeld = false;
        }
        if (playerInput.inputAD)
        {
            if (activePlayer == ActivePlayer.Cube && cubeDash.dashUnlocked && cubeDash.canDash)
            {
                StartCoroutine(cubeDash.Dash());
            }
            if (activePlayer == ActivePlayer.Sphere && sphereSlam.slamUnlocked && !sphereSlam.isSlamPaused && !sphereSlam.isSlaming)
            {
                StartCoroutine(sphereSlam.Slam());
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChange : MonoBehaviour
{
    public enum ActivePlayer
    {
        Cube,
        Sphere
    }

    public ActivePlayer activePlayer;
    public CameraFollow cubeCamera;
    public CameraFollow sphereCamera;
    public GameObject cubeActiveUI;
    public GameObject sphereActiveUI;

    private SquareMain cubeMain;
    private CircleMain sphereMain;
    private PlayerMove cubeMove;
    private PlayerMove sphereMove;
    private PlayerGroundCheck cubeGroundCheck;
    private PlayerGroundCheck sphereGroundCheck;
    private Rigidbody cubeRb;
    private Rigidbody sphereRb;
    private SphereCollider cubeColl;
    private SphereCollider sphereColl;
    private Animator cubeAnim;
    private Animator sphereAnim;
    private PlayerFollowOther cubeFollowOther;
    private PlayerFollowOther sphereFollowOther;
    private PlayerFollowOther cubeCameraFollowOther;
    private PlayerFollowOther sphereCameraFollowOther;

    // Start is called before the first frame update
    void Awake()
    {
        cubeMain = FindObjectOfType<SquareMain>();
        sphereMain = FindObjectOfType<CircleMain>();
        cubeMove = cubeMain.GetComponent<PlayerMove>();
        sphereMove = sphereMain.GetComponent<PlayerMove>();
        cubeGroundCheck = cubeMain.GetComponent<PlayerGroundCheck>();
        sphereGroundCheck = sphereMain.GetComponent<PlayerGroundCheck>();
        cubeRb = cubeMain.GetComponent<Rigidbody>();
        sphereRb = sphereMain.GetComponent<Rigidbody>();
        cubeColl = cubeMain.GetComponent<SphereCollider>();
        sphereColl = sphereMain.GetComponent<SphereCollider>();
        cubeAnim = cubeMain.GetComponentInChildren<Animator>();
        sphereAnim = sphereMain.GetComponentInChildren<Animator>();
        cubeFollowOther = cubeMain.GetComponent<PlayerFollowOther>();
        sphereFollowOther = sphereMain.GetComponent<PlayerFollowOther>();
        cubeCameraFollowOther = cubeCamera.GetComponent<PlayerFollowOther>();
        sphereCameraFollowOther = sphereCamera.GetComponent<PlayerFollowOther>();
    }

    void Start()
    {
        if (activePlayer == ActivePlayer.Cube)
        {
            sphereMain.enabled = false;
            cubeMain.enabled = true;
            cubeCamera.player = cubeRb;
            cubeActiveUI.SetActive(true);
            sphereActiveUI.SetActive(false);
            cubeFollowOther.following = false;
            sphereFollowOther.following = true;
            cubeRb.isKinematic = false;
            sphereRb.isKinematic = true;
            cubeColl.enabled = true;
            sphereColl.enabled = false;
            cubeCameraFollowOther.following = false;
            sphereCameraFollowOther.following = true;
            if (cubeAnim)
            {
                cubeAnim.SetBool("NotActive", false);
            }
            if (sphereAnim)
            {
                sphereAnim.SetBool("NotActive", true);
            }
        }
        else
        {
            sphereMain.enabled = true;
            cubeMain.enabled = false;
            sphereCamera.player = sphereRb;
            cubeActiveUI.SetActive(false);
            sphereActiveUI.SetActive(true);
            cubeFollowOther.following = true;
            sphereFollowOther.following = false;
            cubeRb.isKinematic = true;
            sphereRb.isKinematic = false;
            cubeColl.enabled = false;
            sphereColl.enabled = true;
            cubeCameraFollowOther.following = true;
            sphereCameraFollowOther.following = false;
            if (cubeAnim)
            {
                cubeAnim.SetBool("NotActive", true);
            }
            if (sphereAnim)
            {
                sphereAnim.SetBool("NotActive", false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (activePlayer == ActivePlayer.Cube)
            {
                activePlayer = ActivePlayer.Sphere;
                sphereMain.enabled = true;
                cubeMain.enabled = false;
                cubeActiveUI.SetActive(false);
                sphereActiveUI.SetActive(true);
                cubeFollowOther.following = true;
                sphereFollowOther.following = false;
                sphereRb.isKinematic = false;
                sphereRb.velocity = cubeRb.velocity;
                cubeRb.isKinematic = true;
                cubeColl.enabled = false;
                sphereColl.enabled = true;
                cubeCameraFollowOther.following = true;
                sphereCameraFollowOther.following = false;
                sphereMove.moveDir = cubeMove.moveDir;
                sphereMove.frontDir = cubeMove.frontDir;
                cubeGroundCheck.isGrounded = false;
                if (cubeAnim)
                {
                    cubeAnim.SetBool("NotActive", true);
                }
                if (sphereAnim)
                {
                    sphereAnim.SetBool("NotActive", false);
                }
            }
            else
            {
                activePlayer = ActivePlayer.Cube;
                cubeMain.enabled = true;
                sphereMain.enabled = false;
                cubeActiveUI.SetActive(true);
                sphereActiveUI.SetActive(false);
                cubeFollowOther.following = false;
                sphereFollowOther.following = true;
                cubeRb.isKinematic = false;
                cubeRb.velocity = sphereRb.velocity;
                sphereRb.isKinematic = true;
                cubeColl.enabled = true;
                sphereColl.enabled = false;
                cubeCameraFollowOther.following = false;
                sphereCameraFollowOther.following = true;
                cubeMove.moveDir = sphereMove.moveDir;
                cubeMove.frontDir = sphereMove.frontDir;
                sphereGroundCheck.isGrounded = false;
                if (cubeAnim)
                {
                    cubeAnim.SetBool("NotActive", false);
                }
                if (sphereAnim)
                {
                    sphereAnim.SetBool("NotActive", true);
                }
            }
        }
    }
}

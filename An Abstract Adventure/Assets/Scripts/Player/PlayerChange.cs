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
    public Camera cubeCamera;
    public Camera sphereCamera;
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
    private Animator cubeAnim;
    private Animator sphereAnim;
    private CameraFollow cubeCameraFollow;
    private CameraFollow sphereCameraFollow;

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
        cubeAnim = cubeMain.GetComponentInChildren<Animator>();
        sphereAnim = sphereMain.GetComponentInChildren<Animator>();
        cubeCameraFollow = cubeCamera.GetComponent<CameraFollow>();
        sphereCameraFollow = sphereCamera.GetComponent<CameraFollow>();
        cubeCameraFollow.player = cubeRb;
        sphereCameraFollow.player = sphereRb;
    }

    void Start()
    {
        if (activePlayer == ActivePlayer.Cube)
        {
            sphereMain.enabled = false;
            cubeMain.enabled = true;
            cubeActiveUI.SetActive(true);
            sphereActiveUI.SetActive(false);
        }
        else
        {
            sphereMain.enabled = true;
            cubeMain.enabled = false;
            cubeActiveUI.SetActive(false);
            sphereActiveUI.SetActive(true);
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
            }
            else
            {
                activePlayer = ActivePlayer.Cube;
                cubeMain.enabled = true;
                sphereMain.enabled = false;
                cubeActiveUI.SetActive(true);
                sphereActiveUI.SetActive(false);
            }
        }
    }
}

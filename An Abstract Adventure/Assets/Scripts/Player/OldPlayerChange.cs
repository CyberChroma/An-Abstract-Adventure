using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldPlayerChange : MonoBehaviour
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

    private OldSquareMain cubeMain;
    private OldCircleMain sphereMain;
    private OldPlayerMove cubeMove;
    private OldPlayerMove sphereMove;
    private OldPlayerGroundCheck cubeGroundCheck;
    private OldPlayerGroundCheck sphereGroundCheck;
    private Rigidbody cubeRb;
    private Rigidbody sphereRb;
    private Animator cubeAnim;
    private Animator sphereAnim;
    private OldCameraFollow cubeCameraFollow;
    private OldCameraFollow sphereCameraFollow;

    // Start is called before the first frame update
    void Awake()
    {
        cubeMain = FindObjectOfType<OldSquareMain>();
        sphereMain = FindObjectOfType<OldCircleMain>();
        cubeMove = cubeMain.GetComponent<OldPlayerMove>();
        sphereMove = sphereMain.GetComponent<OldPlayerMove>();
        cubeGroundCheck = cubeMain.GetComponent<OldPlayerGroundCheck>();
        sphereGroundCheck = sphereMain.GetComponent<OldPlayerGroundCheck>();
        cubeRb = cubeMain.GetComponent<Rigidbody>();
        sphereRb = sphereMain.GetComponent<Rigidbody>();
        cubeAnim = cubeMain.GetComponentInChildren<Animator>();
        sphereAnim = sphereMain.GetComponentInChildren<Animator>();
        cubeCameraFollow = cubeCamera.GetComponent<OldCameraFollow>();
        sphereCameraFollow = sphereCamera.GetComponent<OldCameraFollow>();
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

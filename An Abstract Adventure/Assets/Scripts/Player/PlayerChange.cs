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

    private SquareMain cube;
    private CircleMain sphere;

    // Start is called before the first frame update
    void Awake()
    {
        cube = FindObjectOfType<SquareMain>();
        sphere = FindObjectOfType<CircleMain>();
    }

    void Start()
    {
        if (activePlayer == ActivePlayer.Cube)
        {
            sphere.enabled = false;
            cube.enabled = true;
            cubeCamera.player = cube.GetComponent<Rigidbody>();
            cubeActiveUI.SetActive(true);
            sphereActiveUI.SetActive(false);
        }
        else
        {
            sphere.enabled = true;
            cube.enabled = false;
            sphereCamera.player = sphere.GetComponent<Rigidbody>();
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
                sphere.enabled = true;
                cube.enabled = false;
                cubeActiveUI.SetActive(false);
                sphereActiveUI.SetActive(true);
            }
            else
            {
                activePlayer = ActivePlayer.Cube;
                cube.enabled = true;
                sphere.enabled = false;
                cubeActiveUI.SetActive(true);
                sphereActiveUI.SetActive(false);
            }
        }
    }
}

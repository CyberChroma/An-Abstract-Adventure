using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetherSwitch : MonoBehaviour
{
    public enum ActivePlayer
    {
        cube,
        sphere
    }

    public TetherPlayerMove cubePlayer;
    public TetherPlayerMove spherePlayer;
    public ActivePlayer activePlayer;
    public Camera cubeCamera;
    public Camera sphereCamera;

    // Start is called before the first frame update
    void Start()
    {
        cubePlayer.mode = TetherPlayerMove.Mode.Active;
        spherePlayer.mode = TetherPlayerMove.Mode.Following;
        cubeCamera.depth = 1;
        sphereCamera.depth = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (activePlayer == ActivePlayer.cube)
            {
                cubePlayer.mode = TetherPlayerMove.Mode.Following;
                spherePlayer.mode = TetherPlayerMove.Mode.Active;
                activePlayer = ActivePlayer.sphere;
                if (Physics.Raycast(spherePlayer.transform.position, -transform.up, 0.5f, 1 << 8))
                {
                    spherePlayer.isGrounded = true;
                }
                if (!spherePlayer.isGrounded)
                {
                    spherePlayer.canJump = false;
                }
                cubeCamera.depth = 0;
                sphereCamera.depth = 1;
            }
            else
            {
                cubePlayer.mode = TetherPlayerMove.Mode.Active;
                spherePlayer.mode = TetherPlayerMove.Mode.Following;
                activePlayer = ActivePlayer.cube;
                if (Physics.Raycast(cubePlayer.transform.position, -transform.up, 0.5f, 1 << 8))
                {
                    cubePlayer.isGrounded = true;
                }
                if (!cubePlayer.isGrounded)
                {
                    cubePlayer.canJump = false;
                }
                cubeCamera.depth = 1;
                sphereCamera.depth = 0;
            }
        }
    }
}

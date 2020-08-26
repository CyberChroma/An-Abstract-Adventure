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

    private TetherCameraFollow tetherCameraFollow;

    // Start is called before the first frame update
    void Start()
    {
        cubePlayer.mode = TetherPlayerMove.Mode.Active;
        spherePlayer.mode = TetherPlayerMove.Mode.Following;
        tetherCameraFollow = GetComponent<TetherCameraFollow>();
        tetherCameraFollow.player = cubePlayer.GetComponent<Rigidbody>();
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
                tetherCameraFollow.player = spherePlayer.GetComponent<Rigidbody>();
                activePlayer = ActivePlayer.sphere;
                if (Physics.Raycast(spherePlayer.transform.position, -transform.up, 0.5f, 1 << 8))
                {
                    spherePlayer.isGrounded = true;
                }
                if (!spherePlayer.isGrounded)
                {
                    spherePlayer.canJump = false;
                }
            }
            else
            {
                cubePlayer.mode = TetherPlayerMove.Mode.Active;
                spherePlayer.mode = TetherPlayerMove.Mode.Following;
                tetherCameraFollow.player = cubePlayer.GetComponent<Rigidbody>();
                activePlayer = ActivePlayer.cube;
                if (Physics.Raycast(cubePlayer.transform.position, -transform.up, 0.5f, 1 << 8))
                {
                    cubePlayer.isGrounded = true;
                }
                if (!cubePlayer.isGrounded)
                {
                    cubePlayer.canJump = false;
                }
            }
        }
    }
}

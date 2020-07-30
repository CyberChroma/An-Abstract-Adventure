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
        if (Input.GetMouseButtonDown(1))
        {
            if (activePlayer == ActivePlayer.cube)
            {
                cubePlayer.mode = TetherPlayerMove.Mode.Following;
                spherePlayer.mode = TetherPlayerMove.Mode.Active;
                tetherCameraFollow.player = spherePlayer.GetComponent<Rigidbody>();
                activePlayer = ActivePlayer.sphere;

                spherePlayer.rb.velocity = Vector3.zero;
                spherePlayer.currAcceleration = Vector3.zero;

                cubePlayer.rb.velocity = Vector3.zero;
                cubePlayer.currAcceleration = Vector3.zero;
            }
            else
            {
                cubePlayer.mode = TetherPlayerMove.Mode.Active;
                spherePlayer.mode = TetherPlayerMove.Mode.Following;
                tetherCameraFollow.player = cubePlayer.GetComponent<Rigidbody>();
                activePlayer = ActivePlayer.cube;

                cubePlayer.rb.velocity = Vector3.zero;
                cubePlayer.currAcceleration = Vector3.zero;

                spherePlayer.rb.velocity = Vector3.zero;
                spherePlayer.currAcceleration = Vector3.zero;
            }
        }
    }
}

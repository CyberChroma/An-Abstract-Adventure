using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetherSwitch : MonoBehaviour
{
    public TetherPlayerMove cubePlayer;
    public TetherPlayerMove spherePlayer;

    private bool cubeActive;
    private TetherCameraFollow tetherCameraFollow;

    // Start is called before the first frame update
    void Start()
    {
        cubePlayer.active = true;
        spherePlayer.active = false;
        tetherCameraFollow = GetComponent<TetherCameraFollow>();
        tetherCameraFollow.player = cubePlayer.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (cubeActive)
            {
                cubePlayer.active = false;
                spherePlayer.active = true;
                tetherCameraFollow.player = spherePlayer.GetComponent<Rigidbody>();
            }
            else
            {
                cubePlayer.active = true;
                spherePlayer.active = false;
                tetherCameraFollow.player = cubePlayer.GetComponent<Rigidbody>();
            }
            cubeActive = !cubeActive;
        }
    }
}

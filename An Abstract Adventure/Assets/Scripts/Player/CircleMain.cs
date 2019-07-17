using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleMain : MonoBehaviour
{
    private PlayerMove playerMove;
    private PlayerJump playerJump;
    private PlayerDoubleJump playerDoubleJump;

    void Start()
    {
        playerMove = GetComponent<PlayerMove>();
        playerJump = GetComponent<PlayerJump>();
        playerDoubleJump = GetComponent<PlayerDoubleJump>();
    }

    void Update()
    {
        playerMove.Move();
        playerDoubleJump.DoubleJump();
        playerJump.Jump();
    }

    void FixedUpdate()
    {
        playerJump.Fall();
    }
}

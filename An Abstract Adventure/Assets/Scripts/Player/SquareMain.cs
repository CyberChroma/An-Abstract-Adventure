using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareMain : MonoBehaviour
{
    [HideInInspector] public bool activePlayer;
    [HideInInspector] public PlayerMove playerMove;

    private PlayerJump playerJump;
    private PlayerDoubleJump playerDoubleJump;
    private PlayerAttack playerAttack;
    private SquareWallJump squareWallJump;

    void Awake()
    {
        playerMove = GetComponent<PlayerMove>();
        playerJump = GetComponent<PlayerJump>();
        playerDoubleJump = GetComponent<PlayerDoubleJump>();
        playerAttack = GetComponent<PlayerAttack>();
        squareWallJump = GetComponent<SquareWallJump>();
    }

    private void Update()
    {
        if (activePlayer)
        {
            playerDoubleJump.DoubleJump();
            squareWallJump.WallJump();
            playerJump.Jump();
            playerAttack.Attack();
        }
    }

    void FixedUpdate()
    {
        if (activePlayer)
        {
            playerMove.Move();
        }
    }

    void OnEnable()
    {
        playerMove.active = true;
    }

    void OnDisable()
    {
        playerMove.active = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareMain : MonoBehaviour
{
    [HideInInspector] public bool activePlayer;

    private PlayerMove playerMove;
    private PlayerJump playerJump;
    private PlayerDoubleJump playerDoubleJump;
    private PlayerAttack playerAttack;

    void Awake()
    {
        playerMove = GetComponent<PlayerMove>();
        playerJump = GetComponent<PlayerJump>();
        playerDoubleJump = GetComponent<PlayerDoubleJump>();
        playerAttack = GetComponent<PlayerAttack>();
    }

    void Update()
    {
        if (activePlayer)
        {
            playerMove.Move();
            playerDoubleJump.DoubleJump();
            playerJump.Jump();
            playerAttack.Attack();
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

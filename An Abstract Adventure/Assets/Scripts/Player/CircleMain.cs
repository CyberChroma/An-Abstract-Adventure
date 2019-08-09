using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleMain : MonoBehaviour
{
    [HideInInspector] public bool activePlayer;
    [HideInInspector] public PlayerMove playerMove;

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

    private void Update()
    {
        if (activePlayer)
        {
            playerDoubleJump.DoubleJump();
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

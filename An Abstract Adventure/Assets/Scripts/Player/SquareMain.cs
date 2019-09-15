﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareMain : MonoBehaviour
{
    [HideInInspector] public bool activePlayer;
    [HideInInspector] public PlayerMove playerMove;
    [HideInInspector] public PlayerLineUp playerLineUp;

    private PlayerJump playerJump;
    private PlayerDoubleJump playerDoubleJump;
    private PlayerAttack playerAttack;
    private SquareWallJump squareWallJump;
    private SquareShurikenThrow squareShuikenThrow;
    private SquareCrouch squareCrouch;

    void Awake()
    {
        playerMove = GetComponent<PlayerMove>();
        playerJump = GetComponent<PlayerJump>();
        playerDoubleJump = GetComponent<PlayerDoubleJump>();
        playerAttack = GetComponent<PlayerAttack>();
        playerLineUp = GetComponent<PlayerLineUp>();
        squareWallJump = GetComponent<SquareWallJump>();
        squareShuikenThrow = GetComponent<SquareShurikenThrow>();
        squareCrouch = GetComponent<SquareCrouch>();
    }

    private void Update()
    {
        if (activePlayer)
        {
            playerLineUp.LineUp();
            squareShuikenThrow.ShurikenThrow();
            squareCrouch.Crouch();
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

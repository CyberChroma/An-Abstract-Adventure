﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleMain : MonoBehaviour
{
    [HideInInspector] public bool activePlayer;
    [HideInInspector] public PlayerMove playerMove;
    [HideInInspector] public PlayerLineUp playerLineUp;
    
    private PlayerJump playerJump;
    private PlayerDoubleJump playerDoubleJump;
    private PlayerAttack playerAttack;
    private CircleLungeSlash circleLungeSlash;
    private CircleGlide circleGlide;

    void Awake()
    {
        playerMove = GetComponent<PlayerMove>();
        playerJump = GetComponent<PlayerJump>();
        playerDoubleJump = GetComponent<PlayerDoubleJump>();
        playerAttack = GetComponent<PlayerAttack>();
        playerLineUp = GetComponent<PlayerLineUp>();
        circleLungeSlash = GetComponent<CircleLungeSlash>();
        circleGlide = GetComponent<CircleGlide>();
    }

    private void Update()
    {
        if (activePlayer)
        {
            circleGlide.Glide();
            playerLineUp.LineUp();
            circleLungeSlash.LungeSlash();
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

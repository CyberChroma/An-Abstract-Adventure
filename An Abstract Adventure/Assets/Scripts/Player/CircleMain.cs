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
    private CircleReflect circleReflect;

    void Awake()
    {
        playerMove = GetComponent<PlayerMove>();
        playerJump = GetComponent<PlayerJump>();
        playerDoubleJump = GetComponent<PlayerDoubleJump>();
        playerAttack = GetComponent<PlayerAttack>();
        playerLineUp = GetComponent<PlayerLineUp>();
        circleLungeSlash = GetComponent<CircleLungeSlash>();
        circleGlide = GetComponent<CircleGlide>();
        circleReflect = GetComponent<CircleReflect>();
    }

    private void Update()
    {
        if (activePlayer)
        {
            circleGlide.Glide();
            circleReflect.Reflect();
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

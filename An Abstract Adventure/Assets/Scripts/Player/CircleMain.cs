using System.Collections;
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
    private PlayerSwim playerSwim;
    private PlayerSwimBoost playerSwimBoost;
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
        playerSwim = GetComponent<PlayerSwim>();
        playerSwimBoost = GetComponent<PlayerSwimBoost>();
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
            playerAttack.Attack();
            if (playerSwim.swimming)
            {
                playerSwimBoost.SwimBoost();
            }
            else
            {
                playerDoubleJump.DoubleJump();
                playerJump.Jump();
            }
        }
    }

    void FixedUpdate()
    {
        if (activePlayer)
        {
            if (playerSwim.swimming)
            {
                playerSwim.Swim();
            }
            else
            {
                playerMove.Move();
            }
        }
    }

    void OnEnable()
    {
        playerMove.active = true;
    }

    void OnDisable()
    {
        playerMove.active = false;
        playerLineUp.DisableArrow();
        circleReflect.DisableShield();
        playerMove.disableMove = false;
        playerJump.disableJump = false;
        playerDoubleJump.disableDoubleJump = false;
    }
}

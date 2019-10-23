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
    private PlayerInteract playerInteract;
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
        playerInteract = GetComponent<PlayerInteract>();
        circleLungeSlash = GetComponent<CircleLungeSlash>();
        circleGlide = GetComponent<CircleGlide>();
        circleReflect = GetComponent<CircleReflect>();
    }

    private void Update()
    {
        if (activePlayer)
        {
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
                circleGlide.Glide();
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
        playerInteract.canInteract = true;
    }

    void OnDisable()
    {
        playerMove.active = false;
        playerLineUp.DisableArrow();
        circleReflect.DisableShield();
        playerMove.disableMove = false;
        playerJump.disableJump = false;
        playerDoubleJump.disableDoubleJump = false;
        playerInteract.canInteract = false;
    }
}

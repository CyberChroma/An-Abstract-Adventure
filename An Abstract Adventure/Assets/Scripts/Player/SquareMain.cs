using System.Collections;
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
    private PlayerSwim playerSwim;
    private PlayerSwimBoost playerSwimBoost;
    private PlayerInteract playerInteract;
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
        playerSwim = GetComponent<PlayerSwim>();
        playerSwimBoost = GetComponent<PlayerSwimBoost>();
        playerInteract = GetComponent<PlayerInteract>();
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
            playerAttack.Attack();
            if (playerSwim.swimming)
            {
                playerSwimBoost.SwimBoost();
            }
            else
            {
                playerDoubleJump.DoubleJump();
                squareWallJump.WallJump();
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
        playerMove.disableMove = false;
        playerJump.disableJump = false;
        playerDoubleJump.disableDoubleJump = false;
        playerInteract.canInteract = false;
    }
}

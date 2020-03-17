using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareMain : MonoBehaviour
{
    [HideInInspector] public PlayerMove playerMove;

    private PlayerJump playerJump;
    //private PlayerSwim playerSwim;
    //private PlayerSwimBoost playerSwimBoost;
    private PlayerInteract playerInteract;
    private SquareWallJump squareWallJump;
    private SquareDash squareDash;

    void Awake()
    {
        playerMove = GetComponent<PlayerMove>();
        playerJump = GetComponent<PlayerJump>();
        //playerSwim = GetComponent<PlayerSwim>();
        //playerSwimBoost = GetComponent<PlayerSwimBoost>();
        playerInteract = GetComponent<PlayerInteract>();
        squareWallJump = GetComponent<SquareWallJump>();
        squareDash = GetComponent<SquareDash>();
    }

    private void Update()
    {
        squareDash.Dash();
        /*if (playerSwim.swimming)
        {
            playerSwimBoost.SwimBoost();
        }
        else
        {*/
            squareWallJump.WallJump();
            playerJump.Jump();
        //}
    }

    void FixedUpdate()
    {
        /*if (playerSwim.swimming)
        {
            playerSwim.Swim();
        }
        else
        {*/
            playerMove.Move();
        //}
    }

    void OnEnable()
    {
        playerMove.active = true;
        playerInteract.canInteract = true;
    }

    void OnDisable()
    {
        playerMove.active = false;
        playerMove.disableMove = false;
        playerJump.disableJump = false;
        playerInteract.canInteract = false;
    }
}

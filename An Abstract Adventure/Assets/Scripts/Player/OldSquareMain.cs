using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldSquareMain : MonoBehaviour
{
    [HideInInspector] public OldPlayerMove playerMove;
    [HideInInspector] public bool isActive;

    private OldPlayerJump playerJump;
    //private OldPlayerSwim playerSwim;
    //private OldPlayerSwimBoost playerSwimBoost;
    private OldPlayerInteract playerInteract;
    private OldSquareWallJump squareWallJump;
    private OldSquareDash squareDash;

    void Awake()
    {
        playerMove = GetComponent<OldPlayerMove>();
        playerJump = GetComponent<OldPlayerJump>();
        //playerSwim = GetComponent<OldPlayerSwim>();
        //playerSwimBoost = GetComponent<OldPlayerSwimBoost>();
        playerInteract = GetComponent<OldPlayerInteract>();
        squareWallJump = GetComponent<OldSquareWallJump>();
        squareDash = GetComponent<OldSquareDash>();
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

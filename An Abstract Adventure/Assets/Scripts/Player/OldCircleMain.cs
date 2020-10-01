using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldCircleMain : MonoBehaviour
{
    [HideInInspector] public OldPlayerMove playerMove;
    
    private OldPlayerJump playerJump;
    private OldPlayerDoubleJump playerDoubleJump;
    //private OldPlayerSwim playerSwim;
    //private OldPlayerSwimBoost playerSwimBoost;
    private OldPlayerInteract playerInteract;
    private OldCircleSlam circleSlam;

    void Awake()
    {
        playerMove = GetComponent<OldPlayerMove>();
        playerJump = GetComponent<OldPlayerJump>();
        playerDoubleJump = GetComponent<OldPlayerDoubleJump>();
        //playerSwim = GetComponent<OldPlayerSwim>();
        //playerSwimBoost = GetComponent<OldPlayerSwimBoost>();
        playerInteract = GetComponent<OldPlayerInteract>();
        circleSlam = GetComponent<OldCircleSlam>();
    }

    private void Update()
    {
        circleSlam.Slam();
        /*if (playerSwim.swimming)
        {
            playerSwimBoost.SwimBoost();
        }
        else
        {*/
            playerDoubleJump.DoubleJump();
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

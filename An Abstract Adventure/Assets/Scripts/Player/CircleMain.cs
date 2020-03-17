using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleMain : MonoBehaviour
{
    [HideInInspector] public PlayerMove playerMove;
    
    private PlayerJump playerJump;
    private PlayerDoubleJump playerDoubleJump;
    //private PlayerSwim playerSwim;
    //private PlayerSwimBoost playerSwimBoost;
    private PlayerInteract playerInteract;
    private CircleSlam circleSlam;

    void Awake()
    {
        playerMove = GetComponent<PlayerMove>();
        playerJump = GetComponent<PlayerJump>();
        playerDoubleJump = GetComponent<PlayerDoubleJump>();
        //playerSwim = GetComponent<PlayerSwim>();
        //playerSwimBoost = GetComponent<PlayerSwimBoost>();
        playerInteract = GetComponent<PlayerInteract>();
        circleSlam = GetComponent<CircleSlam>();
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

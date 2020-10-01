using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public KeyCode moveLeft;
    public KeyCode moveRight;
    public KeyCode jump;
    public KeyCode ability;

    [HideInInspector] public bool inputML;
    [HideInInspector] public bool inputMR;
    [HideInInspector] public bool inputJD;
    [HideInInspector] public bool inputJU;
    [HideInInspector] public bool inputAD;

    public void GetKeyInput()
    {
        inputML = Input.GetKey(moveLeft);
        inputMR = Input.GetKey(moveRight);
    }

    public void GetKeyDownInput()
    {
        inputJD = Input.GetKeyDown(jump);
        inputAD = Input.GetKeyDown(ability);
    }

    public void GetKeyUpInput()
    {
        inputJU = Input.GetKeyUp(jump);
    }
}

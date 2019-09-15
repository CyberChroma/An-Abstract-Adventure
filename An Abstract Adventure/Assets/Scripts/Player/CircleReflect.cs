using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleReflect : MonoBehaviour
{
    public float rotSmoothing;
    public GameObject shield;

    [HideInInspector] public bool shielding;
    [HideInInspector] public bool disableShielding;

    private float rotation;
    private PlayerMove playerMove;
    private PlayerJump playerJump;
    private PlayerDoubleJump playerDoubleJump;
    private PlayerLineUp playerLineUp;

    void Start()
    {
        shield.SetActive(false);
        playerMove = GetComponent<PlayerMove>();
        playerJump = GetComponent<PlayerJump>();
        playerDoubleJump = GetComponent<PlayerDoubleJump>();
        playerLineUp = GetComponent<PlayerLineUp>();
    }

    public void DisableShield()
    {
        shield.SetActive(false);
        shielding = false;
    }

    public void Reflect()
    {
        if (!disableShielding && Input.GetKey(KeyCode.E))
        {
            if (Input.GetKey(KeyCode.I))
            {
                rotation = 0;
            }
            else if (Input.GetKey(KeyCode.K))
            {
                rotation = 180;
            }
            if (Input.GetKey(KeyCode.L))
            {
                if (rotation == 0)
                {
                    rotation = 315;
                }
                else if (rotation == 180)
                {
                    rotation = 225;
                }
                else
                {
                    rotation = 270;
                }
            }
            else if (Input.GetKey(KeyCode.J))
            {
                if (rotation == 0)
                {
                    rotation = 45;
                }
                else if (rotation == 180)
                {
                    rotation = 135;
                }
                else
                {
                    rotation = 90;
                }
            }
            if (!shielding)
            {
                shield.SetActive(true);
                shielding = true;
                shield.transform.rotation = Quaternion.Euler(Vector3.forward * rotation);
                playerMove.disableMove = true;
                playerJump.disableJump = true;
                playerDoubleJump.disableDoubleJump = true;
                playerLineUp.disableAiming = true;
            }
            shield.transform.position = transform.position;
            shield.transform.rotation = Quaternion.Slerp(shield.transform.rotation, Quaternion.Euler(Vector3.forward * rotation), rotSmoothing * Time.deltaTime);
        }
        else if (shielding)
        {
            shield.SetActive(false);
            shielding = false;
            playerMove.disableMove = false;
            playerJump.disableJump = false;
            playerDoubleJump.disableDoubleJump = false;
            playerLineUp.disableAiming = false;
        }
    }
}

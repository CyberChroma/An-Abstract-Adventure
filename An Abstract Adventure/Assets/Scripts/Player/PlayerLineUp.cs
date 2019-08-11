using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLineUp : MonoBehaviour
{
    public float rotSmoothing;
    public GameObject arrow;

    [HideInInspector] private bool aiming;
    [HideInInspector] private bool released;

    private float rotation;
    private bool inputRecieved;

    // Start is called before the first frame update
    void Start()
    {
        arrow.SetActive(false);
    }

    public void DisableArrow()
    {
        arrow.SetActive(false);
        aiming = false;
        released = false;
    }

    public void LineUp ()
    {
        inputRecieved = false;
        rotation = 360;
        if (Input.GetKey(KeyCode.I))
        {
            inputRecieved = true;
            rotation = 0;
        }
        else if (Input.GetKey(KeyCode.K))
        {
            inputRecieved = true;
            rotation = 180;
        }
        if (Input.GetKey(KeyCode.L))
        {
            inputRecieved = true;
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
            inputRecieved = true;
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
        if (inputRecieved)
        {
            if (!aiming)
            {
                arrow.SetActive(true);
                aiming = true;
                released = false;
            }
            arrow.transform.position = transform.position;
            arrow.transform.rotation = Quaternion.Slerp(arrow.transform.rotation, Quaternion.Euler(Vector3.forward * rotation), rotSmoothing * Time.deltaTime);
        }
        else if (aiming)
        {
            aiming = false;
            released = true;
            arrow.SetActive(false);
        }
    }
}

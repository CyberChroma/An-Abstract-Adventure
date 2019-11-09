﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonOnce : MonoBehaviour
{
    public GameObject[] objectsToActivate;
    public GameObject unpressedObj;
    public GameObject pressedObj;

    [HideInInspector] public bool pressed;

    // Start is called before the first frame update
    void Start()
    {
        pressed = false;
        pressedObj.SetActive(true);
        pressedObj.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!pressed && collision.CompareTag("Player"))
        {
            foreach (GameObject objectToActivate in objectsToActivate)
            {
                if (objectToActivate.GetComponent<MovingObject>())
                {
                    objectToActivate.GetComponent<MovingObject>().enabled = true;
                }
                else if (objectToActivate.GetComponent<ButtonMulti>())
                {
                    objectToActivate.GetComponent<ButtonMulti>().Activate();
                }
            }
            unpressedObj.SetActive(false);
            pressedObj.SetActive(true);
            pressed = true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonWeighted : MonoBehaviour
{
    public GameObject[] objectsToActivate;
    public GameObject unpressedObj;
    public GameObject pressedObj;

    [HideInInspector] public bool pressed;

    // Start is called before the first frame update
    void Start()
    {
        pressed = false;
        unpressedObj.SetActive(true);
        pressedObj.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!pressed && collision.CompareTag("Player"))
        {
            foreach (GameObject objectToActivate in objectsToActivate)
            {
                MovingObject movingObject = objectToActivate.GetComponent<MovingObject>();
                if (movingObject)
                {
                    movingObject.reverse = false;
                    movingObject.enabled = true;
                }
            }
            unpressedObj.SetActive(false);
            pressedObj.SetActive(true);
            pressed = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (pressed && collision.CompareTag("Player"))
        {
            foreach (GameObject objectToActivate in objectsToActivate)
            {
                MovingObject movingObject = objectToActivate.GetComponent<MovingObject>();
                if (movingObject)
                {
                    movingObject.reverse = true;
                    if (movingObject.enabled)
                    {
                        movingObject.NextTarget();
                    }
                    else
                    {
                        movingObject.enabled = true;
                    }
                }
            }
            unpressedObj.SetActive(true);
            pressedObj.SetActive(false);
            pressed = false;
        }
    }
}

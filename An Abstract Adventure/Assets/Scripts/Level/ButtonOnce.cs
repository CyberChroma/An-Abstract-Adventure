using System.Collections;
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
        unpressedObj.SetActive(true);
        pressedObj.SetActive(false);
    }

    private void OnTriggerEnter(Collider collision)
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

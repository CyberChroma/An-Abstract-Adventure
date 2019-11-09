using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonMulti : MonoBehaviour
{
    public int buttonsNeeded;
    public GameObject[] objectsToActivate;

    public void Activate()
    {
        buttonsNeeded--;
        if (buttonsNeeded == 0)
        {
            foreach (GameObject objectToActivate in objectsToActivate)
            {
                if (objectToActivate.GetComponent<MovingObject>())
                {
                    objectToActivate.GetComponent<MovingObject>().enabled = true;
                }
            }
        }
    }
}

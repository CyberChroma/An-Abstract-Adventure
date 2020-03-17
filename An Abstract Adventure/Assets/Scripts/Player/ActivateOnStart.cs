using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateOnStart : MonoBehaviour
{
    public GameObject[] objectsToActivate;

    // Start is called before the first frame update
    void Awake()
    {
        foreach (GameObject activate in objectsToActivate)
        {
            activate.SetActive(true);
        }
    }
}

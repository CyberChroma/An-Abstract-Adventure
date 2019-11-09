using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTimed : MonoBehaviour
{
    public float activeTime;
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
            StartCoroutine(Activate());

        }
    }

    IEnumerator Activate()
    {
        unpressedObj.SetActive(false);
        pressedObj.SetActive(true);
        pressed = true;
        foreach (GameObject objectToActivate in objectsToActivate)
        {
            MovingObject movingObject = objectToActivate.GetComponent<MovingObject>();
            if (movingObject)
            {
                movingObject.reverse = false;
                movingObject.enabled = true;
            }
        }
        yield return new WaitForSeconds(activeTime);
        foreach (GameObject objectToActivate in objectsToActivate)
        {
            MovingObject movingObject = objectToActivate.GetComponent<MovingObject>();
            if (movingObject)
            {
                movingObject.reverse = true;
                movingObject.enabled = true;
            }
        }
        unpressedObj.SetActive(true);
        pressedObj.SetActive(false);
        pressed = false;
    }
}

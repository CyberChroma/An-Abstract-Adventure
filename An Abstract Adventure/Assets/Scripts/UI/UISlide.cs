using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISlide : MonoBehaviour
{
    public float activeTime;
    public float enterSmoothing;
    public float exitSmoothing;
    public Transform offScreenPos;

    [HideInInspector] public bool active;
    private Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        active = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            transform.position = Vector3.Lerp(transform.position, startPos, enterSmoothing);
            if (Vector3.Distance(transform.position, startPos) <= 1)
            {
                StartCoroutine(WaitToDeactivate());
            }
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, offScreenPos.position, exitSmoothing);
        }
    }

    IEnumerator WaitToDeactivate()
    {
        yield return new WaitForSeconds(activeTime);
        active = false;
    }
}

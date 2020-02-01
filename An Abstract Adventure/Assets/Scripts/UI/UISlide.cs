using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISlide : MonoBehaviour
{
    public float activeTime;
    public float enterSmoothing;
    public float exitSmoothing;
    public Transform offScreenPos;
    public bool stayActive;

    [HideInInspector] public bool active;
    private Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.localPosition;
        active = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (stayActive || active)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, startPos, enterSmoothing);
            if (!stayActive && Vector3.Distance(transform.localPosition, startPos) <= 1)
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

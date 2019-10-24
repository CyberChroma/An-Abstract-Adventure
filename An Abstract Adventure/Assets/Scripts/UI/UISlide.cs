using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISlide : MonoBehaviour
{
    public float activeTime;
    public float enterSmoothing;
    public float exitSmoothing;
    public Vector2 dir;

    [HideInInspector] public bool active;
    private Vector3 startPos;
    private Vector3 offPos;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        offPos = new Vector3(transform.position.x + 200 * dir.x, transform.position.y + 100 * dir.y, 0);
        active = false;
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
            transform.position = Vector3.Lerp(transform.position, offPos, exitSmoothing);
        }
    }

    IEnumerator WaitToDeactivate()
    {
        yield return new WaitForSeconds(activeTime);
        active = false;
    }
}

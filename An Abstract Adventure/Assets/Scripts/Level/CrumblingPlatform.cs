using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrumblingPlatform : MonoBehaviour
{
    public float shakeTime;

    private BoxCollider coll;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponentInChildren<BoxCollider>();
        anim = GetComponentInChildren<Animator>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        StartCoroutine(Crumble());
    }

    IEnumerator Crumble ()
    {
        anim.SetTrigger("Shake");
        yield return new WaitForSeconds(shakeTime);
        coll.enabled = false;
        anim.SetTrigger("Fall");
        yield return new WaitForSeconds(3);
        gameObject.SetActive(false);
    }
}

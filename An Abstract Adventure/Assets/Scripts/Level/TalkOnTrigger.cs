using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkOnTrigger : MonoBehaviour
{
    public GameObject playerToTrigger;
    public GrowAndShrink[] dialogue;
    public float talkDelay;
    public float[] talkTime;
    public bool oneTime;

    private bool activated;

    // Start is called before the first frame update
    void Start()
    {
        foreach (GrowAndShrink bubble in dialogue)
        {
            bubble.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (!activated && (playerToTrigger && collision.gameObject == playerToTrigger) || (!playerToTrigger && collision.CompareTag("Player")))
        {
            activated = true;
            foreach (GrowAndShrink bubble in dialogue)
            {
                bubble.gameObject.SetActive(false);
            }
            StopAllCoroutines();
            StartCoroutine(WaitToTalk());
        }
    }

    IEnumerator WaitToTalk ()
    {
        yield return new WaitForSeconds(talkDelay);
        for (int i = 0; i < dialogue.Length; i++)
        {
            dialogue[i].transform.localScale = Vector3.zero;
            dialogue[i].growing = true;
            dialogue[i].gameObject.SetActive(true);
            yield return new WaitForSeconds(talkTime[i]);
            dialogue[i].growing = false;
        }
        if (oneTime)
        {
            gameObject.SetActive(false);
        }
        else
        {
            activated = false;
        }
    }
}

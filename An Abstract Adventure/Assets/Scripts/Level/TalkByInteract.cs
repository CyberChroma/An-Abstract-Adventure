using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkByInteract : MonoBehaviour
{
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

    public void Activate()
    {
        if (!activated)
        {
            activated = true;
            gameObject.layer = 0;
            foreach (GrowAndShrink bubble in dialogue)
            {
                bubble.gameObject.SetActive(false);
            }
            StopAllCoroutines();
            StartCoroutine(WaitToTalk());
        }

    }

    IEnumerator WaitToTalk()
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
        gameObject.layer = 12;
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

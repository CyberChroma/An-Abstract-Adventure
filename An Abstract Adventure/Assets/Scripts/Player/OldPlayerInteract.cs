using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldPlayerInteract : MonoBehaviour
{
    public GrowAndShrink interactArrow;

    [HideInInspector] public bool canInteract;

    private void Start()
    {
        interactArrow.growing = false;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (canInteract && collision.gameObject.layer == 12)
        {
            interactArrow.transform.position = collision.transform.position;
            interactArrow.growing = true;
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (canInteract && collision.gameObject.layer == 12)
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                if (collision.CompareTag("Fade Move"))
                {
                    collision.GetComponent<FadeMove>().MovePlayer(GetComponent<OldPlayerMain>());
                    interactArrow.growing = false;
                }
                else if (collision.CompareTag("Talk"))
                {
                    collision.GetComponent<TalkByInteract>().Activate();
                    interactArrow.growing = false;
                }
            }
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.layer == 12)
        {
            interactArrow.growing = false;
        }
    }
}

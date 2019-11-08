using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [HideInInspector] public bool canInteract;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (canInteract && collision.gameObject.layer == 12 && Input.GetKeyDown(KeyCode.O))
        {
            if (collision.CompareTag("Test"))
            {
                print("Interact Test!");
            } else if (collision.CompareTag("Fade Move")) {
                collision.GetComponent<FadeMove>().MovePlayer(GetComponent<PlayerMain>());
            }
        }
    }
}

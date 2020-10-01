using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldPlayerHit : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Destructable"))
        {
            Destroy(collision.gameObject);
        }
    }
}

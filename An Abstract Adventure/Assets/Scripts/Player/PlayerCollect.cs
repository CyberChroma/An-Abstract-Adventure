using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollect : MonoBehaviour
{
    public int numCollectables;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(gameObject.name + " Collectable"))
        {
            collision.gameObject.SetActive(false);
            numCollectables++;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollect : MonoBehaviour
{
    [HideInInspector] public int numCollectables;
    [HideInInspector] public bool collected;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(gameObject.name + " Collectable"))
        {
            collision.gameObject.SetActive(false);
            numCollectables++;
            collected = true;
        }
    }
}

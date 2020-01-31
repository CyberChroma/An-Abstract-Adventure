using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollect : MonoBehaviour
{
    [HideInInspector] public int numCollectables;
    [HideInInspector] public bool collected;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag(gameObject.name + " Collectable"))
        {
            collision.gameObject.SetActive(false);
            numCollectables++;
            collected = true;
        }
    }
}

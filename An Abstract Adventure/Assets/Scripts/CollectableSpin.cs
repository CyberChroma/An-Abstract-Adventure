using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableSpin : MonoBehaviour
{
    public float[] spriteSpinSpeeds;
    public Transform[] sprites;

    private Vector3 startingPos;
    private float timeOffset;

    // Start is called before the first frame update
    void Start()
    {
        startingPos = sprites[0].position;
        timeOffset = Random.Range(-5f, 5f);
    }

    // Update is called once per frame
    void Update()
    {
         for(int i = 0; i < sprites.Length; i++) {
            sprites[i].Rotate(Vector3.forward, spriteSpinSpeeds[i] * -10 * Time.deltaTime);
        }
        sprites[0].position = startingPos + Vector3.up * Mathf.Cos(Time.time + timeOffset) / 2;
    }
}

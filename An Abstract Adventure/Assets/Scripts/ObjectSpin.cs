using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpin : MonoBehaviour
{
    public float[] spriteSpinSpeeds;
    public Transform[] sprites;

    // Update is called once per frame
    void Update()
    {
         for(int i = 0; i < sprites.Length; i++) {
            sprites[i].Rotate(Vector3.forward, spriteSpinSpeeds[i] * -10 * Time.deltaTime);
        }
    }
}

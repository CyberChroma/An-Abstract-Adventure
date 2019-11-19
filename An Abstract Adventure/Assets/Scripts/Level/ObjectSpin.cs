using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpin : MonoBehaviour
{
    public float[] spriteSpinSpeeds;
    public Transform[] sprites;
    public bool timeUnscaled;

    private void Start()
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            sprites[i].rotation = Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f));
        }
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < sprites.Length; i++) {
            if (timeUnscaled)
            {
                sprites[i].Rotate(Vector3.forward, spriteSpinSpeeds[i] * -10 * 0.02f);
            }
            else
            {
                sprites[i].Rotate(Vector3.forward, spriteSpinSpeeds[i] * -10 * Time.deltaTime);
            }
        }
    }
}

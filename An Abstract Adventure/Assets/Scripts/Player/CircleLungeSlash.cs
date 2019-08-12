using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleLungeSlash : MonoBehaviour
{
    public float lungeTime;
    public float lungePower;

    private PlayerLineUp playerLineUp;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Awake()
    {
        playerLineUp = GetComponent<PlayerLineUp>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LungeSlash()
    {
        if(playerLineUp.released)
        {
            //rb.AddForce( )
        }
    }
}

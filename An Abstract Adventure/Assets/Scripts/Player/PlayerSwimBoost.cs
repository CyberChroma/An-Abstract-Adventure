using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwimBoost : MonoBehaviour
{
    public float boostSpeed;
    public float boostDelay;

    [HideInInspector] public bool swimming;

    private Rigidbody2D rb;
    private PlayerSwim playerSwim;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerSwim = GetComponent<PlayerSwim>();
    }

    public void SwimBoost()
    {

    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetherCameraFollow : MonoBehaviour
{
    public float smoothing;
    public float velocityDisX;
    public float velocityDisY;
    public float cameraDis;
    public float offsetHeight;
    public Vector2 maxVelocity;
    public Rigidbody player;

    private Vector3 movePos;
    private Vector3 camVelocity;

    void FixedUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(player.position.x + Mathf.Min(Mathf.Abs(player.velocity.x), maxVelocity.x) * player.velocity.normalized.x * velocityDisX, player.position.y + Mathf.Min(Mathf.Abs(player.velocity.y), maxVelocity.y) * player.velocity.normalized.y * velocityDisY + offsetHeight, player.position.z + -cameraDis), ref camVelocity, smoothing, Mathf.Infinity, Time.deltaTime);
    }
}

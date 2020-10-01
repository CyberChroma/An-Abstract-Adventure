using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmCameraFollow : MonoBehaviour
{
    public float smoothing;
    public float velocityDisX;
    public float velocityDisY;
    public float offsetHeight;
    public Rigidbody players;

    private Vector3 movePos;
    private Vector3 camVelocity;

    void FixedUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(players.position.x + Mathf.Abs(players.velocity.x) * players.velocity.normalized.x * velocityDisX, players.position.y + Mathf.Abs(players.velocity.y) * players.velocity.normalized.y * velocityDisY + offsetHeight, transform.position.z), ref camVelocity, smoothing, Mathf.Infinity, Time.deltaTime);
    }
}

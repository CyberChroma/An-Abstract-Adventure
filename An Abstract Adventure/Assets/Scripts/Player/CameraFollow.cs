using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerInfo
{
    public float smoothing;
    public float height;
}

[System.Serializable]
public class LookAtInfo
{
    public float smoothing;
    public Transform target;
}

public class CameraFollow : MonoBehaviour
{
    public enum Mode
    {
        Player,
        LookAt
    }

    public Mode mode;
    public PlayerInfo player;
    public LookAtInfo lookAt;

    private Vector3 movePos;
    private Transform playerObj;

    void Start()
    {
        playerObj = GameObject.Find("Kall").transform;
    }

    void LateUpdate()
    {
        if (mode == Mode.Player)
        {
            movePos = Vector3.Lerp(transform.position, new Vector3(playerObj.position.x, playerObj.position.y + player.height, -10), player.smoothing * Time.deltaTime);
        }
        else
        {
            movePos = Vector3.Lerp(transform.position, new Vector3(lookAt.target.position.x, lookAt.target.position.y, -10), lookAt.smoothing * Time.deltaTime);
        }
        transform.position = movePos;
    }
}

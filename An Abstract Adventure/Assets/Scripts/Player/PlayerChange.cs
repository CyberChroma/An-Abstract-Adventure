using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChange : MonoBehaviour
{
    public enum ActivePlayer
    {
        Kall,
        Que
    }

    public ActivePlayer activePlayer;

    private CameraFollow cameraFollow;
    private Transform kall;
    private Transform que;

    // Start is called before the first frame update
    void Awake()
    {
        cameraFollow = GetComponent<CameraFollow>();
        kall = GameObject.Find("Kall").transform;
        que = GameObject.Find("Que").transform;
        if (activePlayer == ActivePlayer.Kall)
        {
            kall.GetComponent<CircleMain>().activePlayer = true;
            que.GetComponent<CircleMain>().activePlayer = false;
            cameraFollow.player = kall;
        }
        else
        {
            kall.GetComponent<CircleMain>().activePlayer = false;
            que.GetComponent<CircleMain>().activePlayer = true;
            cameraFollow.player = que;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (activePlayer == ActivePlayer.Kall)
            {
                activePlayer = ActivePlayer.Que;
                kall.GetComponent<CircleMain>().activePlayer = false;
                que.GetComponent<CircleMain>().activePlayer = true;
                cameraFollow.player = que;
            }
            else
            {
                activePlayer = ActivePlayer.Kall;
                que.GetComponent<CircleMain>().activePlayer = false;
                kall.GetComponent<CircleMain>().activePlayer = true;
                cameraFollow.player = kall;
            }
        }
    }
}

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
    private CircleMain kall;
    private SquareMain que;

    // Start is called before the first frame update
    void Awake()
    {
        cameraFollow = GetComponent<CameraFollow>();
        kall = FindObjectOfType<CircleMain>();
        que = FindObjectOfType<SquareMain>();
        if (activePlayer == ActivePlayer.Kall)
        {
            kall.activePlayer = true;
            kall.playerMove.active = true;
            que.activePlayer = false;
            que.playerMove.active = false;
            cameraFollow.player = kall.transform;
        }
        else
        {
            kall.activePlayer = false;
            kall.playerMove.active = false;
            que.activePlayer = true;
            que.playerMove.active = true;
            cameraFollow.player = que.transform;
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
                kall.activePlayer = false;
                que.activePlayer = true;
                cameraFollow.player = que.transform;
            }
            else
            {
                activePlayer = ActivePlayer.Kall;
                que.activePlayer = false;
                kall.activePlayer = true;
                cameraFollow.player = kall.transform;
            }
        }
    }
}

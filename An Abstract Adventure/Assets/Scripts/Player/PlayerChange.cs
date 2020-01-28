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
    }

    void Start()
    {
        if (activePlayer == ActivePlayer.Kall)
        {
            kall.activePlayer = true;
            que.activePlayer = false;
            cameraFollow.player = kall.GetComponent<Rigidbody2D>();
        }
        else
        {
            kall.activePlayer = false;
            que.activePlayer = true;
            cameraFollow.player = que.GetComponent<Rigidbody2D>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (activePlayer == ActivePlayer.Kall)
            {
                activePlayer = ActivePlayer.Que;
                kall.playerLineUp.DisableArrow();
                kall.activePlayer = false;
                que.activePlayer = true;
                cameraFollow.player = que.GetComponent<Rigidbody2D>();
            }
            else
            {
                activePlayer = ActivePlayer.Kall;
                que.playerLineUp.DisableArrow();
                que.activePlayer = false;
                kall.activePlayer = true;
                cameraFollow.player = kall.GetComponent<Rigidbody2D>();
            }
        }
    }
}

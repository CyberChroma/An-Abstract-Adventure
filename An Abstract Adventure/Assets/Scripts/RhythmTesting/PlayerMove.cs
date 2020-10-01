using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed;
    public float moveSmoothness;
    public float rotSmoothing;

    [HideInInspector] public int frontDir;
    [HideInInspector] public bool moveOverride;
    [HideInInspector] public float moveDir;

    private PlayerMain playerMain;

    // Start is called before the first frame update
    void Start()
    {
        frontDir = 1;
        playerMain = GetComponent<PlayerMain>();
    }

    public float Move()
    {
        if (!moveOverride)
        {
            if (playerMain.playerInput.inputMR)
            {
                if (frontDir == -1)
                {
                    frontDir = 1;
                    moveDir = Mathf.Lerp(moveDir, 1, moveSmoothness * Time.deltaTime);
                }
                moveDir = Mathf.Lerp(moveDir, 1, moveSmoothness * Time.deltaTime);
            }
            else if (playerMain.playerInput.inputML)
            {
                if (frontDir == 1)
                {
                    frontDir = -1;
                    moveDir = Mathf.Lerp(moveDir, -1, moveSmoothness * Time.deltaTime);
                }
                moveDir = Mathf.Lerp(moveDir, -1, moveSmoothness * Time.deltaTime);
            }
            else if (moveDir != 0)
            {
                moveDir = Mathf.Lerp(moveDir, 0, moveSmoothness * Time.deltaTime);
            }
        }
        float moveVel = moveDir * speed * 10 * Time.deltaTime;
        return moveVel;
    }

    public void Turn()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(new Vector3(0, 270 + frontDir * 90, 0)), rotSmoothing * Time.deltaTime);
    }

    public IEnumerator InputOveride(float delay, float setMoveDir)
    {
        moveOverride = true;
        moveDir = setMoveDir;
        yield return new WaitForSeconds(delay);
        moveOverride = false;
    }
}

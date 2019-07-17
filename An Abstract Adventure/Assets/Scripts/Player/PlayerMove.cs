using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed;
    public float moveSmoothness;
    public float stopSmoothness;

    private bool frontRight;
    private Vector3 moveDir = Vector3.zero;
    private PlayerGroundCheck playerGroundCheck;

    void Start()
    {
        frontRight = true;
        playerGroundCheck = GetComponentInChildren<PlayerGroundCheck>();
    }

    public void Move ()
    {
        if (Input.GetKey(KeyCode.D))
        {
            if (!frontRight)
            {
                frontRight = true;
                if (playerGroundCheck.isGrounded)
                {
                    moveDir = Vector3.Lerp(moveDir, transform.right, moveSmoothness * Time.deltaTime);
                } else
                {
                    moveDir = Vector3.zero;
                }
            }
            moveDir = Vector3.Lerp(moveDir, transform.right, moveSmoothness * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            if (frontRight)
            {
                frontRight = false;
                if (playerGroundCheck.isGrounded)
                {
                    moveDir = Vector3.Lerp(moveDir, -transform.right, moveSmoothness * Time.deltaTime);
                }
                else
                {
                    moveDir = Vector3.zero;
                }
            }
            moveDir = Vector3.Lerp(moveDir, -transform.right, moveSmoothness * Time.deltaTime);
        }
        else
        {
            moveDir = Vector3.Lerp(moveDir, Vector3.zero, stopSmoothness * Time.deltaTime);
        }
        transform.position += moveDir * speed * Time.deltaTime;
    }
}

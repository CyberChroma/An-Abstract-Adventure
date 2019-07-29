using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed;
    public float moveSmoothness;
    public float stopSmoothness;

    [HideInInspector] public bool active;
    [HideInInspector] public bool frontRight;

    private Vector3 moveDir = Vector3.zero;
    private PlayerGroundCheck playerGroundCheck;
    private GameObject attackCollider;

    void Awake()
    {
        frontRight = true;
        playerGroundCheck = GetComponentInChildren<PlayerGroundCheck>();
        attackCollider = transform.Find("Attack Hit Box").gameObject;
    }

    private void Update()
    {
        if (!active && moveDir != Vector3.zero)
        {
            moveDir = Vector3.Lerp(moveDir, Vector3.zero, stopSmoothness * Time.deltaTime);
            transform.position += moveDir * speed * Time.deltaTime;
        }
    }

    public void Move ()
    {
        if (Input.GetKey(KeyCode.D))
        {
            if (!attackCollider.activeSelf && !frontRight)
            {
                transform.localScale = new Vector3(1, 1, 1);
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
            if (!attackCollider.activeSelf && frontRight)
            {
                transform.localScale = new Vector3(-1, 1, 1);
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
        else if (moveDir != Vector3.zero)
        {
            moveDir = Vector3.Lerp(moveDir, Vector3.zero, stopSmoothness * Time.deltaTime);
        }
        transform.position += moveDir * speed * Time.deltaTime;
    }
}

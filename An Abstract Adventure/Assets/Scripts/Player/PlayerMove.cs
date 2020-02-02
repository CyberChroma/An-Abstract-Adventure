using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed;
    public float moveSmoothness;
    public float iceSmoothness;
    public float rotSmoothing;
    public GameObject attackCollider;

    [HideInInspector] public bool active;
    [HideInInspector] public int frontDir;
    [HideInInspector] public Vector3 moveDir = Vector3.zero;
    [HideInInspector] public bool moveOverride;
    [HideInInspector] public bool noDrag;
    [HideInInspector] public bool disableMove;

    private bool slide;
    private Rigidbody rb;
    private Animator anim;
    private PlayerGroundCheck playerGroundCheck;
    private MovingObject movingObject;

    void Awake()
    {
        frontDir = 1;
        slide = false;
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        playerGroundCheck = GetComponentInChildren<PlayerGroundCheck>();
    }

    private void FixedUpdate()
    {
        if (!noDrag)
        {
            rb.velocity = new Vector3(rb.velocity.x * 0.5f, rb.velocity.y, 0);
        }
        if (movingObject)
        {
            if (movingObject.velocity.y > 0)
            {
                rb.AddForce(new Vector3(movingObject.velocity.x, 0, 0) / 2, ForceMode.Impulse);
            }
            else
            {
                rb.AddForce(movingObject.velocity / 2, ForceMode.Impulse);
            }
        }
        if ((!active || disableMove) && moveDir != Vector3.zero)
        {
            if (slide)
            {
                moveDir = Vector3.Lerp(moveDir, Vector3.zero, iceSmoothness * Time.deltaTime);
            }
            else
            {
                moveDir = Vector3.Lerp(moveDir, Vector3.zero, moveSmoothness * Time.deltaTime);
            }
            rb.AddForce (moveDir * speed  * 10 * Time.deltaTime, ForceMode.Impulse);
        }
    }

    public void Move ()
    {
        if (active && !disableMove)
        {
            if (!moveOverride)
            {
                if (Input.GetKey(KeyCode.D))
                {
                    if (anim)
                    {
                        anim.SetBool("IsRunning", true);
                    }
                    if (!attackCollider.activeSelf && frontDir == -1)
                    {
                        frontDir *= -1;
                        if (playerGroundCheck.isGrounded)
                        {
                            if (slide)
                            {
                                moveDir = Vector3.Lerp(moveDir, Vector3.right, iceSmoothness * Time.deltaTime);
                            }
                            else
                            {
                                moveDir = Vector3.Lerp(moveDir, Vector3.right, moveSmoothness * Time.deltaTime);
                            }
                        }
                        else
                        {
                            moveDir = Vector3.zero;
                        }
                    }
                    if (slide)
                    {
                        moveDir = Vector3.Lerp(moveDir, Vector3.right, iceSmoothness * Time.deltaTime);
                    }
                    else
                    {
                        moveDir = Vector3.Lerp(moveDir, Vector3.right, moveSmoothness * Time.deltaTime);
                    }
                }
                else if (Input.GetKey(KeyCode.A))
                {
                    if (anim)
                    {
                        anim.SetBool("IsRunning", true);
                    }
                    if (!attackCollider.activeSelf && frontDir == 1)
                    {
                        frontDir *= -1;
                        if (playerGroundCheck.isGrounded)
                        {
                            if (slide)
                            {
                                moveDir = Vector3.Lerp(moveDir, -Vector3.right, iceSmoothness * Time.deltaTime);
                            }
                            else
                            {
                                moveDir = Vector3.Lerp(moveDir, -Vector3.right, moveSmoothness * Time.deltaTime);
                            }
                        }
                        else
                        {
                            moveDir = Vector3.zero;
                        }
                    }
                    if (slide)
                    {
                        moveDir = Vector3.Lerp(moveDir, -Vector3.right, iceSmoothness * Time.deltaTime);
                    }
                    else
                    {
                        moveDir = Vector3.Lerp(moveDir, -Vector3.right, moveSmoothness * Time.deltaTime);
                    }
                }
                else if (moveDir != Vector3.zero)
                {
                    if (anim)
                    {
                        anim.SetBool("IsRunning", false);
                    }
                    if (slide)
                    {
                        moveDir = Vector3.Lerp(moveDir, Vector3.zero, iceSmoothness * Time.deltaTime);
                    }
                    else
                    {
                        moveDir = Vector3.Lerp(moveDir, Vector3.zero, moveSmoothness * Time.deltaTime);
                    }
                }
            }
            rb.AddForce(moveDir * speed * 10 * Time.deltaTime, ForceMode.Impulse);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(new Vector3(0, 270 + frontDir * 90, 0)), rotSmoothing * Time.deltaTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Moving"))
        {
            movingObject = collision.collider.GetComponent<MovingObject>();
        }
        else if (collision.collider.CompareTag("Slippery"))
        {
            slide = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Moving"))
        {
            movingObject = null;
        }
        else if (collision.collider.CompareTag("Slippery"))
        {
            slide = false;
        }
    }
}

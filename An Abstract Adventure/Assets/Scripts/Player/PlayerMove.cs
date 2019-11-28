using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed;
    public float moveSmoothness;
    public float stopSmoothness;
    public float airStopSmoothness;
    public float iceSmoothness;

    [HideInInspector] public bool active;
    [HideInInspector] public bool frontRight;
    [HideInInspector] public Vector2 moveDir = Vector2.zero;
    [HideInInspector] public bool moveOverride;
    [HideInInspector] public bool noDrag;
    [HideInInspector] public bool disableMove;

    private bool slide;
    private Rigidbody2D rb;
    private Animator anim;
    private PlayerGroundCheck playerGroundCheck;
    private GameObject attackCollider;
    private MovingObject movingObject;

    void Awake()
    {
        frontRight = true;
        slide = false;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        playerGroundCheck = GetComponentInChildren<PlayerGroundCheck>();
        attackCollider = transform.Find("Attack Hit Box").gameObject;
    }

    private void FixedUpdate()
    {
        if (!noDrag)
        {
            rb.velocity = new Vector2(rb.velocity.x * 0.5f, rb.velocity.y);
        }
        if (movingObject)
        {
            if (movingObject.velocity.y > 0)
            {
                rb.AddForce(new Vector2(movingObject.velocity.x, 0) / 2, ForceMode2D.Impulse);
            }
            else
            {
                rb.AddForce(movingObject.velocity / 2, ForceMode2D.Impulse);
            }
        }
        if ((!active || disableMove) && moveDir != Vector2.zero)
        {
            if (playerGroundCheck.isGrounded)
            {
                if (slide)
                {
                    moveDir = Vector2.Lerp(moveDir, Vector2.zero, iceSmoothness * Time.deltaTime);
                }
                else
                {
                    moveDir = Vector2.Lerp(moveDir, Vector2.zero, stopSmoothness * Time.deltaTime);
                }
            }
            else
            {
                moveDir = Vector2.Lerp(moveDir, Vector2.zero, airStopSmoothness * Time.deltaTime);
            }
            rb.AddForce (moveDir * speed  * 10 * Time.deltaTime, ForceMode2D.Impulse);
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
                    if (!attackCollider.activeSelf && !frontRight)
                    {
                        Flip();
                        if (playerGroundCheck.isGrounded)
                        {
                            if (slide)
                            {
                                moveDir = Vector2.Lerp(moveDir, transform.right, iceSmoothness * Time.deltaTime);
                            }
                            else
                            {
                                moveDir = Vector2.Lerp(moveDir, transform.right, moveSmoothness * Time.deltaTime);
                            }
                        }
                        else
                        {
                            moveDir = Vector2.zero;
                        }
                    }
                    if (slide)
                    {
                        moveDir = Vector2.Lerp(moveDir, transform.right, iceSmoothness * Time.deltaTime);
                    }
                    else
                    {
                        moveDir = Vector2.Lerp(moveDir, transform.right, moveSmoothness * Time.deltaTime);
                    }
                }
                else if (Input.GetKey(KeyCode.A))
                {
                    if (anim)
                    {
                        anim.SetBool("IsRunning", true);
                    }
                    if (!attackCollider.activeSelf && frontRight)
                    {
                        Flip();
                        if (playerGroundCheck.isGrounded)
                        {
                            if (slide)
                            {
                                moveDir = Vector2.Lerp(moveDir, -transform.right, iceSmoothness * Time.deltaTime);
                            }
                            else
                            {
                                moveDir = Vector2.Lerp(moveDir, -transform.right, moveSmoothness * Time.deltaTime);
                            }
                        }
                        else
                        {
                            moveDir = Vector2.zero;
                        }
                    }
                    if (slide)
                    {
                        moveDir = Vector2.Lerp(moveDir, -transform.right, iceSmoothness * Time.deltaTime);
                    }
                    else
                    {
                        moveDir = Vector2.Lerp(moveDir, -transform.right, moveSmoothness * Time.deltaTime);
                    }
                }
                else if (moveDir != Vector2.zero)
                {
                    if (anim)
                    {
                        anim.SetBool("IsRunning", false);
                    }
                    if (playerGroundCheck.isGrounded)
                    {
                        if (slide)
                        {
                            moveDir = Vector2.Lerp(moveDir, Vector2.zero, iceSmoothness * Time.deltaTime);
                        }
                        else
                        {
                            moveDir = Vector2.Lerp(moveDir, Vector2.zero, moveSmoothness * Time.deltaTime);
                        }
                    }
                    else
                    {
                        moveDir = Vector2.Lerp(moveDir, Vector2.zero, airStopSmoothness * Time.deltaTime);
                    }
                }
            }
            rb.AddForce(moveDir * speed * 10 * Time.deltaTime, ForceMode2D.Impulse);
        }
    }

    public void Flip ()
    {
        transform.localScale = new Vector2(transform.localScale.x * -1, 1);
        frontRight = !frontRight;
    }

    private void OnCollisionEnter2D(Collision2D collision)
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

    private void OnCollisionExit2D(Collision2D collision)
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

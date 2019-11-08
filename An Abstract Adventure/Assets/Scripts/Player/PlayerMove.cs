using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed;
    public float moveSmoothness;
    public float stopSmoothness;
    public float airStopSmoothness;

    [HideInInspector] public bool active;
    [HideInInspector] public bool frontRight;
    [HideInInspector] public Vector2 moveDir = Vector2.zero;
    [HideInInspector] public bool moveOverride;
    [HideInInspector] public bool noDrag;
    [HideInInspector] public bool disableMove;

    private Rigidbody2D rb;
    private PlayerGroundCheck playerGroundCheck;
    private GameObject attackCollider;
    private MovingObject movingObject;

    void Awake()
    {
        frontRight = true;
        rb = GetComponent<Rigidbody2D>();
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
                moveDir = Vector2.Lerp(moveDir, Vector2.zero, stopSmoothness * Time.deltaTime);
            } else
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
                    if (!attackCollider.activeSelf && !frontRight)
                    {
                        transform.localScale = new Vector2(1, 1);
                        frontRight = true;
                        if (playerGroundCheck.isGrounded)
                        {
                            moveDir = Vector2.Lerp(moveDir, transform.right, moveSmoothness * Time.deltaTime);
                        }
                        else
                        {
                            moveDir = Vector2.zero;
                        }
                    }
                    moveDir = Vector2.Lerp(moveDir, transform.right, moveSmoothness * Time.deltaTime);
                }
                else if (Input.GetKey(KeyCode.A))
                {
                    if (!attackCollider.activeSelf && frontRight)
                    {
                        transform.localScale = new Vector2(-1, 1);
                        frontRight = false;
                        if (playerGroundCheck.isGrounded)
                        {
                            moveDir = Vector2.Lerp(moveDir, -transform.right, moveSmoothness * Time.deltaTime);
                        }
                        else
                        {
                            moveDir = Vector2.zero;
                        }
                    }
                    moveDir = Vector2.Lerp(moveDir, -transform.right, moveSmoothness * Time.deltaTime);
                }
                else if (moveDir != Vector2.zero)
                {
                    if (playerGroundCheck.isGrounded)
                    {
                        moveDir = Vector2.Lerp(moveDir, Vector2.zero, stopSmoothness * Time.deltaTime);
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Moving"))
        {
            movingObject = collision.collider.GetComponent<MovingObject>();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Moving"))
        {
            movingObject = null;
        }
    }
}

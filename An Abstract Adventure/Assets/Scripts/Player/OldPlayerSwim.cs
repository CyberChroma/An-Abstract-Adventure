using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldPlayerSwim : MonoBehaviour
{
/*    public float speed;
    public float rotSmoothing;

    [HideInInspector] public bool swimming;
    [HideInInspector] public Vector2 swimDir;

    private bool inputRecieved;
    private float dirX;
    private float dirY;
    private Rigidbody2D rb;
    private PlayerMove playerMove;
    private PlayerJump playerJump;
    private PlayerDoubleJump playerDoubleJump;
    private CircleMain circleMain;
    private SquareMain squareMain;

    void Awake ()
    {
        rb = GetComponent<Rigidbody2D>();
        playerMove = GetComponent<PlayerMove>();
        playerJump = GetComponent<PlayerJump>();
        playerDoubleJump = GetComponent<PlayerDoubleJump>();
        circleMain = GetComponent<CircleMain>();
        squareMain = GetComponent<SquareMain>();
    }

    public void Swim()
    {
        rb.velocity *= 0.9f;
        inputRecieved = false;
        if (Input.GetKey(KeyCode.W))
        {
            inputRecieved = true;
            dirY = 1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            inputRecieved = true;
            dirY = -1;
        }
        else
        {
            dirY = 0;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputRecieved = true;
            dirX = 1;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            inputRecieved = true;
            dirX = -1;
        }
        else
        {
            dirX = 0;
        }
        if (inputRecieved)
        {
            rb.gravityScale = 0;
            swimDir = new Vector2(dirX, dirY).normalized;
            rb.AddForce(swimDir * speed * 10 * Time.deltaTime, ForceMode2D.Impulse);
        }
        else
        {
            rb.gravityScale = 0.5f;
        }
        rb.rotation = Mathf.LerpAngle(rb.rotation, Vector2.SignedAngle(Vector2.right, swimDir), rotSmoothing * Time.deltaTime);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!swimming && collision.CompareTag("Water"))
        {
            swimming = true;
            playerMove.moveDir = Vector2.zero;
            playerMove.enabled = false;
            playerJump.enabled = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (swimming && collision.CompareTag("Water"))
        {
            swimming = false;
            rb.gravityScale = 1;
            playerMove.enabled = true;
            playerJump.enabled = true;
            playerDoubleJump.canDoubleJump = true;
            if (circleMain)
            {
                circleMain.enabled = false;
                circleMain.enabled = true;
            }
            if (squareMain)
            {
                squareMain.enabled = false;
                squareMain.enabled = true;
            }
            rb.rotation = 0;
        }
    }*/
}

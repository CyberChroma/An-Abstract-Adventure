using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwim : MonoBehaviour
{
    public float speed;

    [HideInInspector] public bool swimming;
    [HideInInspector] public Vector2 swimDir;

    private bool inputRecieved;
    private float dirX;
    private float dirY;
    private Rigidbody2D rb;
    private PlayerMove playerMove;
    private PlayerJump playerJump;

    void Awake ()
    {
        rb = GetComponent<Rigidbody2D>();
        playerMove = GetComponent<PlayerMove>();
        playerJump = GetComponent<PlayerJump>();
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
            print(swimDir);
            rb.AddForce(swimDir * speed * 10 * Time.deltaTime, ForceMode2D.Impulse);
        } else
        {
            rb.gravityScale = 0.5f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Water"))
        {
            swimming = true;
            playerMove.enabled = false;
            playerJump.enabled = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Water"))
        {
            swimming = false;
            rb.gravityScale = 1;
            playerMove.enabled = true;
            playerJump.enabled = true;
        }
    }
}

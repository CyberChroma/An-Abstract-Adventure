using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleSlam : MonoBehaviour
{
    public float airPauseTime;
    public float slamSpeed;

    private bool slaming;
    private CircleMain circleMain;
    private PlayerMove playerMove;
    private PlayerGroundCheck playerGroundCheck;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        slaming = false;
        rb = GetComponent<Rigidbody>();
        circleMain = GetComponent<CircleMain>();
        playerMove = GetComponent<PlayerMove>();
        playerGroundCheck = GetComponent<PlayerGroundCheck>();
    }

    void FixedUpdate()
    {
        if (slaming)
        {
            rb.velocity = Vector3.down * slamSpeed * 10;
        }
    }

    public void Slam()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            StartCoroutine(WaitForSlam());
        }
    }

    IEnumerator WaitForSlam()
    {
        circleMain.enabled = false;
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        yield return new WaitForSeconds(airPauseTime);
        slaming = true;
        rb.velocity = Vector3.zero;
        while (true)
        {
            if (playerGroundCheck.isGrounded)
            {
                break;
            }
            yield return null;
        }
        rb.useGravity = true;
        slaming = false;
        circleMain.enabled = true;
    }

    private void OnTriggerStay(Collider collision)
    {
        if (slaming && collision.CompareTag("Slam"))
        {
            collision.gameObject.SetActive(false);
        }
    }
}

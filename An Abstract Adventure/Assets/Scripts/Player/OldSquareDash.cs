using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldSquareDash : MonoBehaviour
{
    public float dashDis;
    public float dashTime;
    public float speedMultiplier;

    private bool dashing;
    private Vector3 moveToSpot;
    private OldSquareMain squareMain;
    private OldPlayerMove playerMove;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        dashing = false;
        rb = GetComponent<Rigidbody>();
        squareMain = GetComponent<OldSquareMain>();
        playerMove = GetComponent<OldPlayerMove>();
    }

    void FixedUpdate()
    {
        if (dashing)
        {
            rb.velocity = (moveToSpot - rb.position) * speedMultiplier;
        }
    }

    public void Dash()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            StartCoroutine(WaitForDash());
        }
    }

    IEnumerator WaitForDash()
    {
        squareMain.enabled = false;
        dashing = true;
        moveToSpot = new Vector3(transform.position.x + dashDis * playerMove.frontDir, transform.position.y, transform.position.z);
        rb.useGravity = false;
        yield return new WaitForSeconds(dashTime);
        rb.useGravity = true;
        dashing = false;
        squareMain.enabled = true;
    }

    private void OnTriggerStay(Collider collision)
    {
        if (dashing && collision.CompareTag("Dash"))
        {
            collision.gameObject.SetActive(false);
        }
    }
}

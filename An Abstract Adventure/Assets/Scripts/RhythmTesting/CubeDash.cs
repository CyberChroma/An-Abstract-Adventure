using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeDash : MonoBehaviour
{
    public bool dashUnlocked;
    public float dashDis;
    public float dashTime;
    public float speedMultiplier;

    [HideInInspector] public bool isDashing;
    [HideInInspector] public bool canDash;
    [HideInInspector] public Vector3 moveToSpot;

    private PlayerMain playerMain;

    // Start is called before the first frame update
    void Start()
    {
        playerMain = GetComponent<PlayerMain>();
    }

    public IEnumerator Dash()
    {
        isDashing = true;
        canDash = false;
        moveToSpot = new Vector3(transform.position.x + dashDis * playerMain.playerMove.frontDir, transform.position.y, transform.position.z);
        playerMain.rb.useGravity = false;
        playerMain.cubeWallJump.currWallJumpVelocity = Vector2.zero;
        yield return new WaitForSeconds(dashTime);
        playerMain.rb.useGravity = true;
        isDashing = false;
        playerMain.playerMove.moveDir = playerMain.playerMove.frontDir;
        if (playerMain.playerGroundDetection.isGrounded)
        {
            yield return new WaitForSeconds(0.1f);
            canDash = true;
        }
    }

    public void StopDashEarly()
    {
        StopCoroutine(Dash());
        playerMain.rb.useGravity = true;
        isDashing = false;
        canDash = true;
    }

    void OnTriggerStay(Collider collision)
    {
        if (isDashing && collision.CompareTag("Dash"))
        {
            collision.gameObject.SetActive(false);
        }
    }
}

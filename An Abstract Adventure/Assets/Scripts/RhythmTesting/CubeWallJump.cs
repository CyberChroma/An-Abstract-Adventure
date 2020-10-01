using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeWallJump : MonoBehaviour
{
    public bool wallJumpUnlocked;
    public float horizontalForce;
    public float verticalForce;
    public float inputStoreTime;
    public float dragH;
    public float dragV;

    [HideInInspector] public bool wallJumpInput;
    [HideInInspector] public int wallDir;
    [HideInInspector] public GameObject wallContact;
    [HideInInspector] public Vector2 currWallJumpVelocity;

    private PlayerMain playerMain;

    // Start is called before the first frame update
    void Start()
    {
        playerMain = GetComponent<PlayerMain>();
    }

    public Vector3 WallJump()
    {
        Vector3 wallJumpVelocity = Vector3.zero;
        playerMain.playerMove.frontDir = -wallDir;
        currWallJumpVelocity = transform.up * verticalForce * 10 + Vector3.right * playerMain.playerMove.frontDir * horizontalForce * 10;
        playerMain.rb.useGravity = true;
        wallJumpInput = false;
        wallContact = null;
        playerMain.cubeDash.canDash = true;
        StopCoroutine(playerMain.playerJump.StoreJumpInput());
        playerMain.playerJump.jumpInput = false;
        StopCoroutine("InputOveride");
        StartCoroutine(playerMain.playerMove.InputOveride(0.25f, 0));
        wallJumpVelocity = currWallJumpVelocity;
        return wallJumpVelocity;
    }

    public Vector3 WallJumpFall()
    {
        Vector3 wallJumpVelocity = Vector3.zero;
        float lastWallJumpVelocityY = currWallJumpVelocity.y;
        currWallJumpVelocity.x /= 1 + dragH;
        currWallJumpVelocity.y /= 1 + dragV;
        if (currWallJumpVelocity.magnitude <= 0.5f)
        {
            currWallJumpVelocity = Vector2.zero;
        }
        wallJumpVelocity.x = currWallJumpVelocity.x;
        wallJumpVelocity.y = lastWallJumpVelocityY - currWallJumpVelocity.y;
        return wallJumpVelocity;
    }

    public IEnumerator StoreWallJumpInput()
    {
        yield return new WaitForSeconds(inputStoreTime);
        wallJumpInput = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 8)
        {
            if (!collision.collider.CompareTag("Slippery"))
            {
                if (Mathf.Abs(collision.contacts[0].normal.x) >= 0.9f)
                {
                    wallContact = collision.gameObject;
                    if (collision.contacts[0].point.x > transform.position.x)
                    {
                        wallDir = 1;
                    }
                    else
                    {
                        wallDir = -1;
                    }

                    //Dash Launch
                    if (playerMain.cubeDashLaunch.dashLaunchUnlocked && playerMain.cubeDash.isDashing)
                    {
                        playerMain.cubeDashLaunch.canDashLaunch = true;
                    }
                }
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == wallContact)
        {
            playerMain.rb.useGravity = true;
            playerMain.playerMove.moveOverride = false;
            wallContact = null;
        }
    }
}

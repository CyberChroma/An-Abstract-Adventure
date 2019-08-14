using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleLungeSlash : MonoBehaviour
{
    public float lungeTime;
    public float lungePower;
    public float lungeDelay;

    private PlayerLineUp playerLineUp;
    private Rigidbody2D rb;
    private PlayerMove playerMove;

    // Start is called before the first frame update
    void Awake()
    {
        playerLineUp = GetComponent<PlayerLineUp>();
        rb = GetComponent<Rigidbody2D>();
        playerMove = GetComponent<PlayerMove>();
    }

    public void LungeSlash()
    {
        if(playerLineUp.released)
        {
            StartCoroutine(WaitToLunge());
        }
    }

    IEnumerator WaitToLunge()
    {
        playerLineUp.released = false;
        playerLineUp.canAim = false;
        playerMove.moveOverride = true;
        playerMove.noDrag = true;
        rb.velocity = Vector2.zero;
        rb.AddForce(playerLineUp.arrow.transform.up.normalized * lungePower, ForceMode2D.Impulse);
        playerMove.moveDir = Vector2.zero;
        rb.gravityScale = 0;
        yield return new WaitForSeconds(lungeTime);
        rb.gravityScale = 1;
        playerMove.moveDir = new Vector2(playerLineUp.arrow.transform.up.normalized.x * 3, 0);
        playerMove.noDrag = false;
        playerMove.moveOverride = false;
        yield return new WaitForSeconds(lungeDelay);
        playerLineUp.canAim = true;
    }
}

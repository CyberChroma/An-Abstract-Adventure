using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareCrouch : MonoBehaviour
{
    private bool setToStand;
    private Transform mainSprite;
    private CapsuleCollider2D cc;
    private PlayerLineUp playerLineUp;

    // Start is called before the first frame update
    void Start()
    {
        setToStand = false;
        mainSprite = transform.Find("Main Sprite");
        cc = GetComponent<CapsuleCollider2D>();
        playerLineUp = GetComponent<PlayerLineUp>();
    }

    public void Crouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            mainSprite.localScale = new Vector3(1.5f, 0.5f, 1);
            cc.size = new Vector2(1.5f, 0.5f);
            playerLineUp.disableAiming = true;
            setToStand = false;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            if (Physics2D.Raycast(transform.position, Vector3.up, 0.5f, 1 << 8))
            {
                setToStand = true;
            }
            else
            {
                Stand();
            }
        }
        if (setToStand && !Physics2D.Raycast(transform.position, transform.up, 0.5f, 1 << 8))
        {
            Stand();
        }
    }

    void Stand ()
    {
        mainSprite.localScale = Vector3.one;
        cc.size = Vector2.one;
        playerLineUp.disableAiming = false;
        setToStand = false;
    }
}

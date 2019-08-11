using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float flickerTime;
    public int flickerNum;
    public float reloadDelay;
    public float knockbackHeight;
    public float disableTime;
    public HealthUI healthUI;

    private int currentHealth;
    private bool canBeDamaged;
    private SpriteRenderer[] images;
    private Rigidbody2D rb;
    private CircleMain circleMain;
    private SquareMain squareMain;

    void Start()
    {
        currentHealth = healthUI.maxHealth;
        canBeDamaged = true;
        images = new SpriteRenderer[3];
        images[0] = GetComponent<SpriteRenderer>();
        images[1] = transform.Find("Inside 1").GetComponent<SpriteRenderer>();
        images[2] = transform.Find("Inside 2").GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        circleMain = GetComponent<CircleMain>();
        squareMain = GetComponent<SquareMain>();
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Kill"))
        {
            Death();
        }
        else if (collision.CompareTag("Hazard") || collision.CompareTag("Enemy"))
        {
            Damage();
        }
    }

    void Damage()
    {
        if (canBeDamaged)
        {
            currentHealth--;
            if (currentHealth <= 0)
            {
                if (circleMain)
                {
                    circleMain.enabled = false;
                }
                else
                {
                    squareMain.enabled = false;
                }
                StartCoroutine(Death());
            }
            else
            {
                rb.velocity = Vector3.zero;
                rb.AddForce(transform.up * knockbackHeight * 10, ForceMode2D.Impulse);
                StartCoroutine(TempDisable());
                StartCoroutine(Flicker());
            }
            if (circleMain)
            {
                circleMain.playerLineUp.DisableArrow();
            }
            else
            {
                squareMain.playerLineUp.DisableArrow();
            }
            healthUI.HealthChange(currentHealth);
        }
    }

    IEnumerator Death()
    {
        currentHealth = 0;
        foreach (SpriteRenderer playerSprite in images)
        {
            playerSprite.enabled = false;
        }
        yield return new WaitForSeconds(reloadDelay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator Flicker()
    {
        canBeDamaged = false;
        for (int i = 0; i < flickerNum; i++) {
            yield return new WaitForSeconds(flickerTime / 2 / flickerNum);
            foreach (SpriteRenderer playerSprite in images)
            {
                playerSprite.enabled = false;
            }
            yield return new WaitForSeconds(flickerTime / 2 / flickerNum);
            foreach (SpriteRenderer playerSprite in images)
            {
                playerSprite.enabled = true;
            }
        }
        canBeDamaged = true;
    }

    IEnumerator TempDisable ()
    {
        if (circleMain)
        {
            circleMain.enabled = false;
        }
        else
        {
            squareMain.enabled = false;
        }
        yield return new WaitForSeconds(disableTime);
        if (circleMain)
        {
            circleMain.enabled = true;
        }
        else
        {
            squareMain.enabled = true;
        }
    }
}

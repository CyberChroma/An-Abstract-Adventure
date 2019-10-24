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
    private GameObject mainSprite;
    private Rigidbody2D rb;
    private CircleMain circleMain;
    private SquareMain squareMain;

    void Start()
    {
        currentHealth = healthUI.maxHealth;
        canBeDamaged = true;
        mainSprite = transform.Find("Main Sprite").gameObject;
        rb = GetComponent<Rigidbody2D>();
        circleMain = GetComponent<CircleMain>();
        squareMain = GetComponent<SquareMain>();
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Kill"))
        {
            StartCoroutine(Death());
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
        if (circleMain)
        {
            circleMain.enabled = false;
        }
        else
        {
            squareMain.enabled = false;
        }
        currentHealth = 0;
        mainSprite.SetActive(false);
        healthUI.HealthChange(currentHealth);
        yield return new WaitForSeconds(reloadDelay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator Flicker()
    {
        canBeDamaged = false;
        for (int i = 0; i < flickerNum; i++) {
            yield return new WaitForSeconds(flickerTime / 2 / flickerNum);
            mainSprite.SetActive(false);
            yield return new WaitForSeconds(flickerTime / 2 / flickerNum);
            mainSprite.SetActive(true);
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

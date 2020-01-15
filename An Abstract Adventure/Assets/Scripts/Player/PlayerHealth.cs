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
    private SpriteRenderer[] sprites;
    private Rigidbody2D rb;
    private Animator anim;
    private CircleMain circleMain;
    private SquareMain squareMain;

    void Start()
    {
        currentHealth = healthUI.maxHealth;
        canBeDamaged = true;
        sprites = transform.Find("Sprites").GetComponentsInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
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
        currentHealth = 0;
        healthUI.HealthChange(currentHealth);
        if (circleMain)
        {
            circleMain.enabled = false;
        }
        else
        {
            squareMain.enabled = false;
        }
        rb.velocity = Vector3.up;
        anim.SetBool("Dead", true);
        yield return new WaitForSeconds(reloadDelay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator Flicker()
    {
        canBeDamaged = false;
        anim.SetTrigger("Hit");
        for (int i = 0; i < flickerNum; i++) {
            yield return new WaitForSeconds(flickerTime / 2 / flickerNum);
            foreach (SpriteRenderer sprite in sprites)
            {
                sprite.enabled = false;
            }
            yield return new WaitForSeconds(flickerTime / 2 / flickerNum);
            foreach (SpriteRenderer sprite in sprites)
            {
                sprite.enabled = true;
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

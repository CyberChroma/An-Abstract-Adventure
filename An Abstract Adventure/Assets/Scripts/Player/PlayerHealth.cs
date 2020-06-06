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
    private MeshRenderer[] meshes;
    private Rigidbody rb;
    private Animator anim;
    private Animator otherAnim;
    private CircleMain circleMain;
    private SquareMain squareMain;
    private PlayerChange playerChange;

    void Start()
    {
        currentHealth = healthUI.maxHealth;
        canBeDamaged = true;
        meshes = transform.Find("Sprites").GetComponentsInChildren<MeshRenderer>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        circleMain = GetComponent<CircleMain>();
        squareMain = GetComponent<SquareMain>();
        otherAnim = GetComponent<PlayerFollowOther>().toFollow.GetComponentInChildren<Animator>();
        playerChange = FindObjectOfType<PlayerChange>();
    }

    void OnTriggerStay(Collider collision)
    {
        if (collision.CompareTag("Kill"))
        {
            StartCoroutine(Death());
        }
        else if (collision.CompareTag("Hazard"))
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
                rb.AddForce(transform.up * knockbackHeight * 10, ForceMode.Impulse);
                StartCoroutine(TempDisable());
                StartCoroutine(Flicker());
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
        rb.isKinematic = true;
        anim.SetBool("Dead", true);
        otherAnim.SetBool("NotActive", false);
        otherAnim.SetBool("Dead", true);
        playerChange.enabled = false;
        yield return new WaitForSeconds(reloadDelay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator Flicker()
    {
        canBeDamaged = false;
        anim.SetTrigger("Hit");
        for (int i = 0; i < flickerNum; i++) {
            yield return new WaitForSeconds(flickerTime / 2 / flickerNum);
            foreach (MeshRenderer mesh in meshes)
            {
                mesh.enabled = false;
            }
            yield return new WaitForSeconds(flickerTime / 2 / flickerNum);
            foreach (MeshRenderer mesh in meshes)
            {
                mesh.enabled = true;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    [Range(3, 5)]
    public int maxHealth;

    private GameObject[] damageSprites;

    // Start is called before the first frame update
    void Awake()
    {
        damageSprites = new GameObject[5];
        Transform healthSprite;
        for (int i = 0; i < 5; i++)
        {
            healthSprite = transform.Find("Health Sprite " + (i + 1).ToString());
            damageSprites[i] = healthSprite.Find("Damage Sprite").gameObject;
            damageSprites[i].SetActive(false);
            if (i >= maxHealth)
            {
                healthSprite.gameObject.SetActive(false);
            }
        }
    }

    public void HealthChange (int currentHealth)
    {
        for (int i = maxHealth-1; i >= currentHealth; i--)
        {
            damageSprites[i].SetActive(true);
        }
    }
}

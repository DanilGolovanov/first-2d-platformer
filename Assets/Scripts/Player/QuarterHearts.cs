using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuarterHearts : MonoBehaviour
{
    [SerializeField] private Image[] heartSlots;
    [SerializeField] private Sprite[] hearts;

    private float currentHealth;
    private float maxHealth;
    private float healthPerSection;

    public void UpdateHearts(float _currentHealth, float _maxHealth)
    {
        currentHealth = _currentHealth;
        maxHealth = _maxHealth;

        healthPerSection = maxHealth / (heartSlots.Length * 4);
    }

    private void Update()
    {
        // index variable starting at 0 for slot checks
        int i = 0;

        foreach (Image image in heartSlots)
        {
            // if current health exceeds current heart's value
            if (currentHealth >= (healthPerSection * 4) + healthPerSection * 4 * i)
            {
                // set heart to 4/4
                heartSlots[i].sprite = hearts[0];
            }
            else if (currentHealth >= (healthPerSection * 3) + healthPerSection * 4 * i)
            {
                // set heart to 3/4
                heartSlots[i].sprite = hearts[1];
            }
            else if (currentHealth >= (healthPerSection * 2) + healthPerSection * 4 * i)
            {
                // set heart to 2/4
                heartSlots[i].sprite = hearts[2];
            }
            else if (currentHealth >= (healthPerSection * 1) + healthPerSection * 4 * i)
            {
                // set heart to 1/4
                heartSlots[i].sprite = hearts[3];
            }
            else
            {
                heartSlots[i].sprite = hearts[4];
            }

            i++;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public int currentHealth;
  
    public int maxHealth = 3;

    private LevelManager levelManager;
    private QuarterHearts quarterHearts;

    [SerializeField]
    private ParticleSystem damageParticle;


    // Start is called before the first frame update
    void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
        currentHealth = maxHealth;
        quarterHearts = FindObjectOfType<QuarterHearts>();
    }

    // Update is called once per frame
    void Update()
    {
        quarterHearts.UpdateHearts(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            levelManager.RespawnPlayer();
        }
    }

    public void DamagePlayer(int damageToGive)
    {
        currentHealth -= damageToGive;
        damageParticle.Play();
    }

    public void RestoreHealth()
    {
        currentHealth = maxHealth;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayer : MonoBehaviour
{
    //public LevelManager levelManager;
    private HealthManager healthManager;

    void Start()
    {
        //levelManager = FindObjectOfType<LevelManager>();
        healthManager = FindObjectOfType<HealthManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            //levelManager.RespawnPlayer();
            healthManager.DamagePlayer(healthManager.maxHealth);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthManager : MonoBehaviour
{
    public int enemyHealth = 3;

    public ParticleSystem deathEffect;

    public Key finalKey;

    private void Start()
    {
        finalKey = FindObjectOfType<Key>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyHealth <= 0)
        {
            if (gameObject.name == "PlayerEnemy")
            {
                finalKey.gameObject.transform.position = new Vector3(8f, -75, 0);
            }
            Destroy(gameObject);
            Instantiate(deathEffect, transform.position, transform.rotation);           
        }
    }

    public void GiveDamage(int damageToGive)
    {
        enemyHealth -= damageToGive;
    }
}

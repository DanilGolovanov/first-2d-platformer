using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayerOnContact : MonoBehaviour
{
    [SerializeField]
    private int damageToGive;

    private HealthManager healthManager;

    // Start is called before the first frame update
    void Start()
    {
        healthManager = FindObjectOfType<HealthManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            healthManager.DamagePlayer(damageToGive);

            PlayerController player = collision.GetComponent<PlayerController>();
            player.knockbackCount = player.knockbackLength;

            if (collision.transform.position.x < transform.position.x)
            {
                player.knockFromRight = true;
            }
            else
            {
                player.knockFromRight = false;
            }
        }
    }
}

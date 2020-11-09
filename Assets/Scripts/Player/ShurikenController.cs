using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenController : MonoBehaviour
{
    [SerializeField]
    private float speed;

    private Rigidbody2D rigidbody2D;

    private PlayerController player;
    
    [SerializeField]
    private ParticleSystem enemyDeathEffectPrefab;
    [SerializeField]
    private ParticleSystem shurikenEffect;

    private int playerLayer = 10;
    private int checkpointsLayer = 9;

    [SerializeField]
    private float rotationSpeed;

    [SerializeField]
    private int damageToGive;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        rigidbody2D = GetComponent<Rigidbody2D>();

        if (player.spriteRenderer.flipX == true)
        {
            speed = -speed;
            rotationSpeed = -rotationSpeed;
        }
    }

    void Update()
    {
        rigidbody2D.velocity = new Vector2(speed, rigidbody2D.velocity.y);
        rigidbody2D.angularVelocity = rotationSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            //particles were flying in the opposite direction of the main camera for some reason
            // 'collision.transform.rotation.y + 180' - makes particles facing the main camera
            Instantiate(enemyDeathEffectPrefab, collision.transform.position, 
                new Quaternion(collision.transform.rotation.x, collision.transform.rotation.y + 180, 
                                collision.transform.rotation.z, collision.transform.rotation.w));

            collision.GetComponent<EnemyHealthManager>().GiveDamage(damageToGive);
        }

        if (collision.gameObject.layer != playerLayer && collision.gameObject.layer != checkpointsLayer)
        {
            Instantiate(shurikenEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        } 
    } 
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject currentCheckpoint;

    private PlayerController player;

    //public GameObject deathParticle;
    [SerializeField]
    private ParticleSystem damageParticle;
    [SerializeField]
    private GameObject respawnParticle;

    [SerializeField]
    private float respawnDelay = 2;

    private bool coroutineIsRunning = false;

    private HealthManager healthManager;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        healthManager = FindObjectOfType<HealthManager>();
    }

    private void Update()
    {
        
    }

    public void RespawnPlayer()
    {
        if (!coroutineIsRunning)
        {
            StartCoroutine("RespawnPlayerCo");
        }
        
    }

    public IEnumerator RespawnPlayerCo()
    {
        coroutineIsRunning = true;
        damageParticle.Play();
        player.enabled = false;
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        yield return new WaitForSeconds(respawnDelay);
        healthManager.RestoreHealth();
        player.transform.position = currentCheckpoint.transform.position;
        player.enabled = true;
        player.knockbackCount = 0;
        coroutineIsRunning = false;
    }
}

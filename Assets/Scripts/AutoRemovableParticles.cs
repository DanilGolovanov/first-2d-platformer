using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class AutoRemovableParticles : MonoBehaviour
{
    ParticleSystem particles;

    private bool coroutineIsRunning = false;

    // Start is called before the first frame update
    void Start()
    {
        particles = gameObject.GetComponent<ParticleSystem>();
        PlayParticles();
    }

    public void PlayParticles()
    {
        if (!coroutineIsRunning)
        {
            StartCoroutine("PlayParticlesCo");
        }
    }

    public IEnumerator PlayParticlesCo()
    {
        coroutineIsRunning = true;
        //particles.Play();
        //gameObject.SetActive(false);
        yield return new WaitForSeconds(particles.main.duration);
        coroutineIsRunning = false;
        Destroy(gameObject);
    }
}

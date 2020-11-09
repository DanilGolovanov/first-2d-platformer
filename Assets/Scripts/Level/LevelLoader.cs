using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    private bool playerInZone;

    public string levelToLoad;

    // Start is called before the first frame update
    void Start()
    {
        playerInZone = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInZone && Input.GetKeyDown(KeyCode.W))
        {
            SceneManager.LoadScene(levelToLoad);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            playerInZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            playerInZone = false;
        }
    }
}

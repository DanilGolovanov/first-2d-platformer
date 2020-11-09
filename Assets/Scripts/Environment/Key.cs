using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public bool keyIsCollected;

    // Update is called once per frame
    void FixedUpdate()
    {
        gameObject.transform.Rotate(gameObject.transform.rotation.x - 1.5f, gameObject.transform.rotation.y, gameObject.transform.rotation.z);   
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            keyIsCollected = true;
            Destroy(gameObject);     
        }      
    }
}

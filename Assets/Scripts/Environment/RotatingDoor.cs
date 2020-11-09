using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingDoor : MonoBehaviour
{
    [SerializeField]
    private Key key;

    [SerializeField]
    private float doorHeight;

    [SerializeField]
    private float doorSpeed = 2.5f;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (key.keyIsCollected)
        {
            // door height is constant
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, doorHeight, gameObject.transform.position.z);
            gameObject.transform.Rotate(gameObject.transform.rotation.x, gameObject.transform.rotation.y, gameObject.transform.rotation.z - doorSpeed);
        }
    }
}

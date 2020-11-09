using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatforms : MonoBehaviour
{
    [SerializeField]
    private float platformHorizontalSpeed;

    [SerializeField]
    private float rightBorder;
    [SerializeField]
    private float leftBorder;

    //[SerializeField]
    //private float topBorder;
    //[SerializeField]
    //private float bottomBorder;

    private bool moveRight = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gameObject.transform.position.x > rightBorder)
        {
            moveRight = false;
        }
        else if (gameObject.transform.position.x < leftBorder)
        {
            moveRight = true;
        }

        if (moveRight)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x + platformHorizontalSpeed, gameObject.transform.position.y, gameObject.transform.position.z);
        }
        else
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x - platformHorizontalSpeed, gameObject.transform.position.y, gameObject.transform.position.z);
        }
    }
}

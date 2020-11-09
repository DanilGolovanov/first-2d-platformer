using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPortal : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Rotate(gameObject.transform.rotation.x, gameObject.transform.rotation.y, gameObject.transform.rotation.z - 1.5f);
    }
}

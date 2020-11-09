using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearingWalls : MonoBehaviour
{
    [SerializeField]
    private Key key;

    // Update is called once per frame
    void Update()
    {
        if (key.keyIsCollected)
        {
            gameObject.SetActive(false);
        }
    }
}

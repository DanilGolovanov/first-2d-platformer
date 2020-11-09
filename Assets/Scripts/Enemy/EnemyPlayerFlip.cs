using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlayerFlip : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<Rigidbody2D>().velocity.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (GetComponent<Rigidbody2D>().velocity.x < 0)
        {
            spriteRenderer.flipX = true;
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public float moveSpeed;
    public bool moveRight;

    private int groundLayer = 8;

    //--------------------Functionality to check the edge

    //public Rigidbody2D rigidbody2D;

    //[SerializeField]
    //private CircleCollider2D leftWallCheck;
    //[SerializeField]
    //private CircleCollider2D rightWallCheck;

    //private bool notAtEdge;
    //[SerializeField]
    //private Transform rightEdgeCheckPosition;
    //[SerializeField]
    //private Transform leftEdgeCheckPosition;
    //[SerializeField]
    //private LayerMask whatIsGround;
    //[SerializeField]
    //private float groundCheckRadius = 3f;

    //--------------------

    // Update is called once per frame
    void Update()
    {
        //notAtEdge =  Physics2D.OverlapCircle(leftEdgeCheckPosition.position, groundCheckRadius, whatIsGround)
        //    && Physics2D.OverlapCircle(rightEdgeCheckPosition.position, groundCheckRadius, whatIsGround);

        //// if reached the edge go other way
        //if (!notAtEdge)
        //{
        //    moveRight = !moveRight;
        //}

        //if (leftWallCheck.IsTouchingLayers(LayerMask.GetMask("Ground")) || rightWallCheck.IsTouchingLayers(LayerMask.GetMask("Ground")))
        //{
        //    moveRight = !moveRight;
        //}

        if (moveRight)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(moveSpeed, GetComponent<Rigidbody2D>().velocity.y);
        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(-moveSpeed, GetComponent<Rigidbody2D>().velocity.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == groundLayer)
        {
            moveRight = !moveRight;
        }
    }
}

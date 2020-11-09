using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float walkSpeed;
    [SerializeField] float accelerateTime;
    [SerializeField] float decelerateTime;
    [SerializeField] Vector2 inputOffset;
    bool canMove = true;

    [Header("Jump")]
    [SerializeField] float jumpingSpeed;
    [SerializeField] float fallMultiplier;
    [SerializeField] float lowJumpMultiplier;
    bool canJump = true;
    bool isJumping;

    [Header("WallJump")]
    [SerializeField] float wallJumpSpeedY;
    [SerializeField] float wallJumpSpeedX;
    [SerializeField] bool canWallJump;
    bool isWallJumped;
    bool isJumpingButtonRelease = true;

    [Header("GroundCheck")]
    [SerializeField] Vector2 pointOffSet;
    [SerializeField] Vector2 size;
    [SerializeField] LayerMask groundLayerMask;
    [SerializeField] LayerMask enemyLayerMask;
    bool gravityModifier = true;
    bool isOnGround;

    [Header("WallCheck")]
    [SerializeField] Vector2 leftPointOffSet;
    [SerializeField] Vector2 rightPointOffSet;
    [SerializeField] Vector2 onWallSize;
    bool isOnWall = true;
    bool isOnLeftWall = true;
    bool isOnRightWall = true;

    [Header("Dash")]
    [SerializeField] float dashForce;
    [SerializeField] float dragMaxForce;
    [SerializeField] float dragDuration;
    [SerializeField] float dashWaitTime;
    bool wasDashed;
    Vector2 dir;

    Rigidbody2D rigidBody;
    Animator animator;
    public SpriteRenderer spriteRenderer;

    float velocityX;

    [Header("Weapon")]
    public Transform firePoint;
    public GameObject shuriken;

    [Header("Knockback")]
    [SerializeField] float knockback;
    public float knockbackLength;
    public float knockbackCount;
    public bool knockFromRight;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        //Ground Check
        isOnGround = OnGround();
        isOnLeftWall = OnLeftWall();
        isOnRightWall = OnRightWall();
        isOnWall = isOnLeftWall ^ isOnRightWall;

        #region Movement
        if (canMove)
        {
            //Move left and right
            if (Input.GetAxisRaw("Horizontal") > inputOffset.x)
            {
                rigidBody.velocity = new Vector2(Mathf.SmoothDamp(rigidBody.velocity.x, walkSpeed * Time.fixedDeltaTime * 60, ref velocityX, accelerateTime), rigidBody.velocity.y);
                spriteRenderer.flipX = false;
                animator.SetFloat("Walk", 1f);
            }
            else if (Input.GetAxisRaw("Horizontal") < inputOffset.x * -1)
            {
                rigidBody.velocity = new Vector2(Mathf.SmoothDamp(rigidBody.velocity.x, walkSpeed * Time.fixedDeltaTime * -60, ref velocityX, accelerateTime), rigidBody.velocity.y);
                spriteRenderer.flipX = true;
                animator.SetFloat("Walk", 1f);
            }
            else
            {
                rigidBody.velocity = new Vector2(Mathf.SmoothDamp(rigidBody.velocity.x, 0, ref velocityX, decelerateTime), rigidBody.velocity.y);
                animator.SetFloat("Walk", 0f);
            }
        }
        #endregion

        #region Jumping and Falling
        if (canJump)
        {
            if (Input.GetAxis("Jump") == 1 && !isJumping)
            {
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpingSpeed);
                isJumping = true;
                isJumpingButtonRelease = false;
            }

            if (isOnGround && (Input.GetAxis("Jump") == 0))
            {
                isJumping = false;
            }
        }
        if (gravityModifier)
        {
            //When player is falling
            if (rigidBody.velocity.y < 0)
            {
                //Speed up falling
                rigidBody.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
            }
            //When player is jumping and release the jump button
            else if (rigidBody.velocity.y > 0 && Input.GetAxis("Jump") != 1)
            {
                //Slow down jumping
                rigidBody.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
            }
        }
        #endregion

        #region Jumping Button Reset
        if (!isJumpingButtonRelease && Input.GetAxis("Jump") == 0)
        {
            isJumpingButtonRelease = true;
        }
        #endregion

        #region Dashing
        if (Input.GetAxisRaw("Dash") == 1 && !wasDashed)
        {
            wasDashed = true;
            dir = GetDir();
            //Zero all the momentum
            rigidBody.velocity = Vector2.zero;
            //Add a force to dash
            rigidBody.velocity += dir.normalized * dashForce;
            StartCoroutine(Dash());
        }

        if (isOnGround && Input.GetAxisRaw("Dash") == 0)
        {
            wasDashed = false;
        }
        #endregion

        #region WallJump
        if (canWallJump)
        {
            if (Input.GetAxis("Jump") == 1 && isOnWall && !isOnGround && !isWallJumped && isJumpingButtonRelease)
            {
                if (isOnLeftWall)
                {
                    rigidBody.velocity = new Vector2(wallJumpSpeedX, wallJumpSpeedY);
                }
                else
                {
                    rigidBody.velocity = new Vector2(wallJumpSpeedX * -1, wallJumpSpeedY);
                }
                isWallJumped = true;
                isJumpingButtonRelease = false;
            }

            if (isOnGround && Input.GetAxis("Jump") == 0)
            {
                isWallJumped = false;
            }
        }
        #endregion
    }

    private void Update()
    {
        if (spriteRenderer.flipX == false)
        {
            firePoint.localPosition = new Vector3(0.6f, firePoint.localPosition.y, firePoint.localPosition.z);
        }
        else
        {
            firePoint.localPosition = new Vector3(-0.6f, firePoint.localPosition.y, firePoint.localPosition.z);
        }

        if (animator.GetBool("Throw"))
        {
            animator.SetBool("Throw", false);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            animator.SetBool("Throw", true);
            Instantiate(shuriken, firePoint.position, firePoint.rotation);
        }  

        if (knockbackCount <= 0)
        {
            canMove = true;
        }
        else
        {
            if (knockFromRight)
            {
                rigidBody.velocity = new Vector2(-knockback, knockback);
                canMove = false;
            }
            if (!knockFromRight)
            {
                rigidBody.velocity = new Vector2(knockback, knockback);
                canMove = false;
            }
            knockbackCount -= Time.deltaTime;
        }
    }

    IEnumerator Dash()
    {
        //Animate dash
        animator.SetBool("Dash", true);
        //Shut down movement function
        canMove = false;
        //Shut down  jumping function
        canJump = false;
        //Shut down falling function
        gravityModifier = false;
        //Shut down gravity function
        rigidBody.gravityScale = 0;
        //Add a air resistance (Rigidbody.Drag)
        DOVirtual.Float(dragMaxForce, 0, dragDuration, (x) => rigidBody.drag = x);
        //Wait for a while
        yield return new WaitForSeconds(dashWaitTime);
        //Open everything we shut down
        canMove = true;
        canJump = true;
        gravityModifier = true;
        rigidBody.gravityScale = 1;
        animator.SetBool("Dash", false);
    }

    public Vector2 GetDir()
    {
        Vector2 tempDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (tempDir.x == 0 && tempDir.y == 0)
        {
            if (!spriteRenderer.flipX)
            {
                tempDir.x = 1;
            }
            else
            {
                tempDir.x = -1;
            }
        }
        return tempDir;
    }

    bool OnGround()
    {
        Collider2D collidedWithGround = Physics2D.OverlapBox((Vector2)transform.position + pointOffSet, size, 0, groundLayerMask);
        Collider2D collidedWithEnemy = Physics2D.OverlapBox((Vector2)transform.position + pointOffSet, size, 0, enemyLayerMask);
        if (collidedWithGround != null || collidedWithEnemy != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool OnLeftWall()
    {
        Collider2D coll = Physics2D.OverlapBox((Vector2)transform.position + leftPointOffSet, onWallSize, 0, groundLayerMask);
        if (coll != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool OnRightWall()
    {
        Collider2D coll = Physics2D.OverlapBox((Vector2)transform.position + rightPointOffSet, onWallSize, 0, groundLayerMask);
        if (coll != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube((Vector2)transform.position + pointOffSet, size);
        Gizmos.DrawWireCube((Vector2)transform.position + leftPointOffSet, onWallSize);
        Gizmos.DrawWireCube((Vector2)transform.position + rightPointOffSet, onWallSize);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            transform.parent = collision.transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            transform.parent = null;
        }
    }
}
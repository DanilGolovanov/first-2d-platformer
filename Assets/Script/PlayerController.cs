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
    [SerializeField] float wallJumpSpeedy;
    [SerializeField] float wallJumpSpeedX;
    [SerializeField] bool canWallJump;
    bool isWallJumped;
    bool isJumpingButtonRelease = true;

    [Header("GroundCheck")]
    [SerializeField] Vector2 pointOffSet;
    [SerializeField] Vector2 size;
    [SerializeField] LayerMask groundLayerMask;
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

    Rigidbody2D rd;
    Animator anim;
    SpriteRenderer sr;

    float velocityX;

    void Awake()
    {
        rd = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
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
                rd.velocity = new Vector2(Mathf.SmoothDamp(rd.velocity.x, walkSpeed * Time.fixedDeltaTime * 60, ref velocityX, accelerateTime), rd.velocity.y);
                sr.flipX = false;
                anim.SetFloat("Walk", 1f);
            }
            else if (Input.GetAxisRaw("Horizontal") < inputOffset.x * -1)
            {
                rd.velocity = new Vector2(Mathf.SmoothDamp(rd.velocity.x, walkSpeed * Time.fixedDeltaTime * -60, ref velocityX, accelerateTime), rd.velocity.y);
                sr.flipX = true;
                anim.SetFloat("Walk", 1f);
            }
            else
            {
                rd.velocity = new Vector2(Mathf.SmoothDamp(rd.velocity.x, 0, ref velocityX, decelerateTime), rd.velocity.y);
                anim.SetFloat("Walk", 0f);
            }
        }
        #endregion

        #region Jumping and Falling
        if (canJump)
        {
            if (Input.GetAxis("Jump") == 1 && !isJumping)
            {
                rd.velocity = new Vector2(rd.velocity.x, jumpingSpeed);
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
            if (rd.velocity.y < 0)
            {
                //Speed up falling
                rd.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
            }
            //When player is jumping and release the jumo button
            else if (rd.velocity.y > 0 && Input.GetAxis("Jump") != 1)
            {
                //Slow down jumping
                rd.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
            }
        }
        #endregion

        #region Jumping Button Reset
        if(!isJumpingButtonRelease && Input.GetAxis("Jump") == 0)
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
            rd.velocity = Vector2.zero;
            //Add a force to dash
            rd.velocity += dir.normalized * dashForce;
            StartCoroutine(Dash());
        }

        if(isOnGround && Input.GetAxisRaw("Dash") == 0)
        {
            wasDashed = false;
        }


        #endregion

        #region WallJump
        if (canWallJump)
        {
            if (Input.GetAxis("Jump") == 1 && isOnWall && !isOnGround && !isWallJumped && isJumpingButtonRelease)
            {
                if(isOnLeftWall)
                {
                    rd.velocity = new Vector2(wallJumpSpeedX, wallJumpSpeedy);
                }
                else
                {
                    rd.velocity = new Vector2(wallJumpSpeedX * -1, wallJumpSpeedy);
                }
                isWallJumped = true;
                isJumpingButtonRelease = false;
            }

            if(isOnGround && Input.GetAxis("Jump") == 0)
            {
                isWallJumped = false;
            }
        }
        #endregion
    }

    IEnumerator Dash()
    {
        //Shut down movement function
        canMove = false;
        //Shut down  jumping function
        canJump = false;
        //Shut down falling function
        gravityModifier = false;
        //Shut down gravity function
        rd.gravityScale = 0;
        //Add a air resistance (Rigidbody.Drag)
        DOVirtual.Float(dragMaxForce, 0, dragDuration, (x) => rd.drag = x);
        //Wait for a while
        yield return new WaitForSeconds(dashWaitTime);
        //Open everything we shut down
        canMove = true;
        canJump = true;
        gravityModifier = true;
        rd.gravityScale = 1;

    }

    public Vector2 GetDir()
    {
        Vector2 tempDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if(tempDir.x == 0 && tempDir.y == 0)
        {
            if (!sr.flipX)
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
        Collider2D coll = Physics2D.OverlapBox((Vector2)transform.position + pointOffSet, size, 0, groundLayerMask);
        if (coll != null)
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
}

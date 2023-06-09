using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private float horizontal;
    private float speed = 8f;
    private float jumpingPower = 12f;
    private bool isFacingRight = true;

    [SerializeField] private bool jumpRestored = false;

    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private bool isWallSliding;
    private float wallSlidingSpeed = 2f;

    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    [SerializeField] private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;
    private Vector2 wallJumpingPower = new Vector2(6f, 12f);

    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;

    public GameObject Player;
    
    private Controls controls;


    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        controls = Player.GetComponent<Controls>();
    }


    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && (IsGrounded() || jumpRestored))
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpingPower);
            jumpRestored = false;
        }

        if (Input.GetButtonUp("Jump") && rigidBody.velocity.y > 0f)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, rigidBody.velocity.y * 0.5f);
        }

        Flip();
        WallSlide();
        WallJump();
    }

    private void FixedUpdate()
    {
        
        if(horizontal > 0)
        {
            rigidBody.velocity += Vector2.right * (horizontal * speed);
            rigidBody.velocity = Vector2.Min(rigidBody.velocity, new Vector2(15, rigidBody.velocity.y));
        } else if (horizontal < 0)
        {
            rigidBody.velocity += Vector2.left * (horizontal * speed);
            rigidBody.velocity = Vector2.Min(rigidBody.velocity, new Vector2(-15, rigidBody.velocity.y));
        }
        else
        {
            if(rigidBody.velocity.x > 1)
            {
                rigidBody.velocity -= Vector2.left * (horizontal * 2);
                rigidBody.velocity = Vector2.Min(rigidBody.velocity, new Vector2(0, rigidBody.velocity.y));
            }
            else if(rigidBody.velocity.x < -1)
            {
                rigidBody.velocity -= Vector2.right * (horizontal * 2);
                rigidBody.velocity = Vector2.Max(rigidBody.velocity, new Vector2(0, rigidBody.velocity.y));
            }
            else
            {
                rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
            }
        }
        

    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }


    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    private void WallSlide()
    {
        //This requires you to alternate between right and left walls, and you can't switch without wall jumping
        if (IsWalled() && !IsGrounded() && horizontal != 0f)
        {
            isWallSliding = true;
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, Mathf.Clamp(rigidBody.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {
            Debug.Log("BAAAAAAAAAAAAAAAAAAAA");
            isWallJumping = true;
            rigidBody.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x * 300, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }

    void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.tag == "Spring")
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpingPower * 1.25f);
        }
        if (collider.gameObject.tag == "Restore")
        {
            jumpRestored = true;
            collider.gameObject.SetActive(false);
            //set true after two seconds
        }
    }
}

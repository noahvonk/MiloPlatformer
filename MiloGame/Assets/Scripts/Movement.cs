using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Movement")]
    [Tooltip("Speed of movement")]
    public float speed;

    private bool isFacingRight = true;

    private float horizontal;
    private float vertical;

    private bool isWallSliding;
    private float wallSlidingSpeed = 2f;

    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;
    //Sweet spot somewhere between 6, 12 and 8, 16
    private Vector2 wallJumpingPower = new Vector2(7f, 14f);

    private Rigidbody2D rigidBody;

    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;

    public GameObject Player;
    
    private Controls controls;

    public enum MovementType
    {
        AllDirections,
        HorizontalOnly,
        VerticalOnly
    }

    [SerializeField]
    private MovementType movementType = MovementType.HorizontalOnly;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        controls = Player.GetComponent<Controls>();
    }

    void Start()
    {

    }

    void FixedUpdate()
    {
        // get the user input values for both axes
        //float horizontal = Input.GetAxis("Horizontal");
        //float vertical = Input.GetAxis("Vertical");
        //horizontal = Input.GetAxisRaw("Horizontal");
        //vertical = Input.GetAxisRaw("Vertical");

        // look at the movementType variable
        // if we only want horizontal movement, then zero out the value for vertical
        // if we only want vertical movement, then zero out the value for horizontal
        // if we want all directions, do not change the user input values
        switch (movementType)
        {
            case MovementType.HorizontalOnly:
                vertical = 0f;
                break;
            case MovementType.VerticalOnly:
                horizontal = 0f;
                break;
        }

        // create a new movement vector based on the horizontal and vertical values
        Vector3 movement = new Vector3(horizontal, vertical);

        // move Codey's position based on the movement vector
        // and scale it based on the time that has passed and the speed
        transform.position += movement * Time.deltaTime * speed;


        //ALL OF THE FOLLOWING is raycast stuff to retry if Plan B fails

        //RaycastHit2D hit;
        //hit = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector3.right), 1f);
        //if (hit.collider && !hit.collider.CompareTag("Player") && !hit.collider.CompareTag("Room"))
        //{
            //Debug.Log("Hit2");
            //Debug.Log(hit.collider.transform.position.ToString());
            //Debug.DrawLine(transform.position, hit.transform.position, Color.red);
            //GetComponent<Controls>().touchingWall = true;
        //}

        //if (hit.collider.CompareTag("Ground"))
        //{
            //Debug.Log("Hit");
        //}
        //else
        //{
            //Debug.Log("Miss");
            //Debug.DrawLine(transform.position, transform.position + transform.TransformDirection(Vector3.right), Color.white);
            //GetComponent<Controls>().touchingWall = false;
        //}
    }

    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        WallSlide();
        WallJump();
    }

    //private bool IsGrounded()
    //{
        //return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    //}

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    private void WallSlide()
    {
        if (IsWalled() && !controls.onGround && horizontal != 0f)
        {
            isWallSliding = true;
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, Mathf.Clamp(rigidBody.velocity.y, -wallSlidingSpeed, float.MaxValue));
            Debug.Log("onWall");
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
            Debug.Log("Wall Jumped");
            isWallJumping = true;
            rigidBody.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
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
}

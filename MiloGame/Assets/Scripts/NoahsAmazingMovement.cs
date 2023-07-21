using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NoahsAmazingMovement : MonoBehaviour
{

    /// <summary>
    /// Updates all other functions each frame
    /// </summary>
    public void Update()
    {
        Move();

        if (dir == direction.RIGHT)
        {
            int mask = LayerMask.GetMask("Wall");
            _hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), new Vector2(1f, 0), .6f, mask);
            Debug.DrawRay(new Vector3(transform.position.x, transform.position.y, 0), new Vector3(.6f, 0, 0));
        }
        else
        {
            int mask = LayerMask.GetMask("Wall");
            _hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), new Vector2(-1f, 0), .6f, mask);
            Debug.DrawRay(new Vector3(transform.position.x, transform.position.y, 0), new Vector3(-.6f, 0, 0));
        }

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }

    }

    /// <summary>
    /// Checks to see if input has been pressed every frame and updates the physics system
    /// </summary>
    private void Move()
    {
        Vector2 velo;
        float move = Input.GetAxis("Horizontal");
        if (_WallJumping)
        {
            velo = new(move * _maxSpeed/3, _rb.velocity.y);
        }
        else
        {
            velo = new(move * _maxSpeed, _rb.velocity.y);
        }
        
        if(move > 0)
        {
            dir = direction.RIGHT;
        }
        else if (move < 0)
        {
            dir = direction.LEFT;
        }

        if (!_grounded && _inWaterfall && velo.y > _MaxWaterfallDropSpeed)
        {
            Debug.Log("Milo is stinky, but getting clean in the waterfall");
            velo.y -= _WaterfallFallSpeedIncrement;
        }
        else if(!_grounded && _inWaterfall)
        {
            velo.y = -_MaxWaterfallDropSpeed;
        }


        _rb.velocity = velo;
    }
    /// <summary>
    /// Checks for jump input and updates the physics system
    /// </summary>
    private void Jump()
    {
        if (_grounded)
        {
            _rb.velocity +=  Vector2.up * _jumpStrength;
            Debug.Log("Jumping: " + _rb.velocity.ToString());
        }
        else
        {
            Debug.Log(_hit.collider);
            if(_hit.collider != null && _hit.collider.gameObject.CompareTag("Ground"))
            {
                if(dir == direction.LEFT)
                {
                    _rb.velocity = Vector2.zero;
                    _rb.AddForce(new Vector2(_wallJumpStrength * _maxSpeed * 20, 300));
                }
                else if (dir == direction.RIGHT)
                {
                    _rb.velocity = Vector2.zero;
                    _rb.AddForce(new Vector2(-_wallJumpStrength * _maxSpeed * 20, 300));
                }
                _WallJumping = true;
                Debug.Log("wall jumping");
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer renderer = GetComponentInChildren<SpriteRenderer>();
        _rb = GetComponentInChildren<Rigidbody2D>();
        _ground = new Vector3(0, renderer.localBounds.size.y, transform.position.z);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collided with item with tag: " + collision.collider.tag + "\nPlayer Pos = " + transform.position.ToString() + " and colliders pos = " + collision.collider.transform.position);
        if (_WallJumping)
        {
            _WallJumping = false;
        }
        if (collision.collider.CompareTag("Ground") && collision.collider.gameObject.transform.localPosition.y >= _ground.y)
        {
            Debug.Log("Entering Ground");
            _grounded = true;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Waterfall"))
        {
            _inWaterfall = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Waterfall"))
        {
            _inWaterfall = false;
        }
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground") && collision.collider.gameObject.transform.localPosition.y >= _ground.y)
        {
            Debug.Log("Leaving Ground");
            _grounded = false;
        }

    }

    // serialized fields

    [SerializeField]
    private float _speedIncrement = 2f;

    [SerializeField]
    private float _maxSpeed = 10f;

    [SerializeField]
    private float _jumpStrength = 15f;

    [SerializeField]
    private float _gravity = -9.8f;

    [SerializeField]
    private float _wallJumpStrength = 10f;

    [SerializeField]
    private float _WaterfallFallSpeedIncrement = 2f;

    [SerializeField]
    private float _MaxWaterfallDropSpeed = 10f;
    // private fields

    private float _appliedForce = 0.0f;

    private Vector3 _ground = Vector3.zero;

    private direction dir = direction.RIGHT;

    [SerializeField]
    private bool _grounded = false;

    private Rigidbody2D _rb;

    RaycastHit2D _hit;

    private bool _WallJumping = false;
    float _curJump = 0f;

    [SerializeField]
    private bool _inWaterfall = false;

    
    // enums

    enum direction { LEFT, RIGHT}


}

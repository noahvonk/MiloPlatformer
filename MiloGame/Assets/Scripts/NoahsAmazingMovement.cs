using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
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
            if(move > 0)
            {
                velo = new(Mathf.Max(_rb.velocity.x * (move), _wallJumpStrength), _rb.velocity.y);
            }
            else if(move < 0)
            {
                velo = new(Mathf.Min(_rb.velocity.x * (move ), -_wallJumpStrength), _rb.velocity.y);
            }
            else
            {
                velo = new(Mathf.MoveTowards(_rb.velocity.x, 0, 0.1f), _rb.velocity.y);
            }
            
        }
        else
        {
            if (move > 0)
            {
                velo = new(Mathf.Max(_rb.velocity.x * (move), _maxSpeed), _rb.velocity.y);
            }
            else if (move < 0)
            {
                velo = new(Mathf.Min(_rb.velocity.x * (move), -_maxSpeed), _rb.velocity.y);
            }
            else
            {
                velo = new(0, _rb.velocity.y);
            }
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

        Debug.Log(velo);
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
            if( !_WallJumping && _hit.collider != null && _hit.collider.gameObject.CompareTag("Ground"))
            {
                if(dir == direction.LEFT) dir = direction.RIGHT;
                else if(dir == direction.RIGHT) dir = direction.LEFT;

                if(dir == direction.LEFT)
                {
                    _rb.velocity = new Vector2(-_rb.velocity.x, 0);
                    _rb.AddRelativeForce(new Vector2(-_wallJumpStrength, 8), ForceMode2D.Impulse);
                }
                else if (dir == direction.RIGHT)
                {
                    _rb.velocity = new Vector2(-_rb.velocity.x, 0);
                    _rb.AddRelativeForce(new Vector2(_wallJumpStrength, 8), ForceMode2D.Impulse);
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
        _floorCollider = GetComponentInChildren<BoxCollider2D>();
        enter += OnBoxCollidedEntered;
        GetComponentInChildren<ContactReporter>()._enteredReportBacks.Add(enter);
        exit += OnBoxCollidedExited;
        GetComponentInChildren<ContactReporter>()._exitedReportBacks.Add(exit);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {

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

    private void OnBoxCollidedEntered()
    {
        Debug.Log("Entering Ground");
        _grounded = true;
        if (_WallJumping)
        {
            _WallJumping = false;
        }
    }

    private void OnBoxCollidedExited()
    {
        Debug.Log("Leaving Ground");
        _grounded = false;
    }


    private void OnCollisionExit2D(Collision2D collision)
    {

    }

    // unity actions
    UnityAction enter;
    UnityAction exit;


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

    private BoxCollider2D _floorCollider;
    // enums

    enum direction { LEFT, RIGHT}


}

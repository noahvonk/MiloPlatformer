using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class NoahsAmazingMovement : MonoBehaviour
{


    /// <summary>
    /// Updates all other functions each frame
    /// </summary>
    public void Update()
    {

        Move();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    /// <summary>
    /// Checks to see if input has been pressed every frame and updates the physics system
    /// </summary>
    private void Move()
    {
        Vector2 velo = Vector2.zero;
        float move = Input.GetAxis("Horizontal");
        if (move > 0)
        {
            SetDirection(ref dir, direction.RIGHT);
            transform.localScale = new Vector3(1, 1, 1);
            int mask = LayerMask.GetMask("Wall");
            _hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), new Vector2(1f, 0), .6f, mask);
            Debug.DrawRay(new Vector3(transform.position.x, transform.position.y, 0), new Vector3(.6f, 0, 0));
            //Debug.Log(_rb.velocity + "Test4");
            if (!_WallJumping)
            {
                _canClimb = true;
            }
        }
        else if (move < 0)
        {
            SetDirection(ref dir, direction.LEFT);
            transform.localScale = new Vector3(-1, 1, 1);
            int mask = LayerMask.GetMask("Wall");
            _hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), new Vector2(-1f, 0), .6f, mask);
            Debug.DrawRay(new Vector3(transform.position.x, transform.position.y, 0), new Vector3(-.6f, 0, 0));
            //Debug.Log(_rb.velocity + "Test5");
            if (!_WallJumping)
            {
                _canClimb = true;
            }
        }
        else
        {
            _canClimb = false;
        }
        if (_hit.collider != null && _hit.collider.gameObject.CompareTag("Ground"))
        {
            _WallJumping = false;
        }


        if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) && _canClimb && _hit.collider != null && _hit.collider.gameObject.CompareTag("Ground"))
        {

            //Debug.Log("In Climbing position");
            velo = Climb();
            _rb.gravityScale = 0;

        }
        else
        {
            _rb.gravityScale = 1;
            if (_WallJumping)
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
                    velo = _rb.velocity;
                }
            //Debug.Log(_rb.velocity + "Test2");
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
            //Debug.Log(_rb.velocity + "Test3");
            }

        }


        if (!_grounded && _inWaterfall && velo.y > _MaxWaterfallDropSpeed)
        {
            //Debug.Log("Milo is stinky, but getting clean in the waterfall");
            velo.y -= _WaterfallFallSpeedIncrement;
            //Debug.Log(_rb.velocity + "Test6");
        }
        else if(!_grounded && _inWaterfall)
        {
            velo.y = -_MaxWaterfallDropSpeed;
            //Debug.Log(_rb.velocity + "Test7");
        }

        //Debug.Log(velo);
        _rb.velocity = velo;
        //Debug.Log(_rb.velocity + "Test8");
    }
    /// <summary>
    /// Checks for jump input and updates the physics system
    /// </summary>
    private void Jump()
    {
        if (_grounded)
        {
            _rb.velocity +=  Vector2.up * _jumpStrength;
            //Debug.Log("Jumping: " + _rb.velocity.ToString());
        }
        else
        {
            //Debug.Log(_hit.collider);
            if( !_WallJumping && _hit.collider != null && _hit.collider.gameObject.CompareTag("Ground"))
            {
                _canClimb = false;
                ChangeDirection(ref dir);

                if(dir == direction.LEFT)
                {
                    _rb.velocity = new Vector2(-Mathf.Abs(_rb.velocity.x), 0);
                    _appliedForce = new Vector2(-_wallJumpStrength, 8);
                    StartCoroutine(WallJumpEnumerator());
                }
                else if (dir == direction.RIGHT)
                {
                    _rb.velocity = new Vector2(Mathf.Abs(_rb.velocity.x), 0);
                    _appliedForce = new Vector2(_wallJumpStrength, 8);
                    StartCoroutine(WallJumpEnumerator());
                }
                _WallJumping = true;
                //Debug.Log("wall jumping");
            }
        }
    }

    private void ChangeDirection(ref direction d)
    {
        if(d == direction.LEFT)
        {
            d = direction.RIGHT;
        }
        else
        {
            d = direction.LEFT;
        }
        ChangeCharacterLookingDirection();
    }

    private void SetDirection(ref direction d, direction dir)
    {
        d = dir;
        ChangeCharacterLookingDirection();
    }

    private void ChangeCharacterLookingDirection()
    {
        if(dir == direction.LEFT)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    private Vector2 Climb()
    {
        //Debug.Log("Milo is smell");
        return new Vector2(_rb.velocity.x, _climbSpeed);
        //Debug.Log(_rb.velocity + "Climb");
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
        //Debug.Log(_rb.velocity + "TriggerEnter");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Waterfall"))
        {
            _inWaterfall = false;
        }
        //Debug.Log(_rb.velocity + "TriggerExit");
    }

    private void OnBoxCollidedEntered()
    {
        //Debug.Log("Entering Ground");
        _grounded = true;
        if (_WallJumping)
        {
            _WallJumping = false;
        }
    }

    private void OnBoxCollidedExited()
    {
        //Debug.Log("Leaving Ground");
        _grounded = false;
    }


    private void OnCollisionExit2D(Collision2D collision)
    {

    }

    // unity actions
    UnityAction enter;
    UnityAction exit;

    private IEnumerator WallJumpEnumerator()
    {
        Vector2.MoveTowards(_appliedForce, new Vector2(0,0), .05f);
        _rb.AddForce(_appliedForce, ForceMode2D.Impulse);
        while(_WallJumping && _appliedForce.x != 0 && _appliedForce.y != 0)
        {
            yield return null;
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
    private float _climbSpeed = 10f;

    [SerializeField]
    private float _WaterfallFallSpeedIncrement = 2f;

    [SerializeField]
    private float _MaxWaterfallDropSpeed = 10f;
    // private fields

    private Vector2 _appliedForce = new Vector2(0.0f, 0.0f);

    private Vector3 _ground = Vector3.zero;

    private direction dir = direction.RIGHT;

    [SerializeField]
    private bool _grounded = false;

    private Rigidbody2D _rb;

    private bool _canClimb = true;

    RaycastHit2D _hit;

    private bool _WallJumping = false;
    float _curJump = 0f;

    [SerializeField]
    private bool _inWaterfall = false;

    private BoxCollider2D _floorCollider;
    // enums

    enum direction { LEFT, RIGHT}


}

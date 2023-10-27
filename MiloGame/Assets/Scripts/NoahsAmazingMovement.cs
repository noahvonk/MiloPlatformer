using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.UIElements.UxmlAttributeDescription;

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
            //perhaps here will be code that will help prevent mr. enemy man from running through walls
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
            //velo = Climb();
            //_rb.gravityScale = 0;

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

        // if (!_grounded && _upWaterfall && velo.y > _MaxWaterfallDropSpeed)
        // {
        //     velo.y += _WaterfallFallSpeedIncrement;
        //     Debug.Log("First if");
        // }
        if (_upWaterfall)
        {
            velo.y = _MaxWaterfallDropSpeed;
            Debug.Log("Second if");
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
        if (_grounded || restored)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, 0);
            _rb.velocity +=  Vector2.up * _jumpStrength;
            //Debug.Log("Jumping: " + _rb.velocity.ToString());
            restored = false;
        }
        else
        {
            //Debug.Log(_hit.collider);
            if( !_WallJumping && _hit.collider != null && (_hit.collider.gameObject.CompareTag("Ground") || _hit.collider.gameObject.CompareTag("Wall") || _hit.collider.gameObject.CompareTag("LeftWall") || _hit.collider.gameObject.CompareTag("RightWall")))
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
        GetComponentInChildren<ContactReporter>()._enteredReportBacks.Add(OnBoxCollidedEntered);
        GetComponentInChildren<ContactReporter>()._exitedReportBacks.Add(OnBoxCollidedExited);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Waterfall"))
        {
            _inWaterfall = true;
            Debug.Log("Waterfall");
        }
        if (collision.CompareTag("Reverse"))
        {
            _upWaterfall = true;
            Debug.Log("Reverse Waterfall");
        }
        if (collision.CompareTag("Restore"))
        {
            var jumpRestore = collision.transform.GetComponent<JumpRestore>();
            if (!jumpRestore.used && !grounded)
            {
                restored = true;
                StartCoroutine(jumpRestore.RespawnTime());
                collision.gameObject.SetActive(false);
            }
            //deactivate gameobject
        }
        if (collision.CompareTag("Lift"))
        {
            
        }
        //Debug.Log(_rb.velocity + "TriggerEnter");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Waterfall"))
        {
            _inWaterfall = false;
        }
        if (collision.CompareTag("Reverse"))
        {
            _upWaterfall = false;
        }
        //Debug.Log(_rb.velocity + "TriggerExit");
    }



    private void OnBoxCollidedExited(Collider2D collider)
    {
        //Debug.Log("Leaving Ground");
        _grounded = false;
    }

    private void OnBoxCollidedEntered(Collider2D collider)
    {
        //Debug.Log("Entering Ground");
        _grounded = true;
        if (_WallJumping)
        {
            _WallJumping = false;
        }
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

    public static bool grounded
    {
        get => _grounded;
        set => _grounded = value;
    }

    [SerializeField]
    private static bool _grounded = false;

    private Rigidbody2D _rb;

    private bool _canClimb = true;

    RaycastHit2D _hit;

    private bool _WallJumping = false;
    float _curJump = 0f;

    [SerializeField]
    private bool _inWaterfall = false;

    [SerializeField]
    private bool _upWaterfall = false;

    public bool restored = false;

    private BoxCollider2D _floorCollider;
    // enums

    enum direction { LEFT, RIGHT}


}

using System.Collections;
using System.Collections.Generic;
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

        if (dir == direction.RIGHT)
        {

            int mask = LayerMask.GetMask("Wall");
            _hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), new Vector2(0, 1), 1, mask);

        }
        else
        {
            int mask = LayerMask.GetMask("Wall");
            _hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), new Vector2(0, -1), 1, mask);
        }

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        Move();
    }

    /// <summary>
    /// Checks to see if input has been pressed every frame and updates the physics system
    /// </summary>
    private void Move()
    {
        float move = Input.GetAxis("Horizontal");

        Vector2 velo = new(move, 0);

        velo.x = move * Time.deltaTime * _maxSpeed;

        _rb.MovePosition(new Vector2(transform.position.x, transform.position.y) + velo * Time.deltaTime * _maxSpeed);

        //if (move != 0)
        //{
        //    Vector2 moveDir = Vector2.zero;

        //    // check if we are at or going to be at max speed
        //    Vector2 velo = _rb.velocity.Abs();
        //    if (velo.x == _maxSpeed)
        //    {
        //        return;
        //    }
        //    else if (velo.x >= _maxSpeed + move)
        //    {
        //        _rb.velocity = new Vector2(_maxSpeed * move, _rb.velocity.y);
        //    }
        //    else
        //    {
        //        moveDir = Vector2.right * move * _speedIncrement * Time.deltaTime;
        //        _rb.velocity += moveDir;
        //    }
        //}
        //else
        //{
        //    Vector2 velo = _rb.velocity;
        //    if (velo.Abs().x != 0)
        //    {
        //        if (velo.x > 0)
        //        {
        //            velo.x -= _speedIncrement * 2 * Time.deltaTime;
        //            if(velo.x < 0)
        //            {
        //                velo.x = 0;
        //            }
        //        }
        //        else
        //        {
        //            velo.x += _speedIncrement * 2 * Time.deltaTime;
        //            if (velo.x > 0)
        //            {
        //                velo.x = 0;
        //            }
        //        }
        //    }
        //    _rb.velocity = velo;

        //}
    }
    /// <summary>
    /// Checks for jump input and updates the physics system
    /// </summary>
    private void Jump()
    {
        if (_grounded)
        {
            _curJump = _jumpStrength;
        }
    }

    /// <summary>
    /// Called to move the player downward when not on the ground
    /// </summary>
    private void ApplyGravity()
    {

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
        if (collision.collider.CompareTag("Ground") && collision.collider.gameObject.transform.position.y < _ground.y)
        {
            _grounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Ground") && collision.collider.gameObject.transform.position.y < _ground.y)
        {
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


    // private fields

    private float _appliedForce = 0.0f;

    private Vector3 _ground = Vector3.zero;

    private direction dir = direction.RIGHT;
    
    private bool _grounded = false;

    private Rigidbody2D _rb;

    RaycastHit2D _hit;

    float _curJump = 0f;


    
    // enums

    enum direction { LEFT, RIGHT}


}

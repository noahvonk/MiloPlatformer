using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    // create public variables that describe Codey's jump movement
    // these are initialized but can be altered in the Inspector
    public float jumpStrength = 18f;
    public float fallMultiplier = 1.5f;
    public float jumpMultiplier = 1f;

    // create a reference for Codey's game object
    public GameObject Player;

    // create a reference to Codey's rigidbody
    private Rigidbody2D rigidBody;

    // create a reference to Codey's player controller
    // to see if Codey is on the ground or not
    private Controls controls;

    // store the references to Codey's rigidbody and player controller
    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        controls = Player.GetComponent<Controls>();
    }

    void Update()
    {
        // if the user presses the jump button (space)
        // and Codey is on the ground
        if (Input.GetButtonDown("Jump") && controls.onGround)
        {
            ActivateJump();
        }
    }
    // use FixedUpdate because we are calculating physics
    void FixedUpdate()
    {
        // each frame calculate the effects of gravity
        // and change the value of the Codey's velocity
        // if Codey's velocity is negative
        // then Codey is falling and we use the fallMultiplier
        if (rigidBody.velocity.y < 0)
        {
            rigidBody.velocity += Vector2.up * Physics.gravity.y * fallMultiplier * Time.fixedDeltaTime;
        }
        // and if Codey's velocity is positive
        // then Codey is jumping and we use the jumpMulitiplier
        else if (rigidBody.velocity.y > 0)
        {
            rigidBody.velocity += Vector2.up * Physics.gravity.y * jumpMultiplier * Time.fixedDeltaTime;
        }

    }

    void ActivateJump()
    {
        // and apply a velocity in the up direction (0, 1, 0) with
        // a magnitude of Codey's jump strength
        rigidBody.velocity = Vector3.up * jumpStrength;
        controls.onGround = false;
    }

    void SpringJump()
    {
        // and apply a velocity in the up direction (0, 1, 0) with
        // a magnitude of Codey's jump strength
        rigidBody.velocity = Vector3.up * (jumpStrength * 1.25f);
        controls.onGround = false;
    }

    void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.tag == "Ground" || collider.gameObject.tag == "Sink" || collider.gameObject.tag == "Block" || collider.gameObject.tag == "Switch" ||  collider.gameObject.tag == "Lift")
        {
            if (gameObject.transform.position.y >= collider.gameObject.transform.position.y)
            {
                //getting there, can still sidejump on upper half
                controls.onGround = true;
            }
        }

        if (collider.gameObject.tag == "Spring")
        {
            SpringJump();
        }
        if (collider.gameObject.tag == "Restore")
        {
            controls.onGround = true;
            collider.gameObject.SetActive(false);
            //set true after two seconds
        }
    }

    void OnCollisionExit2D(Collision2D collider)
    {
        if (collider.gameObject.tag == "Ground")
        {
            controls.onGround = false;
            //exception if you walk onto other things
        }
        if (collider.gameObject.tag == "Block")
        {
            controls.onGround = false;
        }
        if (collider.gameObject.tag == "Switch")
        {
            controls.onGround = false;
        }
        if (collider.gameObject.tag == "Lift")
        {
            controls.onGround = false;
        }
    }
}

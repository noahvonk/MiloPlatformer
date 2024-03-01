using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    private Rigidbody rigidBody;
    
    [SerializeField]
    public bool onGround;

    public bool touchingWall;

    public float centerOfCodeyToFeetDistance;

    // Start is called before the first frame update
    void Start()
    {
        onGround = true;
        touchingWall = false;
        rigidBody = GetComponent<Rigidbody>();
        centerOfCodeyToFeetDistance = GetComponent<Collider2D>().bounds.extents.y;
    }
}

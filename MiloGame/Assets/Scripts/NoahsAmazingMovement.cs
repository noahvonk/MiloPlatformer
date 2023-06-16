using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NoahsAmazingMovement : MonoBehaviour
{

    /// <summary>
    /// Updates all other functions each frame
    /// </summary>
    public void Update()
    {

    }

    /// <summary>
    /// Checks to see if input has been pressed every frame and updates the physics system
    /// </summary>
    private void Move()
    {

    }
    /// <summary>
    /// Checks for jump input and updates the physics system
    /// </summary>
    private void Jump()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // serialized fields

    [SerializeField]
    private float _speed = 10f;

    [SerializeField]
    private float _jumpStrength = 15f;

    [SerializeField]
    private float _gravity = 9.8f;

    [SerializeField]
    private bool _grounded = false;


    // private fields

    private float _appliedForce = 0.0f;


}

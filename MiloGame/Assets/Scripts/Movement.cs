using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Movement")]
    [Tooltip("Speed of movement")]
    public float speed;

    public enum MovementType
    {
        AllDirections,
        HorizontalOnly,
        VerticalOnly
    }

    [SerializeField]
    private MovementType movementType = MovementType.HorizontalOnly;

    void Start()
    {

    }

    void FixedUpdate()
    {
        // get the user input values for both axes
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

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

        RaycastHit2D hit;
        hit = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector3.right), 1f);
        if (hit.collider && !hit.collider.CompareTag("Player") && !hit.collider.CompareTag("Room"))
        {
            Debug.Log(hit.collider.transform.position.ToString());
            Debug.DrawLine(transform.position, hit.transform.position, Color.red);
            GetComponent<Controls>().touchingWall = true;
        }
        else
        {
            Debug.DrawLine(transform.position, transform.position + transform.TransformDirection(Vector3.right), Color.white);
            GetComponent<Controls>().touchingWall = false;
        }
    }
}

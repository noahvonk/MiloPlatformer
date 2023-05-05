using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lift : MonoBehaviour
{
    [SerializeField]
    private float bottomHeight;
    [SerializeField]
    private float topHeight;
    [SerializeField]
    private float speed = 0.02f;
    [SerializeField]
    private bool moveUp;

    private bool playerStick = false;

    // Start is called before the first frame update
    void Start()
    {
        moveUp = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= bottomHeight)
        {
            moveUp = true;
        }
        if (transform.position.y >= topHeight)
        {
            moveUp = false;
        }

        if (moveUp)
        {
            transform.position += new Vector3(0, speed, 0);
        }
        else
        {
            transform.position += new Vector3(0, -speed, 0);
        }

        if (playerStick && !moveUp)
        {
            GameManager.Instance.Player.transform.position += new Vector3(0, -speed, 0);
        } 
        else if (playerStick && moveUp)
        {
            GameManager.Instance.Player.transform.position += new Vector3(0, speed, 0);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && collision.gameObject.transform.position.y >= gameObject.transform.position.y)
        {
            
            playerStick = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            playerStick = false;
        }
    }
}

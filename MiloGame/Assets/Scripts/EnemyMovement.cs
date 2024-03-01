using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private float followSpeed;
    private const int gravity = 1;
    private bool touchingWall;
    private bool touchingLeftWall;
    private bool touchingRightWall;
    [SerializeField] private float LeftWallPosition;
    [SerializeField] private float RightWallPosition;
    // Start is called before the first frame update
    void Start()
    {
        touchingWall = false;
        touchingLeftWall = false;
        touchingRightWall = false;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = Vector2.MoveTowards(transform.position, new Vector2(GameManager.Instance.Player.transform.position.x, transform.position.y), followSpeed * Time.deltaTime);
        //transform.position = Vector2.MoveTowards(transform.position, new Vector2(GameManager.Instance.Player.transform.position.x, transform.position.y), followSpeed * Time.deltaTime);
        //transform.position = Vector2.MoveTowards(transform.position, GameManager.Instance.Player.transform.position, followSpeed * Time.deltaTime);
        if (!touchingWall)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(GameManager.Instance.Player.transform.position.x, transform.position.y), followSpeed * Time.deltaTime);
        }
        else if (touchingLeftWall && GameManager.Instance.Player.transform.position.x >= LeftWallPosition)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(GameManager.Instance.Player.transform.position.x, transform.position.y), followSpeed * Time.deltaTime);
        }
        else if (touchingRightWall && GameManager.Instance.Player.transform.position.x <= RightWallPosition)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(GameManager.Instance.Player.transform.position.x, transform.position.y), followSpeed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "LeftWall")
        {
            touchingWall = true;
            touchingLeftWall = true;
            touchingRightWall = false;
        }
        if (collision.gameObject.tag == "RightWall")
        {
            touchingWall = true;
            touchingLeftWall = false;
            touchingRightWall = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "LeftWall")
        {
            touchingWall = false;
            touchingLeftWall = false;
            touchingRightWall = false;
        }
        if (collision.gameObject.tag == "RightWall")
        {
            touchingWall = false;
            touchingLeftWall = false;
            touchingRightWall = false;
        }
    }
}

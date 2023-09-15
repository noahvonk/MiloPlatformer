using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private float followSpeed;
    private const int gravity = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = Vector2.MoveTowards(transform.position, new Vector2(GameManager.Instance.Player.transform.position.x, transform.position.y), followSpeed * Time.deltaTime);
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(GameManager.Instance.Player.transform.position.x, transform.position.y), followSpeed * Time.deltaTime);
        //transform.position = Vector2.MoveTowards(transform.position, GameManager.Instance.Player.transform.position, followSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {

        }
        else
        {
            Debug.Log("This should work");
            //transform.position = Vector2.MoveTowards(transform.position, GameManager.Instance.Player.transform.position, followSpeed * Time.deltaTime);
            //transform.position = Vector2.MoveTowards(transform.position, new Vector2(GameManager.Instance.Player.transform.position.x, transform.position.y), followSpeed * Time.deltaTime);
        }
    }
}

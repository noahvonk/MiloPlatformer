using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sinking : MonoBehaviour
{
    public GameObject groundToucher;
    
    [SerializeField]
    private bool sinking = false;

    [SerializeField]
    private float speed = 0.02f;

    [SerializeField]
    private float maxHeight = -2.26154f;
    public bool onGround;

    void Start()
    {
        onGround = groundToucher.GetComponent<Controls>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (sinking)
        {
            //doesn't work after dying
            //number too high
            transform.position += new Vector3(0, -speed, 0);
            //doesn't move player after death
            GameManager.Instance.Player.transform.position += new Vector3(0, -speed, 0);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && collision.transform.position.y >= transform.position.y)
        {
            sinking = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        sinking = false;
    }
}

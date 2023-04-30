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
            //doesn't move player after death
            GameManager.Instance.Player.transform.position += new Vector3(0, -speed, 0);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sinking : MonoBehaviour
{
    private bool sinking = false;

    // Update is called once per frame
    void Update()
    {
        if (sinking)
        {
            //number too high
            transform.position += new Vector3 (0, -1, 0);
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

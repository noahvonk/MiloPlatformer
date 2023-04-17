using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sinking : MonoBehaviour
{
    [SerializeField]
    private bool sinking = false;

    // Update is called once per frame
    void Update()
    {
        if (sinking)
        {
            //number too high
            transform.position += new Vector3 (0.0f, -0.01f, 0.0f);
        }
        else if (transform.position.y >= -2.26154)
        {

        }
        else if (sinking == false)
        {
            //Problem is when the platform moves, you leave contact, making sinking false
            //Debug.Log("Miracles Happen");
            //transform.position += new Vector3 (0.0f, 0.01f, 0.0f);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waterfall : MonoBehaviour
{

    void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.tag == "Waterfall")
        {
            //I would need to increase the fall speed but Noah confuses me
        }
    }

}

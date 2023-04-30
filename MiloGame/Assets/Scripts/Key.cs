using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public bool hasKey;

    // Start is called before the first frame update
    void Start()
    {
        hasKey = false;

        GameObject[] gameObjects;
        gameObjects = GameObject.FindGameObjectsWithTag("Key Piece");
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] gameObjects;
        gameObjects = GameObject.FindGameObjectsWithTag("Key Piece");

        if (gameObjects.Length == 0)
        {
            hasKey = true;
            GameManager.Instance.Lock2.SetActive(false);
        }
    }

    void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.tag == "Key")
        {
            hasKey = true;
            collider.gameObject.SetActive(false);
            GameManager.Instance.Lock.SetActive(false);
        }
        if (collider.gameObject.tag == "Key Piece")
        {
            collider.gameObject.SetActive(false);
        }
    }
}

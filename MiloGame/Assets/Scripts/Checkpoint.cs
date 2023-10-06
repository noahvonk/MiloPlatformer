using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
public class Checkpoint : MonoBehaviour
{
    public GameObject GameManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D (Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Checkpoint 1 reached");
            Vector3 newCheckpoint = transform.position;
            GameManager.GetComponent<GameManager>().checkPoint = newCheckpoint;
            Debug.Log("New Coordinates are: " + newCheckpoint);
        }
    }
}

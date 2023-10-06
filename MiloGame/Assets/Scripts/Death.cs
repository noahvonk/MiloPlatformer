using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Death : MonoBehaviour
{
    public GameObject GameManager;
    void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.tag == "Death" || collider.gameObject.tag == "Enemy")
        {
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            GameManager.GetComponent<GameManager>().moveToCheckPoint();
        }
    }
}

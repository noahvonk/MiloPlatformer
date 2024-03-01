using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    private void OnCollisionEnter2D (Collision2D collision)
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (collision.gameObject.tag == "Player")
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
    }

}

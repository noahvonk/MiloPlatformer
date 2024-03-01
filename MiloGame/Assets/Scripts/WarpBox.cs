using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpBox : MonoBehaviour
{
    [SerializeField] public float Warp_x;
    [SerializeField] public float Warp_y;
    public GameObject GameManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D (Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Yes");
            Vector3 newWarpbox;
            newWarpbox = Vector3(Warp_x, Warp_y, 0);
            GameManager.GetComponent<GameManager>().warpBoxCoordinates = newWarpbox;
            GameManager.GetComponent<GameManager>().goToWarpbox();
        }
        Debug.Log("Almost Yes");
    }
}

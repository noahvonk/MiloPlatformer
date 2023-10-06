using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject Player;

    public Vector3 checkPoint = new Vector3(0, 0, 0);
    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
            Instance = this;
        } else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void moveToCheckPoint()
    {
        Player.transform.position = checkPoint;
    }
}

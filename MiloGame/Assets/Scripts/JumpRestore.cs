using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpRestore : MonoBehaviour
{
    private bool used = false;
    public float waitTime = 3;
    
    public NoahsAmazingMovement NoahsAmazingMovement;
    // add a gameobject variable for the object to be deactivated
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if (!used && !NoahsAmazingMovement.grounded)
                NoahsAmazingMovement.restored = true;
                StartCoroutine(RespawnTime());
                //deactivate gameobject
        }

    }

    private IEnumerator RespawnTime ()
    {
        yield return new WaitForSeconds (waitTime);
        used = false;
        //reactivate gameobject
    }

}

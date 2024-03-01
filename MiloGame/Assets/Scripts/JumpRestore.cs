using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpRestore : MonoBehaviour
{
    public bool used = false;
    public float waitTime = 3;
    // add a gameobject variable for the object to be deactivated
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator RespawnTime ()
    {
        yield return new WaitForSeconds (waitTime);
        used = false;
        //reactivate gameobject
        gameObject.SetActive(true);
    }

}

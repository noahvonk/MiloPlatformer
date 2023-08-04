using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSwitch : MonoBehaviour
{
    public Color FireColor;
    public Color IceColor;
    public Color BlockOffColor;

    public bool FireOn;

    public List<GameObject> FireBlock;
    public List<GameObject> IceBlock;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().color = IceColor;
        FireOn = false;
        foreach(GameObject block in IceBlock)
        {
            block.GetComponent<SpriteRenderer>().color = IceColor;
        }

        foreach (GameObject block in FireBlock)
        {
            block.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.tag == "Player" && transform.position.y >= collider.transform.position.y)
        {
            if (FireOn)
            {
                FireOn = false;
                gameObject.GetComponent<SpriteRenderer>().color = IceColor;
                foreach (GameObject block in FireBlock)
                {
                    block.SetActive(false);
                }
                foreach (GameObject block in IceBlock)
                {
                    block.SetActive(true);
                }
            }
            else
            {
                FireOn = true;
                gameObject.GetComponent<SpriteRenderer>().color = FireColor;
                foreach (GameObject block in FireBlock)
                {
                    block.SetActive(true);
                }
                foreach (GameObject block in IceBlock)
                {
                    block.SetActive(false);
                }
            }
        }
    }
}

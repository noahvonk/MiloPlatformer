using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSwitch : MonoBehaviour
{
    public Color FireColor;
    public Color IceColor;
    public Color BlockOffColor;

    [SerializeField]
    private bool FireOn;

    public GameObject FireBlock;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().color = FireColor;
        FireOn = true;
        FireBlock.GetComponent<SpriteRenderer>().color = FireColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            if (FireOn)
            {
                FireOn = false;
                gameObject.GetComponent<SpriteRenderer>().color = IceColor;
                FireBlock.GetComponent<SpriteRenderer>().color = BlockOffColor;
            }
            else
            {
                FireOn = true;
                gameObject.GetComponent<SpriteRenderer>().color = FireColor;
                FireBlock.GetComponent<SpriteRenderer>().color = FireColor;
            }
        }
    }
}

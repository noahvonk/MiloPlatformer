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
    public GameObject IceBlock;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().color = FireColor;
        FireOn = true;
        FireBlock.GetComponent<SpriteRenderer>().color = FireColor;
        IceBlock.SetActive(false);
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
                FireBlock.SetActive(false);
                IceBlock.SetActive(true);
            }
            else
            {
                FireOn = true;
                gameObject.GetComponent<SpriteRenderer>().color = FireColor;
                FireBlock.SetActive(true);
                IceBlock.SetActive(false);
            }
        }
    }
}

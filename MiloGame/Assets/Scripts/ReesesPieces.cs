using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReesesPieces : MonoBehaviour
{
    public bool hasKey;

    public GameObject LockA;
    public GameObject LockB;
    public GameObject LockC;

    // Start is called before the first frame update
    void Start()
    {
        hasKey = false;

        GameObject[] piecesA;
        piecesA = GameObject.FindGameObjectsWithTag("Key Piece A");

        GameObject[] piecesB;
        piecesB = GameObject.FindGameObjectsWithTag("Key Piece B");
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] piecesA;
        piecesA = GameObject.FindGameObjectsWithTag("Key Piece A");

        GameObject[] piecesB;
        piecesB = GameObject.FindGameObjectsWithTag("Key Piece B");

        if (piecesA.Length == 0)
        {
            hasKey = true;
            LockA.SetActive(false);
        }

        if (piecesB.Length == 0)
        {
            hasKey = true;
            LockB.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Key Piece A")
        {
            //var keyPiece = collider.transform.GetComponent<JumpRestore>(); 
            collider.gameObject.SetActive(false);
        }
        if (collider.gameObject.tag == "Key Piece B")
        {
            collider.gameObject.SetActive(false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class ContactReporter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        _collider = gameObject.GetOrAddComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Checks for collisions and invokes callbacks
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Entererd");
        foreach(string s in _comparatorTags)
        {
            if (collision.CompareTag(s))
            {
                foreach(ContactReport a in _enteredReportBacks)
                {
                    a?.Invoke(collision);
                }
            }
        }
    }

    /// <summary>
    /// On exit, invoke all listeners
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        foreach (string s in _comparatorTags)
        {
            if (collision.CompareTag(s))
            {
                foreach (ContactReport a in _exitedReportBacks)
                {
                    a?.Invoke(collision);
                }
            }
        }
    }

    [SerializeField] private List<string> _comparatorTags = new();

    public delegate void ContactReport(Collider2D collider);
    public List<ContactReport?> _enteredReportBacks = new();
    public List<ContactReport?> _exitedReportBacks = new();
    private Collider2D _collider;
}

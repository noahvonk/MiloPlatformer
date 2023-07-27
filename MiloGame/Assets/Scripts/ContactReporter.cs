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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach(string s in _comparatorTags)
        {
            if (collision.CompareTag(s))
            {
                foreach(UnityAction a in _enteredReportBacks)
                {
                    a?.Invoke();
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        foreach (string s in _comparatorTags)
        {
            if (collision.CompareTag(s))
            {
                foreach (UnityAction a in _exitedReportBacks)
                {
                    a?.Invoke();
                }
            }
        }
    }

    [SerializeField] private List<string> _comparatorTags = new();

    public List<UnityAction>? _enteredReportBacks = new();
    public List<UnityAction>? _exitedReportBacks = new();
    private Collider2D _collider;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SwitchController : MonoBehaviour
{
    public Color FireColor;
    public Color IceColor;
    // Start is called before the first frame update
    void Start()
    {
        GetChildrenAndUpdateListeners();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void GetChildren()
    {
        foreach (Transform child in transform)
        {
            //print("Foreach loop: " + child);
            _contactList.Add(child.GetComponentInChildren<ContactReporter>());
        }
    }

    private void GetChildrenAndUpdateListeners()
    {
        foreach (Transform child in transform)
        {
            //print("Foreach loop: " + child);
            var contactReporter = child.GetComponentInChildren<ContactReporter>();
            _contactList.Add(contactReporter);
            contactReporter._enteredReportBacks.Add(FlipBlockActivity);
        }

        
    }

    private void OnDisable()
    {
        foreach (ContactReporter contactReporter in _contactList)
        {
            contactReporter._enteredReportBacks.Clear();
        }

        //OnEnterTrigger -= FlipBlockActivity;
    }


    private void FlipBlockActivity(Collider2D collision)
    {
        foreach (ContactReporter ct in _contactList)
        {
            Debug.Log("flipColor");
            SpriteRenderer s = ct.GetComponentInParent<SpriteRenderer>();
            if (s.color == FireColor)
            {
                s.color = IceColor;
            }
            else
            {
                s.color = FireColor;
            }
        }
        foreach (GameObject obj in _blocks)
        {
            obj.SetActive(!obj.activeSelf);
            Debug.Log("BlockFlipped");
        }
        Debug.Log("Contact Reporter List Size: " + _contactList.Count);
    }
    [SerializeField]
    private List<ContactReporter> _contactList = new();
    [SerializeField]
    private List<GameObject> _blocks = new();

    UnityAction OnEnterTrigger;
    UnityAction OnExitTrigger;
}

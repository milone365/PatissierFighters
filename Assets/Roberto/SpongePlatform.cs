using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpongePlatform : MonoBehaviour
{
    public Vector3 border = new Vector3(3, 3, 3);
    bool go = true;
    [SerializeField]
    float moveSpeed=5;
    Vector3 startPos;
    Vector3 destination;
    [SerializeField]
    Transform endpoint=null;
    [SerializeField]
    float distanceCheck = 1;
    void Start()
    {
        startPos = transform.position;
        destination = endpoint.position;
    }

    // Update is called once per frame
    void Update()
    {
        Collider[] hits = Physics.OverlapBox(transform.position, border);
        foreach(var h in hits)
        {
            if (h.GetComponent<Crab>())
            {
                if (go)
                {
                    go = false;
                }
            }
        }
        
        float distance = Vector3.Distance(transform.position, destination);
        if (distance <= distanceCheck)
        {
            go = !go;
        }
        if (go)
        {
            destination = endpoint.position;
        }
        else
        {
            destination = startPos;
        }
        transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);
    }
}

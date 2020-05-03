using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicRay : MonoBehaviour
{
    [SerializeField]
    GameObject particle = null;
    Vector3 destination;
    [SerializeField]
    float speed = 20;
   
    //移動
    void Update()
    {
        if (destination == null) return;
        transform.position = Vector3.MoveTowards(transform.position, destination, speed*Time.deltaTime);
    }
    //目的地ももらう
   public void setDestination(Vector3 d)
    {
        destination = d;
    }

    //あたり安定
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == StaticStrings.player)
        {
            if (particle != null)
                Instantiate(particle, transform.position, transform.rotation);
            Destroy(gameObject);
        } 
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//destroy eney point
public class ADV_destroyZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "AI")
        {
            Destroy(other.gameObject);
        }
    }
}

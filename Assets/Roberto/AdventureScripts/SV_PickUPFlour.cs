using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SV_PickUPFlour : MonoBehaviour
{
    [SerializeField]
    float value = 10;
    
    //タワーを回復
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == StaticStrings.player)
        {
            SV_Tower tower = FindObjectOfType<SV_Tower>();
            tower.Healing(value);
            Destroy(gameObject);
        }
    }
}

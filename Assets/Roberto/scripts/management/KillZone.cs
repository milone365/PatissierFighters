using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    //死とリスポーン
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == StaticStrings.player || other.tag == StaticStrings.helper)
        {
            HealthManager h = other.GetComponent<HealthManager>();
            h.gameObject.SetActive(false);
            h.teleportToSpawnPoint();
            h.gameObject.SetActive(true);
        }
    }
}

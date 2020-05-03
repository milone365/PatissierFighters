using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADV_KillZone : MonoBehaviour
{
    Vector3 spawnPosition = Vector3.zero;
    ADV_Player player;
    void Start()
    {
        player = FindObjectOfType<ADV_Player>();
        spawnPosition = player.transform.position;
    }

    //respawn
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == StaticStrings.player)
        {
            IHealth h = other.GetComponent<IHealth>();
            h.takeDamage(1);
            other.transform.position = spawnPosition;
        }
        if (other.tag == "AI")
        {
            Destroy(other.gameObject);
        }
    }
}

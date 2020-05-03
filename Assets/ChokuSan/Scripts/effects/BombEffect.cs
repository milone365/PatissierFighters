using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombEffect : MonoBehaviour
{
    public GameObject player;

    public void BombShoot(Vector3 dir)
    {
        GetComponent<Rigidbody>().AddForce(dir);
    }
    private void OnCollisionEnter(Collision player)
    {
        if (player.gameObject.CompareTag("bomb"))
        {
            BombShoot(new Vector3(0, 200, 200));
        }
    }
}

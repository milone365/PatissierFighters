using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletPickUP : MonoBehaviour
{
   [SerializeField]
    WPbullet b=null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == StaticStrings.player)
        {
            B_Player player = other.GetComponent<B_Player>();
            if ( player== null) return;

            player.status.getCurrentweapon.changeBullet(b);
            Destroy(gameObject);
        }
    }
}

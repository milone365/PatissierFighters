using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADV_Hurt : MonoBehaviour
{
    //プレイヤーにダメージをつける
    [SerializeField]
    int damage = 1;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == StaticStrings.player)
        {
            IHealth health = other.GetComponent<IHealth>();
            if (health != null)
            {
                health.takeDamage(damage);
            }
        }
        if (other.GetComponent<Adv_Drive>())
        {
            other.GetComponent<Adv_Drive>().blinking();
        }
    }
}

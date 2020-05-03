using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PancakeBomb : MonoBehaviour
{
    //ダメージを付ける
    private void OnTriggerEnter(Collider other)
    {
       
        if (other.tag == StaticStrings.player)
        {
            IHealth health = other.GetComponent<IHealth>();
            health.takeDamage(1);
            if (EffectDirector.instance != null)
            {
                EffectDirector.instance.playInPlace(transform.position, StaticStrings.bomb);
            }
            Destroy(gameObject);
        }
        if (other.tag == "Boss")
        {
            IEnemy health = other.GetComponent<IEnemy>();
            health.takeDamage(10);
            if (EffectDirector.instance != null)
            {
                EffectDirector.instance.playInPlace(transform.position, StaticStrings.bomb);
            }
            Destroy(gameObject);
        }
    }
}

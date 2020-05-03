using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADV_EnemyHealth : MonoBehaviour, IEnemy
{

    //hp manager

    [SerializeField]
    int score = 50;
    [SerializeField]
    float HP = 30;
    bool death = false;
    public string EffectName = "";
    public void Healing(float heal)
    {
        throw new System.NotImplementedException();
    }

    public void takeDamage(float damageToTake)
    {
        if (death) return;
        HP -= damageToTake;
        if (HP <= 0)
        {
            death = true;
            if (EffectDirector.instance != null)
            {
                if(EffectName!="")
                EffectDirector.instance.EffectAndPopup(transform.position, EffectName, score);
            }
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADV_Destroyable : MonoBehaviour, IShotable
{
    [SerializeField]
    float HP = 3;
    bool death = false;
    public string EffectName = "PUFF";
    [SerializeField]
    int points = 100;

    //damageable object, takedamage function
    public void interact(Vector3 hitPos)
    {
        if (death) return;
        HP --;
        if (HP <= 0)
        {
            death = true;
            if (EffectDirector.instance != null)
            {
                if (EffectName != "")
                EffectDirector.instance.EffectAndPopup(hitPos, EffectName, points);
            }
            Destroy(gameObject);
        }
    }
}

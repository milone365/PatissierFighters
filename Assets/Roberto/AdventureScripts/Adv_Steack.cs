using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Adv_Steack : MonoBehaviour,IShotable
{
    ParticleSystem smoke;
    bool ended;
    int hp=2;
    
    //煙
    public void interact(Vector3 hitPos)
    {
       if (ended) return;
        hp--;
        if (hp <= 0)
        {
            ended = true;
            smoke.Play();
            smoke.loop = true;
            Destroy(gameObject);
            
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        smoke = GetComponentInParent<ParticleSystem>();
    }
}

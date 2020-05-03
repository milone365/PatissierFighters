using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADV_staticEnemy : MonoBehaviour,IEnemy
{
    [SerializeField]
    int hp = 3;
    ADV_Player p;
    [SerializeField]
    float atkDelay = 3;
    float atkCounter = 3;
    bool isDeath;
    [SerializeField]
    int points = 120;
    public void Healing(float heal)
    {
        throw new System.NotImplementedException();
    }

  
    public void takeDamage(float damageToTake)
    {
        if (isDeath) return;
        hp--;
        if (hp <= 0)
        {
            isDeath = true;
            EffectDirector.instance.EffectAndPopup(transform.position, "BOMB", points);
            Destroy(gameObject);
        }
    }

    void Start()
    {
        p = FindObjectOfType<ADV_Player>();
        ADV_TossPancakeBall pball = GetComponent<ADV_TossPancakeBall>();
        if (pball == null) return;
       pball.setPlayer(p.transform);
    }

    // rotate and attack
    void Update()
    {
        if (p == null) return;
        transform.LookAt(p.transform,Vector3.up);
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        atkCounter -= Time.deltaTime;
        if (atkCounter <= 0)
        {
            atkCounter = atkDelay;
            GetComponent<Animator>().SetTrigger("ATK");
        }
    }
}

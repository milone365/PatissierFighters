using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    protected Team tm;
    B_Player player = null;
    string atk;
    private void Start()
    {
        tm = GetComponentInParent<ITeam>().getTeam();
    }

    public virtual void Initialize(B_Player p)
    {
        player = p;
    }
    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag==StaticStrings.helper|| other.tag == StaticStrings.player
            || other.tag == StaticStrings.tower)
        {
            Team enemyteam = other.GetComponent<ITeam>().getTeam();
            if (enemyteam != tm)
            {
                IAttack<Transform> attacker = GetComponentInParent<IAttack<Transform>>();
                if (attacker == null) return;
                attacker.attack(other.transform);
                int num = Random.Range(1, 3);
                
                switch (num)
                {
                    case 1:atk = StaticStrings.HELPERMELEE;
                        break;
                    case 2: atk = StaticStrings.MELEE2;
                        break;
                    default:
                        atk = StaticStrings.HELPERMELEE;
                        break;
                }
                EffectDirector.instance.playInPlace(transform.position, atk);
            }
                
           
        }
    }
}

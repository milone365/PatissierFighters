using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace td
{
    public class TD_Attack_melee : MonoBehaviour
    {
        
        float atk = 1;
        public void reciveStatistic(float _atk)
        {
            atk = _atk;
        }
       
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == StaticStrings.player || other.tag == StaticStrings.AI)
            {
                TD_HealthManager h = other.GetComponent<TD_HealthManager>();
                if (h != null)
                {
                    h.takeDamage(atk);
                }
            }
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace td
{
    public class TD_PointZone : MonoBehaviour
    {
        //ポイントをあげる
        public Team tm;
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == StaticStrings.pancake)
            {
                if (TD_PancakeBall.instance.getPossessor() != tm)
                {
                    TD_PancakeBall.instance.givePoint();
                }
               
            }
        }
    }

}

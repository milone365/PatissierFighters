using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PancakePickUP : MonoBehaviour
{

    bool cantake = false;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Helper")
        {
            if (!cantake) return;
            KitchenHelper helper= other.GetComponent<KitchenHelper>();
            //段階によって取られます
            if (other.GetComponent<FiniStateMachine>().currentState.stateType != stateType.cake) return;
           
            if (helper.haveMaterial) return;
            //flag->true
            helper.reciveMaterial();
            //リストから削除
            PancakeSpawner.instance.RemovePancake(this.transform);
            Destroy(gameObject);
        }
       
    }

    public void Cantake()
    {
        cantake = true;
    }
}

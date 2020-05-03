using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toaster : MonoBehaviour,IShotable
{
    
    [SerializeField]
    Transform levelUp=null;
    bool active = false;

    //移動
    public void interact(Vector3 hitPos)
    {
        if (active) return;
        active = true;

        transform.position = levelUp.transform.position;
        
    }

   
    
}

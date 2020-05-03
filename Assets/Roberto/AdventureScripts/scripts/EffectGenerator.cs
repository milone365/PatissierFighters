using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectGenerator : MonoBehaviour
{
    [SerializeField]
    Transform[] points = null;
    
    //好きな場所にものをスポーンする
    public void generateEffectAtPoint()
    {
        if (EffectDirector.instance != null)
        {
            foreach(var p in points)
            {
                EffectDirector.instance.spawnInPlace(p.position,0);
            }
            
        }
    }
}

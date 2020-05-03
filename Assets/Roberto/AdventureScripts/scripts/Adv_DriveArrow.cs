using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Adv_DriveArrow : MonoBehaviour,IShotable
{
   public Arrow arrowVerse;
    [SerializeField]
    Adv_Drive veicle=null;
    
    //方向を送る
    public void interact(Vector3 hitpos)
    {
        if (veicle == null) return;
        veicle.MoveTo(arrowVerse);
    }
}
public enum Arrow
{
    right,
    left,
    up,
    down,
    front,
    back
}
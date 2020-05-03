using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TD_canvas : MonoBehaviour
{
    [SerializeField]
    Image crossAir = null;
    public void activeCrossAir(bool v)
    {
        crossAir.enabled = v;
    }
   
}

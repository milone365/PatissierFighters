using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using td;


public class TD_healthBar : MonoBehaviour
{
    [SerializeField]
    Slider healthslider = null;
    [SerializeField]
    Text healthtext = null;
    TD_HealthManager h = null;

    private void Start()
    {
        h = GetComponentInParent<TD_HealthManager>();
        healthslider.maxValue = h.getmaxHealth();
    }
    //ｈｐバーアップデート
    private void Update()
    {
        healthslider.value=h.getHealth ();
        healthtext.text = h.getHealth() + "/" + h.getmaxHealth();
    }

}

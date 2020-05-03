using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ADV_BossBar : MonoBehaviour
{
    
    Slider hpbar;
    [SerializeField]
    Image Icon=null;
    Image[] allImages;
    void Start()
    {
        hpbar = GetComponentInChildren<Slider>();
        allImages = GetComponentsInChildren<Image>();
    }
    //bar update
    public void takeDamage(float hp)
    {
        hpbar.value = hp;
    }
    //active all images,value set
    public void activeBar(Sprite s, float maxHp,float hp)
    {
        Icon.sprite = s;
        Icon.preserveAspect = true;
        foreach(var i in allImages)
        {
            i.enabled = true;
        }
        hpbar.maxValue = maxHp;
        hpbar.value = hp;
    }
    //deactive all images
    public void deactivateBar()
    {
        foreach (var i in allImages)
        {
            i.enabled = false;
        }
    }
}

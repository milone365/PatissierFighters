using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Adv_activeSE : MonoBehaviour
{
    //SE　active
    public string sound="";
    public void playSE()
    {
        if (Soundmanager.instance != null)
        {
            Soundmanager.instance.PlaySeByName(sound);
        }
    }
}

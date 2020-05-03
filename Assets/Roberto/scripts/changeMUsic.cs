using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeMUsic : MonoBehaviour
{
    //音楽を変える
    [SerializeField]
    string musicName = "";
    void Start()
    {
        if (Soundmanager.instance == null) return;
        Soundmanager.instance.StopBgm();
        Soundmanager.instance.PlayBgmByName(musicName);
    }

   
}

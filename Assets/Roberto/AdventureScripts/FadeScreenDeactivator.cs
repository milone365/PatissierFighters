using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeScreenDeactivator : MonoBehaviour
{
    // fadescreenを消す
    void Start()
    {
        GameObject fadeScreen;
        fadeScreen = GameObject.Find("FadeScreen");
        if (fadeScreen != null)
        {
            fadeScreen.GetComponent<Animation>().Play("unfade");
        }
    }

}

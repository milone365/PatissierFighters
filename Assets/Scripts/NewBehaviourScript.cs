using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    void Start()
    {
        for (int i = 0; i < Display.displays.Length; i++)
            Display.displays[i].Activate(1920, 1080, 60);
    }
}

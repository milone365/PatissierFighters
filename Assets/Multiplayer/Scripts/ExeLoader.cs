using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class ExeLoader : MonoBehaviour
{
    public string linker;
   void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Process.Start(linker);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraActivator : MonoBehaviour
{
    [SerializeField]
    GameObject cam1 = null, cam2 = null;
    //deactive camera
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == StaticStrings.player)
        {
            if (cam1.activeInHierarchy)
            {
                cam2.SetActive(true);
                cam1.SetActive(false);
            }
            else
            {
                cam2.SetActive(false);
                cam1.SetActive(true);
            }
        }
    }
}

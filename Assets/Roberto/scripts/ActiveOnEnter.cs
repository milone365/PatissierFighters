using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveOnEnter : MonoBehaviour
{
    [SerializeField]
    GameObject gm=null;
    [SerializeField]
    GameObject deactiveObj = null;
    [SerializeField]
    bool activeOnEnable = false;
    [SerializeField]
    float activateTime = 0.2f;
    [SerializeField]
    bool destroy = false;


    //始まるときの活性
    private void Start()
    {
        if (activeOnEnable)
        {
            Invoke("ACTIVATE", activateTime);
        }
    }
    //triggerの活性
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == StaticStrings.player)
        {
            Invoke("ACTIVATE", activateTime);
        }
      
    }
    void ACTIVATE()
    {
        gm.SetActive(true);
        if (deactiveObj != null)
        {
            if (destroy)
            {
                Destroy(deactiveObj);
            }
            else
            {
                deactiveObj.SetActive(false);
            }
            
        }
        gameObject.SetActive(false);
    }
}

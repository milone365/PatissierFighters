using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.UI;
using td;

public class ControlSceneTimer : MonoBehaviour
{
    [SerializeField]
    Text timetext = null;
    public float time;
    bool isLoaded = false;
    

    private void Update()
    {
        if(!isLoaded)
        time -= Time.deltaTime;
        if (time <= 0)
        {
             isLoaded = true;
            Destroy(gameObject);
            
        }
        if (timetext != null)
        {
            timetext.text = "" + (int)time;
        }
    }

}

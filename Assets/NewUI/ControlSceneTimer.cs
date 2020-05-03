using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.UI;
using M;
using td;

public class ControlSceneTimer : NetworkBehaviour
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
            NetWorkCleaner.instance.removeObjectFromServer(this.gameObject);
            Destroy(gameObject);
            
        }
        if (timetext != null)
        {
            timetext.text = "" + (int)time;
        }
    }

}

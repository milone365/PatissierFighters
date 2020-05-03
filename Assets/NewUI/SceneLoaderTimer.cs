using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderTimer : MonoBehaviour {
    public string scenename;
    public float time;
    bool isLoaded = false;

    private void Update()
    {
        time -= Time.deltaTime;
        if(time <= 0)
        {
            if (!isLoaded)
            {
                isLoaded = true;
                Load(scenename);
            }
           
        }
        if (Input.GetButtonDown(StaticStrings.X_key))
        {
             Load(scenename);
         }
    }

    public void Load(string scenename)
    {
        StartCoroutine(loadGameCo());
    }

    IEnumerator loadGameCo()
    {
        GameObject fadescreen = GameObject.Find("FadeScreen");
        if (fadescreen != null)
        {
            fadescreen.GetComponent<Animation>().Play("fade");
        }
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene(scenename);
    }

}

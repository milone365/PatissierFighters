using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public bool active_X=false;
    public string scenename;
    SaveGame sg;
    private void Start()
    {
        sg = FindObjectOfType<SaveGame>();
    }
    private void Update()
    {
        if (active_X)
        {
            if (Input.GetButtonDown(StaticStrings.X_key))
            {
                Load(scenename);
            }
        }
    }
    public void Load(string scenename)
    {
        Soundmanager s = FindObjectOfType<Soundmanager>();
        if (s != null)
        {
            s.PlaySeByName(StaticStrings.EnterSoundEffect);
        }

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
    public void loadGAME()
    {
        if (sg != null)
        {
            sg.saveGame();
        }
        StartCoroutine(loadGameCo());
    }

}

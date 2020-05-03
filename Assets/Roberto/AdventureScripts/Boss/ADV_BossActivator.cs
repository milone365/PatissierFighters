using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADV_BossActivator : MonoBehaviour
{
    ADV_Suicide_Robot[] robots;
    bool isPassed = false;
    SceneLoader loader;
    void Start()
    {
        robots = FindObjectsOfType<ADV_Suicide_Robot>();
        loader = FindObjectOfType<SceneLoader>();
    }

    //aniamtion play,sound effect
    IEnumerator suicideCo()
    {
        foreach (var r in robots)
        {
            r.makeSuicide();
            yield return new WaitForSeconds(1.5f);
        }
        yield return new WaitForSeconds(2f);
        loader.Load("ADV_BossBattle");
        if (Soundmanager.instance != null)
            Soundmanager.instance.PlaySeByName("robotLaugh");
    }
    //active animation event
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == StaticStrings.player)
        {
            if (isPassed) return;
            isPassed = true;
            StartCoroutine(suicideCo());
        }
    }
}

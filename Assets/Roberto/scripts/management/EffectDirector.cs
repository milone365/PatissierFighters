using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectDirector : MonoBehaviour
{
    public static EffectDirector instance;
    Dictionary<string,Transform> particles = new Dictionary<string,Transform>();
    [SerializeField]
    ParticleSystem[] effects=null;
    public GameObject[] objToSpawn=null;
    GameObject popUp = null;
    ADV_CrossAir crossAir;
    ADV_UIManager uiManager=null;
    Text pointText;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
       foreach(var item in effects)
        {
           particles.Add(item.gameObject.name, item.transform);
        }
        popUp = GameObject.Find("PopUp");
        if(popUp!=null)
        pointText = popUp.GetComponentInChildren<Text>();

    }
    public void stopParticle(string name)
    {
        if (particles.ContainsKey(name))
        {
          particles[name].GetComponent<ParticleSystem>().Stop();
        }
        else
        {
            Debug.Log("particle is can't finded");
        }
    }
    //好きな場所へparticle Systemを移動して、play
    public void playInPlace(Vector3 place,string name)
    {
        if (particles.ContainsKey(name))
        {
            particles[name].position = place;
            particles[name].GetComponent<ParticleSystem>().Play();

        }
        else
        {
            Debug.Log("particle is can't finded");
        }
    }

    //好きな場所でものを作る
    public void spawnInPlace(Vector3 pos,int v)
    {
        Instantiate(objToSpawn[v], pos, Quaternion.identity);
    }
    public void PassCrossair(ADV_CrossAir c)
    {
        crossAir = c;
    }
    public void setUImanager(ADV_UIManager man)
    {
        uiManager = man;
        popUp = GameObject.Find("PopUp");
        if (popUp != null)
            pointText = popUp.GetComponentInChildren<Text>();
    }

   
    public void SurvivalPopUp(int n)
    {
        //文字を変える
        pointText.text = n.ToString();
        //アニメーションPlay
        popUp.GetComponent<Animator>().SetTrigger("GO");
        
    }
    //popUpPlay
    public void generatePopUp(int n)
    {
        //文字を変える
        pointText.text= n.ToString();
        //CrossAirのところへ移動
        pointText.transform.position = crossAir.getCursorPos();
        //アニメーションPlay
        popUp.GetComponent<Animator>().SetTrigger("GO");
        if (uiManager == null) return;
        //スコアアップ
        uiManager.addPoints(n);
    }

    //popUp＆particleSystem
    public void EffectAndPopup(Vector3 place, string name,int n)
    {
        if (particles.ContainsKey(name))
        {
            particles[name].position = place;
            particles[name].GetComponent<ParticleSystem>().Play();

        }
        else
        {
            Debug.Log("particle is can't finded");
        }
        pointText.text = n.ToString();
        pointText.transform.position = crossAir.getCursorPos();
        popUp.GetComponent<Animator>().SetTrigger("GO");
        uiManager.addPoints(n);
    }
}

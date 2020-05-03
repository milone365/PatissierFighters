using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowMan : MonoBehaviour
{
    [SerializeField]
    int maxMovement = 6;
    [SerializeField]
    ParticleSystem snow=null;
    int movement = 0;
    GameObject objectToFreez = null;
    bool go = false;
    bool ai=false;
    float frozeTimer = 1.5f;
    float scrollingTime = 1.5f;
    Transform camerahandler;
    void Update()
    {
        if (ai)
        {
            automaticScroll();
        }
        else
        {
            if (Input.GetButtonDown(StaticStrings.X_key))
            {
                addMovement();
            }
        }
    }
    //インデクスに1を足す、上限に着いたら雪だるまを削除して、キャラクターを活性する
    void addMovement()
    {
        movement++;
        GetComponent<Animator>().SetTrigger(StaticStrings.Scroll);
        snow.Play();
        if (movement > maxMovement)
        {
            if (!go)
            {
                go = true;
                ripristing();
            }
            
        }
    }
    //reference
    public void objectToFreeze(GameObject obj,bool isAI,Transform camera)
    {
        camerahandler = camera;
        objectToFreez = obj;
        objectToFreez.SetActive(false);
        ai = isAI;
        if (ai == false)
        {
            maxMovement = 20;
        }
    }
    //無効されたオブジェクトを活性する
    private void ripristing()
    {
        if(objectToFreez!=null)
        objectToFreez.SetActive(true);
        if (camerahandler != null)
        {
            B_Player player = objectToFreez.GetComponentInChildren<B_Player>();
            camerahandler.transform.SetParent(player.transform);
        }
        FiniStateMachine sm = objectToFreez.GetComponent<FiniStateMachine>();
        if (sm != null) { sm.changeState(stateType.cake); }
        Destroy(this.gameObject);
    }
    //時間が終ったら自動的に関数を呼ぶ
    public void automaticScroll()
    {
        frozeTimer -= Time.deltaTime;
        if (frozeTimer <= 0)
        {
            frozeTimer = scrollingTime;
            addMovement();
            
        }
    }
}

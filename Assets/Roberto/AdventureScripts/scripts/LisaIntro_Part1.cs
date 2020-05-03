using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LisaIntro_Part1 : MonoBehaviour
{
    [SerializeField]
    GameObject lisa2 = null;
    float pushTime = 3;
    float pushCounter = 3;
    Animator anim;
    int pushIndex = 0;
    [SerializeField]
    Transform pastoryBag = null;
    void Start()
    {
        anim = GetComponent<Animator>();
        
    }

    //終わったら無効にする,二番目のキャラクターを活性する

    // Update is called once per frame
    void Update()
    {
        pushCounter -= Time.deltaTime;
        if(pushCounter<=0)
        {
            pushCounter = pushTime;
            anim.SetTrigger("PUSH");
            pushIndex++;
            if (pushIndex > 2)
            {
                lisa2.SetActive(true);
                gameObject.SetActive(false);
            }
        }
    }

    public void Cream()
    {
        if (EffectDirector.instance != null && pastoryBag != null)
            EffectDirector.instance.playInPlace(pastoryBag.position, StaticStrings.CREAMHIT);
    }
}

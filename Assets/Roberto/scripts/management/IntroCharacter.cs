using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroCharacter : MonoBehaviour
{
    Animator anim;
    bool idle=false;
    [SerializeField]
    Transform introCube=null;
    [SerializeField]
    float speed = 1;
    bool go = false;
    [SerializeField]
    GameObject continueScreen=null;
    Soundmanager soundmanager;
    [SerializeField]
    bool isCharacter = false;
    Vector3 introcubepos = Vector3.zero;
    void Start()
    {
        soundmanager = FindObjectOfType<Soundmanager>();
        if(soundmanager!=null)
        soundmanager.PlayBgmByName(StaticStrings.MenuBGM);
        anim = GetComponent<Animator>();
        anim.SetTrigger(StaticStrings.walking);
        introcubepos = new Vector3(introCube.transform.position.x, transform.position.y, introCube.transform.position.z);
        init();
        
    }

    void init()
    {
        go = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!go) return;
        anim.SetBool(StaticStrings.idle,idle);
        transform.position = Vector3.MoveTowards(transform.position, introcubepos,speed*Time.deltaTime);
        
    }

    public void Play()
    {
        StartCoroutine(playCo());
    }

    IEnumerator playCo()
    {
        idle = true;

        yield return new WaitForSeconds(1);
        idle = false;
        anim.SetTrigger(StaticStrings.samba);
       
       
        if (isCharacter)
        {
            Title t = FindObjectOfType<Title>();
            t.show();
        }
         yield return new WaitForSeconds(1.5f);
        
        //メッセージを活性する

        if (isCharacter)
        {
            if (continueScreen != null && !continueScreen.activeInHierarchy)
            {
                continueScreen.SetActive(true);
            }
        }
        //ｘを押さないうち続けない
        while (!Input.GetButtonDown(StaticStrings.X_key))
        {
            yield return null;
        }

        anim.SetTrigger(StaticStrings.waving);
        if (isCharacter)
        {
            SceneLoader loader = FindObjectOfType<SceneLoader>();
            loader.Load(loader.scenename);
        }
        //挨拶
        yield return new WaitForSeconds(1f);
        anim.SetTrigger(StaticStrings.waving);
        yield return new WaitForSeconds(1f);
        anim.SetTrigger(StaticStrings.waving);
        
    }
}

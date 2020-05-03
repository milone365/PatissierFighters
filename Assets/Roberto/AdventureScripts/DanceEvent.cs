using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DanceEvent : MonoBehaviour
{
    //images
    [SerializeField]
    Sprite[] icons=null;
    [SerializeField]
    Sprite blank = null;
    //messagebox
    GameObject panel;
    Text messageTXT;
    string message="";
    //waypoints
    public List<Transform> routepoints = new List<Transform>();
    [SerializeField]
    Transform center = null;
    //player dance step selections
    PassSelector[] stepSelectors = null;
    //player choseIndex
    int choseIndex = -1;
    ADV_Player player;
    //
    bool battleIsStarted = false;
    public List<Dancer> dancers = new List<Dancer>();
    int stepCount = 0;
    float waitingTime = 3;
    bool Stop = false;
    int danceIndex = 0;
    Dancer currentDancer;
    Vector3 playerStartPos = Vector3.zero;
    float takePositionTime = 5;
    //前のステップ
    DanceMoves lastMoves=DanceMoves.ballerin;
    #region start
    private void Start()
    {
        player = FindObjectOfType<ADV_Player>();
        playerStartPos = player.transform.position;
        stepSelectors = GetComponentsInChildren<PassSelector>();
        RouteFollower[] robots = GetComponentsInChildren<RouteFollower>();
        panel = GameObject.Find("MessageBox");
        messageTXT = panel.GetComponentInChildren<Text>();
        panel.SetActive(false);
        for(int i=0; i < stepSelectors.Length; i++)
        {
            stepSelectors[i].gameObject.SetActive(false);
            Dancer d = new Dancer();
            d.follower = robots[i];
            d.startPoint = d.follower.transform.position;
            d.icon = robots[i].gameObject.GetComponentInChildren<SpriteRenderer>();
            d.anim = robots[i].gameObject.GetComponent<Animator>();
            switch (i)
            {
                case 0: d.danceName = "SAMBA";break;
                case 1: d.danceName = "SALSA";break;
                case 2: d.danceName = "MACARENA";break;
            }
            dancers.Add(d);
            dancers[i].follower.addWayPoint(d.startPoint);
        }
        
        Invoke("activeAll", 3);
    }
    void activeAll()
    {
        if (Soundmanager.instance != null)
        {
            Soundmanager.instance.PlayBgmByName("POP");
        }
        message = "ダンスバトルへようこそ! ロボットのダンスを見て" +
            "ステップを覚えて！";
        panel.SetActive(true);
        StartCoroutine(writeMessage(message));
        currentDancer = dancers[danceIndex];
        currentDancer.follower.addWayPoint(center);
        battleIsStarted = true;
    }
#endregion

    private void Update()
    {
        if (!battleIsStarted) return;
        if (Stop) return;
        battle();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            goBack();
        }
    }
    void battle()
    {
        takePositionTime -= Time.deltaTime;
        if (takePositionTime <= 0)
        {
            if (currentDancer.danced == false)
            {
                currentDancer.anim.SetTrigger(currentDancer.danceName);
                currentDancer.danced = true;
            }
            else
            {
                waitingTime -= Time.deltaTime;
                if (waitingTime <= 0)
                {
                    waitingTime = 3;
                    stepCount++;
                    currentDancer.icon.sprite = blank;
                    if (stepCount > 3)
                    {
                        Stop = true;
                        stepCount = 0;
                        GoToTheNextDancer();
                        return;
                    }
                    //元のステップといつも違うように
                    bool isDifferent = false;
                    DanceMoves m=DanceMoves.ballerin;
                    int rnd=0;
                    while (!isDifferent)
                    {
                        //chose in enum class
                         rnd = UnityEngine.Random.Range(0, 3);
                        //convert int in enum
                        m = (DanceMoves)rnd;
                        if (m != lastMoves)
                        {
                            isDifferent = true;
                            lastMoves = m;
                        }
                    }
                   
                    //add to moves list
                    currentDancer.phaseMoves.Add(m);
                    //change icon 
                    currentDancer.icon.sprite = icons[rnd];
                    if (rnd == 1) {
                        currentDancer.icon.gameObject.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
                    } else {
                        currentDancer.icon.gameObject.transform.localScale = new Vector3(1, 1, 1);
                    }
                    danceMessage(stepCount);
                }
            }
        }
       
    }
    //どんなステップをわかるようにメッセージを書く
    void danceMessage(int n)
    {
        string str = "";
        switch (n)
        {
            case 1:str = "1"; break;
            case 2: str = "2"; break;
            case 3: str = "3"; break;
        }
        StartCoroutine(writeMessage(str));
    }
    //プレイヤーのターンが始まる
    private void GoToTheNextDancer()
    {
         currentDancer.follower.addWayPoint(currentDancer.startPoint);
         player.GetComponent<RouteFollower>().addWayPoint(center);
            foreach(var s in stepSelectors)
            {
                s.gameObject.SetActive(true);
            }
    }

    //正しければポイントを貰う
    public void SendDanceMovement(DanceMoves mov)
    {
        string m = "";
        choseIndex++;
        switch (choseIndex)
        {
            case 0:m = "よくできた"; break;
            case 1:m = "すごい！";break;
            case 2:m = "いい感じ";break;
        }
        if (choseIndex > currentDancer.phaseMoves.Count) return;
        if (currentDancer.phaseMoves[choseIndex] == mov)
        {
            StartCoroutine(writeMessage(m));
            if (choseIndex == 2)
            {
                foreach (var s in stepSelectors)
                {
                    s.gameObject.SetActive(false);
                }
                player.GetComponent<Animator>().SetTrigger(currentDancer.danceName);
                EffectDirector.instance.generatePopUp(300);
                Invoke("goBack", 10);
            }
           
        }
        else
        {
            //そうではない場合は、出る
            StartCoroutine(writeMessage("ダンスへたっぴだね"));
            foreach (var s in stepSelectors)
            {
                s.gameObject.SetActive(false);
            }
            goBack();
        }
    }

    //自分の所へ戻って、ダンスバトルが終わったら続く
   void goBack()
    {
        player.GetComponent<Animator>().SetTrigger("GOBACK");
        choseIndex =-1;
        player.GetComponent<RouteFollower>().addWayPoint(playerStartPos);
        danceIndex++;
        if (danceIndex >dancers.Count-1)
        {
            battleIsStarted = false;
            StartCoroutine(endCo());
            return;
        }
        currentDancer = dancers[danceIndex];
        currentDancer.follower.addWayPoint(center);
        takePositionTime = 5;
        Stop = false;
    }
    //ルートを続く
    IEnumerator endCo()
    {
        yield return new WaitForSeconds(1);
        EffectDirector.instance.generatePopUp(250);
        player.GetComponent<Animator>().SetTrigger("GOBACK");
        player.GetComponent<RouteFollower>().changeRoute(routepoints);
        yield return new WaitForSeconds(2);
        Destroy(panel);
    }

    //文字はゆっくり出てきます
   IEnumerator writeMessage(string txt)
    {
        messageTXT.text = "";
        foreach(char letter in txt.ToCharArray())
        {
            messageTXT.text += letter;
            yield return new WaitForSeconds(0.1f);
        }
        
    } 
    //ダンサークラス
    [System.Serializable]
    public class Dancer
    {
        public List<DanceMoves> phaseMoves = new List<DanceMoves>();
        public bool danced = false;
        public RouteFollower follower;
        public Vector3 startPoint;
        public SpriteRenderer icon;
        public Animator anim;
        public string danceName = "";
    }
}

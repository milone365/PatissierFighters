using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
public class B_GameManager : MonoBehaviour
{
    public List<B_Player> playerList = new List<B_Player>();
    public List<PancakeTower> allTowers = new List<PancakeTower>();
    SceneLoader sceneloader;
    public static B_GameManager instance;
    [SerializeField]
    GameObject mainCamera=null;
    [SerializeField]
    Sprite[] numbers = null;
    [SerializeField]
    Image numberImage=null;
    GameObject fadescreen;
    private void Awake()
    {
        instance = this;
    }
    //プレやのリストを作る
    void Start()
    {
        B_Player[] players = FindObjectsOfType<B_Player>();
        foreach(B_Player p in players)
        {
            playerList.Add(p);
        }
        PancakeTower[] towers = FindObjectsOfType<PancakeTower>();
        foreach(var item in towers)
        {
            allTowers.Add(item);
        }
        sceneloader = GetComponent<SceneLoader>();
        fadescreen = GameObject.Find(StaticStrings.FadeScreen);
        if(fadescreen!=null)
        fadescreen.GetComponent<Animation>().Play("unfade");
    }

    IEnumerator showGameEnd()
    {
       PlayerPrefs.SetString(StaticStrings.result, winningMessage());
        sceneloader.Load("ResultsScene");
        yield return new WaitForSeconds(2f);
        mainCamera.SetActive(true);
       
    }

    //メッセージを書く
    string winningMessage()
    {
        string message = "";
        B_Player[] players = playerList.OrderByDescending(x => x.Tower.Height).ToArray();

        foreach (B_Player p in players)
        {
            message += p.PlayerName + ": " + p.Tower.Height +"\n";
        }
            return message;

    }

    public void StartcountDown(bool v)
    {
      StartCoroutine(countDownCo(v));
    }

    //三から一までのCountDown
    IEnumerator countDownCo(bool v)
    {
        CharacterSelection selection = FindObjectOfType<CharacterSelection>();
        if (selection != null)
        {
            selection.deactiveArrows();
        }
        if (!numberImage.enabled)
            numberImage.enabled = true;
        numberImage.sprite = numbers[0];
        if(Soundmanager.instance!=null)
        Soundmanager.instance.PlaySeByName(StaticStrings.CountDownSoundEffect);
        yield return new WaitForSeconds(1);
        numberImage.sprite = numbers[1];
        yield return new WaitForSeconds(1);
        numberImage.sprite = numbers[2];
        yield return new WaitForSeconds(1);
        if (v)
        {
            numberImage.sprite = numbers[3];
            //ゲーム活性する
            selection = FindObjectOfType<CharacterSelection>();
            if (selection == null)
            {
                BattleRoyalCharacterGen gen = FindObjectOfType<BattleRoyalCharacterGen>();
                gen.activeGame();

            }
            else
            {
                selection.activeGame();
            }

        }
        else 
        {
            numberImage.sprite = numbers[4];
            StartCoroutine(showGameEnd());
        }
        yield return new WaitForSeconds(0.5f);
        numberImage.enabled=false;
    }
}

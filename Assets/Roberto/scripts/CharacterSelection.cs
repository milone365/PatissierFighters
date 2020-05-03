using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.AI;
using SA;
public class CharacterSelection : MonoBehaviour
{
   public GameObject[] desactiveItems;
   public B_Player[] player;
   public GameObject[] buttons;
   public GameObject[] characters;
   public GameObject[] cpuCharacters;
   PancakeSpawner spawner;
    GameObject gamescene =null;
    Vector3 offsetPosition = new Vector3(0, 0, 0);
    bool isPressed = false;
    private void Start()
    {
       gamescene = GameObject.FindGameObjectWithTag(StaticStrings.gameScene);
        
    }

    public void selectCaracter(int num)
    {
        if (isPressed) return;
        isPressed = true;
       for (int i = 0; i < buttons.Length; i++)
        {
            if (i== num)
            {
                //キャラクターを作る
                GameObject newChar = Instantiate(characters[num], player[i].transform.position, player[i].transform.rotation);
                //中にプレイヤーを移動する
                player[i].gameObject.transform.SetParent(newChar.transform);
                player[i].transform.localPosition = offsetPosition;
                player[i].activeteCanvs();
                //gameSceneの中に移動
                newChar.transform.SetParent(gamescene.transform);
                Inputhandler hand = newChar.GetComponent<Inputhandler>();
                hand.InitInGame();
            }
            else
            {
                //AIを作る
                GameObject newChar = Instantiate(cpuCharacters[i], player[i].transform.position, player[i].transform.rotation);
                //中にプレイヤーを移動する
                player[i].transform.SetParent(newChar.transform);
                player[i].transform.localPosition = offsetPosition;
                //gameSceneの中に移動
                newChar.transform.SetParent(gamescene.transform);
                //カメラ削除
                Camerahandler hand = player[i].GetComponentInChildren<Camerahandler>();
                Destroy(hand.gameObject);
                //脳追加
                CPUBrain brain = newChar.GetComponent<CPUBrain>();
                brain.INIT(player[i]);
            }
            //メニューボタン無効
            buttons[i].SetActive(false);

        }

        B_GameManager.instance.StartcountDown(true);
    }
    //ゲーム始まり
    public void deactiveArrows()
    {
        
        for (int i = 0; i < desactiveItems.Length; i++)
        {
            desactiveItems[i].SetActive(false);
        }
    }
    public void activeGame()
    {
        spawner = FindObjectOfType<PancakeSpawner>();
        
        Clock clock = FindObjectOfType<Clock>();
        clock.GaneStart = true;
        Camera.main.gameObject.SetActive(false);
       
        Soundmanager sm = FindObjectOfType<Soundmanager>();
        if (sm != null)
        {
            sm.StopBgm();
            sm.PlayBgmByName(StaticStrings.BattleSceneBGM);
        }
        spawner.startGame();
        gameObject.SetActive(false);
        
    }
}


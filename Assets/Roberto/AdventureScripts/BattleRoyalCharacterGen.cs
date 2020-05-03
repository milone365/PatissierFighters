using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SA;

public class BattleRoyalCharacterGen : MonoBehaviour
{
    public Partecipants maxPlayers;
    public B_Player[] player;
    public GameObject[] characters;
    public GameObject[] cpuCharacters;
    PancakeSpawner spawner;
    GameObject gamescene = null;
    Vector3 offsetPosition = new Vector3(0, 0, 0);
    bool isPressed = false;
    private void Start()
    {
        gamescene = GameObject.FindGameObjectWithTag(StaticStrings.gameScene);
       
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            SpawnCharacters();
        }
    }
    public void SpawnCharacters()
    {
        if (isPressed) return;
        isPressed = true;
        for (int i=0; i<characters.Length;i++)
        {
            if (i<(int)maxPlayers)
            {
                //idを渡す
                player[i].playerID = "P" + (i+1)+ "_";
                //キャラクターを作る
                GameObject newChar = Instantiate(characters[i], player[i].transform.position, player[i].transform.rotation);
                //キャラクターのなかでプレーヤーのゲームオブジェクトを入れる
                player[i].gameObject.transform.SetParent(newChar.transform);
                player[i].transform.localPosition = offsetPosition;
                //active　canvas
                player[i].activeteCanvs();
                player[i].GetComponentInChildren<Camera>().targetDisplay = i;
                newChar.transform.SetParent(gamescene.transform);
                Inputhandler hand = newChar.GetComponent<Inputhandler>();
                hand.InitInGame();
            }
            else
            {
                //アイを作る
                GameObject newChar = Instantiate(cpuCharacters[i], player[i].transform.position, player[i].transform.rotation);
                player[i].transform.SetParent(newChar.transform);
                player[i].transform.localPosition = offsetPosition;
                newChar.transform.SetParent(gamescene.transform);
                Camerahandler hand = player[i].GetComponentInChildren<Camerahandler>();
                Destroy(hand.gameObject);
                CPUBrain brain = newChar.GetComponent<CPUBrain>();
                brain.INIT(player[i]);
            }
        }
        B_GameManager.instance.StartcountDown(true);
    }

    public void activeGame()
    {
        spawner = FindObjectOfType<PancakeSpawner>();

        Clock clock = FindObjectOfType<Clock>();
        clock.GaneStart = true;
        Camera.main.gameObject.SetActive(false);
        gameObject.SetActive(false);
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

public enum Partecipants
{
    Two=2,
    Three=3,
    Four=4
}
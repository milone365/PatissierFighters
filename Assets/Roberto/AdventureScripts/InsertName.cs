using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InsertName : MonoBehaviour
{
    string NM;
    InputField field;
    [SerializeField]
    public string gameType = "Adventure";
    private void Start()
    {
        field = GetComponentInChildren<InputField>();
    }
    //プレイヤーの名前を保存して、シーンをロード

    public void SetName()
    {
        NM = field.GetComponentInChildren<Text>().text;
        saveGame();
    }
   public void saveGame()
    {
        PlayerPrefs.SetString(StaticStrings.playerName, NM);
        SceneLoader loader = FindObjectOfType<SceneLoader>();
        loader.loadGAME();
        
    }
}

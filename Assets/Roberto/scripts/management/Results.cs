using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Results : MonoBehaviour
{
    [SerializeField]
    Text resultText=null;
   
    //メッセージを書く
    void Start()
    {
        if (PlayerPrefs.HasKey(StaticStrings.result))
        {
            resultText.text = PlayerPrefs.GetString(StaticStrings.result);
        }
        else
        {
            resultText.text = "";
        }
    }

}

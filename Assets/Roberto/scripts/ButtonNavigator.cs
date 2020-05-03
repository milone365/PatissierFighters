using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonNavigator : MonoBehaviour
{
    Button[] allButtons;
    public int buttonIndex = 0;
    [SerializeField]
    Transform _cursor=null;
    [SerializeField]
    Transform[] positions=null;
    public float r_input = 0;
    bool charSelector = false;
    bool stop = false;
    CharacterSelection cs;
    bool loading = true;

    //インプットを読む
    bool right_input()
    {
        r_input = Input.GetAxis(StaticStrings.Right);
        r_input = r_input > 0 ? 1 : r_input;
        if(r_input==0)
        {
            stop = false;
        }
        return r_input >0;
       
    }

    bool left_input()
    {
        r_input = Input.GetAxis(StaticStrings.Right);
        r_input = r_input < 0 ? -1 : r_input;
        if (r_input == 0)
        {
            stop = false;
        }
        return r_input < 0;

    }

    private void Start()
    {
        cs = FindObjectOfType<CharacterSelection>();
        if (cs != null)
        {
            charSelector = true;
        }
        allButtons = GetComponentsInChildren<Button>();
        allButtons[buttonIndex].Select();
        _cursor.transform.position = positions[0].transform.position;
        Invoke("onloadingScene", 3);
    }

    void onloadingScene()
    {
        loading = false;
    }
    //UIの中で移動
    private void Update()
    {

        if (loading) return;
         if (right_input())
         {
            if (!stop)
            {
                stop = true;
                buttonIndex++;
                buttonIndex %= allButtons.Length;
                _cursor.transform.position = positions[buttonIndex].transform.position;
                allButtons[buttonIndex].Select();
            }
                
          }
        if (left_input())
        {
            if (!stop)
            {
                stop = true;
                buttonIndex--;
                if(buttonIndex < 0)
                {
                    buttonIndex = 0;
                }
                _cursor.transform.position = positions[buttonIndex].transform.position;
                 allButtons[buttonIndex].Select();
            }

        }

        buttonFuntion();
    }

   //ボタンの関数を呼ぶ
    public virtual void buttonFuntion()
    {
        if (Input.GetButtonDown(StaticStrings.X_key))
        {
            if (charSelector)
            {
                cs.selectCaracter(buttonIndex);
                if(_cursor.GetComponent<Image>()!=null)
                _cursor.GetComponent<Image>().enabled = false;
            }
            else
            {
                string n = allButtons[buttonIndex].GetComponent<SceneLoader>().scenename;
                SceneLoader s = allButtons[buttonIndex].GetComponent<SceneLoader>();

                if (s.isActiveAndEnabled)
                {
                    s.Load(n);
                }
            }
            
        }
    }
}

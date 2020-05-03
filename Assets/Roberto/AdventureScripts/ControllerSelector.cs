using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ControllerSelector : MonoBehaviour
{
    [SerializeField]
    Button[] allButtons = null;
    public int buttonIndex = 0;
   float r_input, l_input;
    bool stop = true;
    [SerializeField]
    Transform _cursor = null;
    [SerializeField]
    Transform[] positions = null;

    SceneLoader loader;
    #region inputs
    bool right_Input()
    {
        r_input = Input.GetAxis(StaticStrings.Right);
        r_input = r_input > 0 ? 1 : r_input;
        if (r_input == 0)
        {
            stop = false;
        }
        return r_input > 0;
    }
    bool left_Inpu()
    {
        l_input = Input.GetAxis(StaticStrings.Right);
        l_input = l_input < 0 ? -1 : l_input;
        if (l_input == 0)
        {
            stop = false;
        }
        return l_input < 0;
    }
    #endregion
    void Start()
    {
        loader = FindObjectOfType<SceneLoader>();
        _cursor.transform.position = positions[0].transform.position;
    }


    void Update()
    {
        
        buttonNavigation();
         if (Input.GetButtonDown(StaticStrings.X_key))
            {
                if (buttonIndex == 0)
                {
                    simple();
                }
                else
                {
                    pro();
                }
            }
        
    }

    //メニューの中で移動
    void buttonNavigation()
    {
        if (right_Input())
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
        if (left_Inpu())
        {
            if (!stop)
            {
                stop = true;
                buttonIndex--;
                if (buttonIndex < 0)
                {
                    buttonIndex = 0;
                }
                allButtons[buttonIndex].Select();
                _cursor.transform.position = positions[buttonIndex].transform.position;
            }

        }
    }

    //シーンロード
    public void simple()
    {
        PlayerPrefs.SetInt(StaticStrings.GameControllerValue, 0);
        loader.loadGAME();
    }
    public void pro()
    {
        PlayerPrefs.SetInt(StaticStrings.GameControllerValue, 1);
        loader.loadGAME();

    }

  
    
}

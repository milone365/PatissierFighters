using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    Button[] allButtons=null;
    public int buttonIndex = 0;
    GameObject panel = null;
    public string sceneToLoad;
    float r_input, l_input;
    bool stop = true;
    bool menuActive;
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
        panel = transform.GetChild(0).gameObject;
        panel.SetActive(false);
    }

    
    void Update()
    {
        if (Input.GetButtonDown(StaticStrings.options))
        {
            gamePause();
        }
        buttonNavigation();
        menuActive = panel.activeInHierarchy;
        if (menuActive)
        {
            if (Input.GetButtonDown(StaticStrings.X_key))
            {
                if (buttonIndex == 0)
                {
                    goToAnotherScene();
                }
                else
                {
                    resume();
                }
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
            }

        }
    }

    //シーンロード
     public void goToAnotherScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneToLoad);
    }
    
    //ゲームに戻る
     public void resume()
     {
        panel.SetActive(false);
        Time.timeScale = 1;
    }
    //pause
     public void gamePause()
    {
        if (panel == null) return;
        if (!panel.activeInHierarchy)
        {
            panel.SetActive(true);
            Time.timeScale = 0;
            allButtons[buttonIndex].Select();
        }
        else
        {
            panel.SetActive(false);
            Time.timeScale = 1;
        }
    }
}

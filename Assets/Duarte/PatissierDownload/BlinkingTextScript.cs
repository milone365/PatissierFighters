using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement; 

public class BlinkingTextScript : MonoBehaviour
{
    [SerializeField]
    Text BlinkingText;
    [SerializeField]
    Image img=null;
    public string IsBlinkingText = "";
    string WhiteText = "";
    bool IsBlinking = true;
    public GameObject fadescreen;
    public float timer;

    public string SceneToLoad;

    void Start()
    {
      IsBlinking = true;

       // BlinkingText = GetComponent<Text>();

        StartCoroutine(BlinkText());

    }

    public void activeBlickText()
    {
        IsBlinking = true;

        BlinkingText = GetComponent<Text>();

        StartCoroutine(BlinkText());
    }

    public IEnumerator BlinkText()
    {
        while (IsBlinking)
        {
            if(BlinkingText!=null)
            BlinkingText.text = WhiteText;
            if (img != null)
                img.enabled = false;

            yield return new WaitForSeconds(.5f);
            if(img!=null)
            img.enabled = true;
            if (BlinkingText != null)
                BlinkingText.text = IsBlinkingText;
            yield return new WaitForSeconds(1f);
        }
    }
    public void LoadScene()
    {
        StartCoroutine(loadsceneCo());
          
    }
    IEnumerator loadsceneCo()
    {
        fadescreen.SetActive(true);

       
        yield return new WaitForSeconds(timer);
        SceneManager.LoadScene(SceneToLoad);
    }

}

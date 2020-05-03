using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvIntro_last : MonoBehaviour
{
    [SerializeField]
    GameObject pancake = null;
    Animator anim;
    SceneLoader loader;
    //カットシーンのパンケーキの移動アニメーションとシーンロード
    void Start()
    {
        anim = pancake.GetComponent<Animator>();
        anim.SetTrigger("MOVE");
        loader = FindObjectOfType<SceneLoader>();
        StartCoroutine(endSceneCo());
    }
    IEnumerator endSceneCo()
    {
        yield return new WaitForSeconds(7);
        loader.loadGAME();
    }
  
}

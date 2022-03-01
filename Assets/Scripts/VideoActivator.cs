using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
public class VideoActivator : MonoBehaviour
{
    public float timer = 2;
    [SerializeField]
    VideoPlayer player = null;
    [SerializeField]
    GameObject controllerImage = null;
    private void Start()
    {
        StartCoroutine(VideoCo());
    }
 
    IEnumerator VideoCo()
    {
        yield return new WaitForSeconds(1);
        player.Play();
        yield return new WaitForSeconds(timer);
        controllerImage.SetActive(true);
        gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LisaADV_Intro : MonoBehaviour
{
    [SerializeField]
    Transform point = null;
    [SerializeField]
    float speed = 10;
    Vector3 destination;
    Animator animator;
    Animator anim;
    [SerializeField]
    GameObject questionMark = null;
    Vector3 startPosition;
    
    ADV_Intro_Robots brownRobot;
    ParticleSystem electricity;
    bool minization = false;
    float minSize = 6.5f;
    float size = 50;
    [SerializeField]
    GameObject exclamationMark = null;
    float gravity = -1;
    private void Start()
    {
        
        startPosition = transform.position;
        animator = GetComponent<Animator>();
        destination = new Vector3(point.position.x, transform.position.y, point.position.z);
        electricity = GetComponentInChildren<ParticleSystem>();
    }

    
    void Update()
    {
        //小さくなる
        if (minization)
        {
            if (size > minSize)
            {
                size -= Time.deltaTime*10;
                transform.localScale= new Vector3(size, size, size);
            }
            else
            {
                //gravity
                gravity -= Time.deltaTime;
                transform.Translate(0, gravity, 0);
            }
        }
        else
        {
            movement();
        }
    }

    //移動
    void movement()
    {

        if (transform.position != destination)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
            transform.LookAt(destination);
            Vector3 look = destination - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(look), Time.deltaTime * 2f);
        }
        float distance = Vector3.Distance(transform.position, destination);
        animator.SetFloat("MOVE", distance);
    }
    //最初のポイントに戻る
    public void comeBack()
    {
        destination = startPosition;
        StartCoroutine(ShotCo());
    }
    //ロボットが打つ関数
    IEnumerator ShotCo()
    {
        yield return new WaitForSeconds(2);
        if(questionMark!=null)
        questionMark.SetActive(true);
         yield return new WaitForSeconds(4);
        brownRobot = GameObject.Find("BrownRobot").GetComponent<ADV_Intro_Robots>();
        brownRobot.passLisa(this);
        brownRobot.ShotAction(); 
    }
    
    //minimize活性
    public void Minimize()
    {
        electricity.Play();
        minization = true;
        questionMark.SetActive(false);
        exclamationMark.SetActive(true);
        
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADV_Intro_Robots : MonoBehaviour
{
    [SerializeField]
    Transform pistol = null;
    [SerializeField]
    GameObject magicRay = null;
    [SerializeField]
    GameObject exclamtionMark = null;
    [SerializeField]
    List<Transform> points = new List<Transform>();
    [SerializeField]
    float speed = 10;
    Vector3 destination;
    Animator animator;
    float actionTimeCounter = 2;
    bool isDead = false;
    int pointIndex = 0;
    LisaADV_Intro lisa;
    SDV_CutSceneManager manager;
    private void Start()
    {
        animator = GetComponent<Animator>();
        destination = new Vector3(points[pointIndex].position.x, transform.position.y, points[pointIndex].position.z);
        manager = FindObjectOfType<SDV_CutSceneManager>();
        manager.AddRobot(this);


    }
    //移動
    void Update()
    {
        if (isDead) return;
        if (transform.position != destination)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
            transform.LookAt(destination);
            Vector3 look = destination - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(look), Time.deltaTime * 2f);
        }
        else
        {
            if (pointIndex<points.Count-1)
                {
                    pointIndex++;
                    destination = new Vector3(points[pointIndex].position.x, transform.position.y, points[pointIndex].position.z);
            }
            else
            {
                actionTimeCounter -= Time.deltaTime;
                if (actionTimeCounter <= 0)
                {

                }

            }
         }
        float distance = Vector3.Distance(transform.position, destination);
        animator.SetFloat("MOVE", distance);
    }
    //打つ
    public void ShotAction()
    {
        if (lisa != null)
        {
            transform.LookAt(lisa.transform.position,Vector3.up);
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        }
        if (exclamtionMark != null)
            exclamtionMark.SetActive(true);
        animator.SetTrigger("ACTION");
    }
    //waypointをもらう
    public void addPointToList(Transform p)
    {
        points.Add(p);
    }
    //バレットを打つ、 LisaADV_Intro スクリプトから minimizeを呼び出す
    public void InstantiateRay()
    {
       GameObject newMagic = Instantiate(magicRay, pistol.transform.position, pistol.transform.rotation);
        newMagic.GetComponent<MagicRay>().setDestination(transform.forward * 5000);
        Invoke("callMinimize", 1);
    }
    void callMinimize()
    {
        lisa.Minimize();
        manager.guideRobot();
        exclamtionMark.SetActive(false);
     
    }
    //reference
    internal void passLisa(LisaADV_Intro lisaADV_Intro)
    {
        lisa = lisaADV_Intro;
    }

  
    
}

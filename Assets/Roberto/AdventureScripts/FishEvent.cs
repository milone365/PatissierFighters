using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishEvent : MonoBehaviour
{
    Vector3 posDown=Vector3.zero, posUp=Vector3.zero;
    float moveDelay = 3;
    [SerializeField]
    float moveCounter=3;
    float moveSpeed = 5;
    [SerializeField]
    float yOffset = 8;
    bool goUp = true;
    private void Start()
    {
        
        posDown = transform.position;
        posUp = new Vector3(transform.position.x, transform.position.y + yOffset, transform.position.z);
    }

    //上下へ移動
    private void Update()
    {
        moveCounter -= Time.deltaTime;
        if (moveCounter <= 0)
        {
            goUp = !goUp;
            moveCounter = moveDelay;
            
        }
        if (goUp)
        {
            moveUP();
        }
        else
        {
            moveDown();
        }
    }

    private void moveDown()
    {
        transform.position = Vector3.Lerp(transform.position, posDown, moveSpeed * Time.deltaTime);
    }

    private void moveUP()
    {
        transform.position = Vector3.Lerp(transform.position, posUp, moveSpeed * Time.deltaTime);
    }
}

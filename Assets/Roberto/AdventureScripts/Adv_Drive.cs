using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Adv_Drive : MonoBehaviour
{
    [SerializeField]
    protected float moveSpeed = 15;
    SkinnedMeshRenderer rend;
    [SerializeField]
    float movingTime = 2;
    float movingCounter;
    public float movingAmount = 15;
    float X_arrow, Y_arrow, Z_arrow;
    bool moving;
    public bool MOVING { get { return moving; } set { moving = value; } }
    bool invincible = false;
    float invincibletimer = 3;
    // Start is called before the first frame update
    void Start()
    {
        movingCounter = movingTime;
        X_arrow = 0;
        Y_arrow = 0;
        Z_arrow = 0;
        rend = GetComponentInChildren<SkinnedMeshRenderer>();
    }

    // 移動とblinking
    void Update()
    {
        if (moving)
        {
            Move();
        }
        if (invincible)
        {
            invincibletimer -= Time.deltaTime;
            if (Mathf.FloorToInt(invincibletimer * 10) % 2 == 0)
            {
                rend.enabled = true;
            }
            else
            {
                rend.enabled = false;
            }
            if (invincibletimer <= 0)
            {
                invincible = false;
                rend.enabled = true;
            }
        }
       
    }

    public void blinking()
    {
        invincible = true;
    }
    #region Arrows
    //移動の為値
    public void MoveTo(Arrow d)
    {
        if (moving) return;
        switch (d)
        {
            case Arrow.right:
                X_arrow = movingAmount;
                break;
            case Arrow.left:
                X_arrow = -movingAmount;
                break;
            case Arrow.up:
                Y_arrow = movingAmount;
                break;
            case Arrow.down:
                Y_arrow = -movingAmount;
                break;
            case Arrow.front:
                Z_arrow = movingAmount;
                break;
            case Arrow.back:
                Z_arrow = -movingAmount;
                break;
        }
        moving = true;
    }
    //移動
    void Move()
    {
        float v = moveSpeed * Time.deltaTime;
        transform.Translate(X_arrow * v, Y_arrow * v, Z_arrow * v);
        movingCounter -= Time.deltaTime;
        if (movingCounter <= 0)
        {
            moving = false;
            X_arrow = 0;
            Y_arrow = 0;
            Z_arrow = 0;
            movingCounter = movingTime;
        }


    }


    #endregion
}

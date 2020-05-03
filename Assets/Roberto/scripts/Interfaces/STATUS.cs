using UnityEngine;
using System.Collections.Generic;
using System.Collections;


public class STATUS
{
    float speed = 8;
    float pettingRange = 10;
    float runSpeed = 12;
    int ingredient;
    #region property
    public float RunSpedd { get { return runSpeed; } set { runSpeed = value; } }
    public float pettingrange
    {
        get { return pettingRange; }
        set { pettingRange = value; }
    }
    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }
    public int Ingredients
    {
        get { return ingredient; }
        set { ingredient = value; }
    }
    #endregion
    float Mp = 0;
    float MaxMP = 100;
    Weapon currentWeapon;
    
    public float runningSpeed = 15;
    public float ATTACK = 1; //{ get { return attak; } set { attak = value; } }
    public STATUS()
    {
       currentWeapon = new Weapon();
    }
    int bomb = 2;
    public int BOMB { get { return bomb; } set { bomb = value; } }
   
    //武器
    public Weapon getCurrentweapon
    {
      get { return currentWeapon; }
      set { currentWeapon = value; }
    }
    //爆弾
    public void addBomb(int value)
    {
        BOMB += value;
    }
    //武器を変える
    public void changeWeapon(Weapon wp)
    {
        currentWeapon = wp;
    }
    //マナ
    public float getMP()
    {
        return Mp;
    }
    public void AddMP(float value)
    {
        Mp += value;
        if (Mp > MaxMP)
        {
            Mp = MaxMP;
        }
    }
    public void UseManapoint(float v)
    {
        Mp -= v;
        if (Mp <= 0)
        {
            Mp = 0;
        }
    }

    
}


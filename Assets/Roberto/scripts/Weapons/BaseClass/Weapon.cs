using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Weapon
{
   
   protected float chasingDistance,attackRange;
   protected float attackDelay,attak;
   protected float attacktimer;
   protected int numOfAttack;
   public WPbullet bullet { get { return bu; } set { bu = value; } }
   public float bulletRange = 50;
   WPbullet bu;
    bool haveSpBULLET=false;
    public bool getSPbullet() { return haveSpBULLET; }
   public void changeBullet(WPbullet b)
   {
        bullet = b;
   }

   public Weapon()
    {
        chasingDistance = 10;
        attackRange = 2.5f;
        attackDelay = 1.5f;
        attak = 1;
        attacktimer = attackDelay;
        numOfAttack = 4;
        
    }

   public Weapon(float _chasingDistance,float atkrange,float atkdelay,float atk, float atkTimer,int numOfAtk)
    {
        chasingDistance = _chasingDistance;
        attackRange = atkrange;
        attackDelay = atkdelay;
        attak = atk;
        attacktimer = atkTimer;
        numOfAttack = numOfAtk;
       
    }
    

    public float attackrange
    {
        get { return attackRange; }
        set { attackRange = value; }
    }
    public float chasingdistance
    {
        get { return chasingDistance; }
        set { chasingDistance = value; }
    }
    public float attackdelay
    {
        get { return attackDelay; }
        set { attackDelay = value; }
    }
    public float Attack
    {
        get { return attak; }
        set { attak = value; }
    }
    public float attackTimer
    {
        get { return attacktimer; }
        set { attacktimer = value; }
    }
    public int numOfattack
    {
        get { return numOfAttack; }
        set { numOfAttack = value; }
    }

    public void activeSpecialBullet(bool v)
    {
        haveSpBULLET = v;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun :Weapon
{
    public Gun():base ()
    {
      
    }

    public Gun(float _chasingDistance, float _atkrange, float atkdelay, float atk, float atkTimer, int numOfAtk) :base()
    {
        chasingDistance = _chasingDistance;
        attackRange = _atkrange;
        attackDelay = atkdelay;
        attak = atk;
        attacktimer = atkTimer;
        numOfAttack = numOfAtk;
    }

   
}

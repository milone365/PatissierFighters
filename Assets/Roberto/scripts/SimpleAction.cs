using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SA;

public class SimpleAction : PlayerActions
{
    //symple comman controller
  public SimpleAction(B_Player p,Inputhandler inp,Animator an):base(p, inp, an)
    {
        p.takeIngredient += ontakingIngredient;
    }

    public override void InputUpdating()
    {

        if (R2Input())
        {
            fire();
        }
        if (SquareInput())
        {
            tossBomb();
        }
        if (TriangleInput())
        {
            UseSkill();
        }
        if (skillActive)
        {
            skillMantenance();
        }
    }

    void ontakingIngredient()
    {
        player.Add_SkillPoints(100);
        
    }
}

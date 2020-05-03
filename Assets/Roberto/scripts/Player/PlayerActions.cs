using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using SA;

public class PlayerActions
{
    string playerID = "";
    //powerUpValue
    int value = 0;
   protected B_Player player = null;
    int Currentstate = 0;
    GameObject actionMenu;
    public List<Transform> targets = new List<Transform>();

    //場合によって関数を変更するデリゲート
    public delegate void powerUpAction(float value);
    powerUpAction pwActions;
    //
   protected  bool skillActive = false;
    public Dictionary<string, Sprite> messageImages = new Dictionary<string, Sprite>();
   protected Animator anim;
   protected  Inputhandler hand;
    float fireRate = 0.2f;
    float fireCounter = 0.2f;
    Vector3 dir;
    float skilltime = 15;
    Sprite retrunSprite(string n)
    {
        if (messageImages.ContainsKey(n))
        {
            return messageImages[n];
        }
        return null;
    }
    #region constructor
    public PlayerActions(B_Player _p, Inputhandler handle,Animator _anim)
    {
        player = _p;
        playerID = player.getID();
        actionMenu = player.getCanvas().actionMenu;
        foreach (var p in player.enemyList)
        {
            targets.Add(p.transform);
            targets.Add(p.Tower.transform);
            targets.Add(p.Helper.transform);
        }
        
        hand = handle;
        anim = _anim;

        int count = 0;
        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i].GetComponent<B_Player>())
            {
                count = 0;
            }
            else if (targets[i].GetComponent<PancakeTower>())
            {
                count = 1;
            }
            else
            {
                count = 2;
            }

            player.getCanvas().giveTargetImage(i, count);
            //attackCounter = player.status.getCurrentweapon.attackdelay;
        }
       
        Sprite[] sprites = player.getCanvas().messageIcons.ToArray();
        foreach(var item in sprites)
        {
            messageImages.Add(item.name, item);
        }
        
    }

    #endregion

    #region inputs

    protected bool goUp = false;
    protected float x_input;
    //directions inputs
    protected bool upInput()
    {
        x_input = Input.GetAxis(playerID + StaticStrings.Up);
        x_input= x_input > 0 ? 1 : 0;
        if (x_input == 0) { goUp = false; }
        return x_input > 0;
    }

    protected bool downInput()
    {
        return Input.GetAxis(playerID + StaticStrings.Up) < 0;
    }
    protected bool RightInput()
    {
        return Input.GetAxis(playerID+StaticStrings.Right) > 0;
    }
    protected bool LeftInput()
    {
        return Input.GetAxis(playerID+StaticStrings.Right) < 0;
    }

    //backButtons inputs
   
   protected bool L1input()
    {
        return Input.GetButton(playerID+StaticStrings.L1_key);
    }
    protected bool R2Input()
    {
        
        return Input.GetButton(playerID+StaticStrings.R2_key);
    }
   protected bool R1Input()
    {
        return Input.GetButton(playerID+StaticStrings.R1_key);
    }

    //symbolsButtons
    protected bool TriangleInput()
    {
        return Input.GetButtonDown(playerID+StaticStrings.Triangle_key);
    }
    protected bool X_input()
    {
        
        return Input.GetButtonDown(playerID+StaticStrings.X_key);
       
    }
    protected bool CircleInput()
    {
        return Input.GetButtonDown(playerID+StaticStrings.Circle_key);
    }
    protected bool SquareInput()
    {
        return Input.GetButtonDown(playerID+StaticStrings.Square_key);
    }
    #endregion


    //UPDATE
    public virtual void InputUpdating()
    {
        if (R2Input())
        {
            fire();
        }
        if (R1Input())
        {
            powerUpsUpdating();
        }
        else
        {
            if (player.getCanvas().ingredientsMenu.activeInHierarchy)
            {
                player.getCanvas().ingredientsMenu.SetActive(false);
            }
        }
       if (L1input())
        {
            ActionsInputs();
            if (CircleInput())
            {
                player.helperAttack();
            }
        }
        if (TriangleInput())
        {
          UseSkill();
        }
        if (SquareInput())
        {
            tossBomb();
        }
        if (skillActive)
        {
            skillMantenance();
        }
        //カンヴァスの画像の活性
        if (L1input() && !R1Input())
        {
            actionMenu.SetActive(true);
            player.getMesage().SetActive(true);
            player.getIcon().SetActive(false);
        }
        else
        {
            actionMenu.SetActive(false);
            player.getMesage().SetActive(false);
            player.getIcon().SetActive(true);
        }
    }

    //爆弾を投げる
    protected void tossBomb()
    {
        if (player.status.BOMB < 1) { return; }
        player.status.BOMB--;
        anim.SetTrigger(StaticStrings.bomb);
    }

   
    #region PowerUps

    //robot poweup
    void PowerUp_Helper(float value)
    {
        player.Helper.powerUp(value);
    }
    //tower power up
     void PowerUp_Tower(float value)
    {
        player.Tower.powerUp(value);
    }
    //マナーポイントアップ
     void PowerUp_Add_SkillPoints(float value)
    {
        player.Add_SkillPoints(value);
    }

     void powerUpsUpdating()
    {
        //材料がなければ出る
        if (player.status.Ingredients<1) return;
        if (!player.getCanvas().ingredientsMenu.activeInHierarchy)
        player.getCanvas().ingredientsMenu.SetActive(true);

        if (upInput())
        {
            player.getCanvas().goToImage(0);
            value = 50;
            //delegate change
            pwActions = PowerUp_Add_SkillPoints;
        
        }
         if (LeftInput())
        {
            player.getCanvas().goToImage(1);
            value = 5;
            //delegate change
            pwActions = PowerUp_Helper;
        }
         if (RightInput())
        {
            player.getCanvas().goToImage(2);
            value = 5;
            //delegate change
            pwActions = PowerUp_Tower;
        }
        if (CircleInput())
        {
           if (pwActions != null)
            {
                //delegate active
                pwActions(value);
            }
               
            else
            {
                Debug.LogError("pwaAction is null");
            }
            player.status.Ingredients--;
            player.getCanvas().activeIngredientImage(false);
        }

    }
    #endregion

    
    
    #region input_Actions
    public void ActionsInputs()
    {
        //グルマを回る
        if (upInput())
           {
            if (!goUp)
            {
                goUp = true;
                Currentstate++;
                Currentstate %= targets.Count;
                hand.reciveTrget(targets[Currentstate]);
                player.getCanvas().arrowCenter.rotation = Quaternion.Euler(0, 0, Currentstate * 360 / 9);
                player.getCanvas().changeArrowColor(targets[Currentstate].GetComponent<ITeam>().getTeam());
            }
            player.changeMesageImage(retrunSprite("Attack_Icon"));


            }

            if (RightInput())
            {
                player.helperDefenceTower();
            player.changeMesageImage(retrunSprite("Defense_Icon"));
        }
            if (LeftInput())
            {
                player.helperMakeCake();
            player.changeMesageImage(retrunSprite("MakePancake"));
        }

            
    }
    #endregion

    #region FightIng
    //打つ
    protected void _Shoot()
    {
          player.SetDirectionfForShot();
         if (!anim.GetCurrentAnimatorStateInfo(0).IsName(StaticStrings.shooting)&&!anim.IsInTransition(0))
            anim.SetTrigger(StaticStrings.shooting);
    }

 
    #endregion

    #region skill
    //スキル活性
    public void UseSkill()
    {
        if (player.status.getMP() < 100) return;
        player.status.UseManapoint(100);
        skillActive = true;
        player.getCanvas().changeWeaponImage(true);
        player.changeWeapon(true);
    }
   protected void skillMantenance()
    {
        skilltime -= Time.deltaTime;
        if (skilltime <= 0)
        {
            skilltime = 15;
            skillActive = false;
            player.changeWeapon(false);
            player.getCanvas().changeWeaponImage(false);
        }
    }
    #endregion

    protected void fire()
    {
        fireRate -= Time.deltaTime;
        if (fireRate <= 0)
        {
            fireRate = fireCounter;
            _Shoot();
        }
       
    }
}

public enum PlayerID
{
    P1=1,
    P2=2,
    P3=3,
    P4=4
}
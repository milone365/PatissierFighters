using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectManager : MonoBehaviour
{
    public IceballController iceball;
    public GameObject iceballPrefab;
    public GameObject Player;
    public Rigidbody rigid_body;

    public float HP = 100;
    public float NowHP;

    public float time_flame = 0f;
    public float CountTime_flame = 0;
    public float NextCountTime_flame = 1;
    public float Timeout_flame = 5;

    public float time_donut = 0f;
    public float CountTime_donut = 0;
    public float NextCountTime_donut = 1;
    public float Timeout_donut = 5;

    public float time_iceball = 0f;
    public float CountTime_iceball = 0;
    public float NextCountTime_iceball = 1;
    public float Timeout_iceball = 5;

    public bool flameATK_on = false;
    public bool Timer_on_flame = false;

    public bool invincible_on = false;
    public bool Timer_on_donut = false;

    public bool iceBomb_on = false;
    public bool Timer_on_iceball = false;



    public void BombShoot(Vector3 dir)
    {
        GetComponent<Rigidbody>().AddForce(dir);
    }

    public int layer1 = 1;
    public int layer2 = 2;
    // Start is called before the first frame update
    void Start()
    {
        layer1 = LayerMask.NameToLayer("Player");
        layer2 = LayerMask.NameToLayer("Player");
    }


    private void OnCollisionEnter(Collision Player)
    {
        if (Player.gameObject.CompareTag("other"))
        {
            flameATK_on = true;
        }


        if (Player.gameObject.CompareTag("donut"))
        {
            GetComponent<ParticleSystem>().Play();
            invincible_on = true;
        }

        if (Player.gameObject.CompareTag("bomb"))
        {
            BombShoot(new Vector3(0, 200, 200)); //爆発の衝撃
        }

        if (Player.gameObject.CompareTag("icebomb"))
        {
            iceBomb_on = true;
            if (iceBomb_on)  //アイスボールを付ける
            {
                GameObject iceball =
                Instantiate(iceballPrefab) as GameObject;
               // iceball.GetComponent<IceballController>().ice(new Vector3(0, 2, 0));
            }
        }

    }

    //flame のエフェクト
    //継続ダメージ
    void FlameATK()
    {
        if (flameATK_on)
        {
            Timer_on_flame = true;
            time_flame += 1;
            if (time_flame >= 60)
            {
                CountTime_flame -= 1;
                time_flame = 0;

            }

            if (Timer_on_flame)
            {
                if (CountTime_flame <= -1)
                {
                    HP -= 1;
                    CountTime_flame = 0;
                    Timeout_flame -= 1;
                }
            }
            if (Timeout_flame <= 0)
            {
                flameATK_on = false;
                Timer_on_flame = false;
                Timeout_flame = 5;
            }
        }
    }

    //donutエフェクト
    void invincible()
    {
        if (invincible_on)
        {
            GetComponent<BoxCollider>().isTrigger = true;
            Timer_on_donut = true;
            time_donut += 1;

            if (time_donut >= 60)
            {
                CountTime_donut -= 1;
                time_donut = 0;

            }

            if (Timer_on_donut)
            {
                if (CountTime_donut <= -1)
                {
                    CountTime_donut = 0;
                    Timeout_donut -= 1;
                }
            }
            if (Timeout_donut <= 0)
            {
                invincible_on = false;
                Timer_on_donut = false;
                Timeout_donut = 5;
                GetComponent<BoxCollider>().isTrigger = false;
            }
        }
    }

    //icebomb エフェクト
    void iceBomb()
    {
        if (iceBomb_on)
        {
            rigid_body.constraints = RigidbodyConstraints.FreezeAll;  //プレイヤーをフリーズ
            Timer_on_iceball = true;
            time_iceball += 1;

            if (time_iceball >= 60)
            {
                CountTime_iceball -= 1;
                time_iceball = 0;

            }

            if (Timer_on_iceball)
            {
                if (CountTime_iceball <= -1)
                {
                    CountTime_iceball = 0;
                    Timeout_iceball -= 1;
                }
            }
            if (Timeout_iceball <= 0)
            {
                iceBomb_on = false;
                Timer_on_iceball = false;
                Timeout_iceball = 5;
                rigid_body.constraints = RigidbodyConstraints.None; //プレイヤーのフリーズ解除
            }
        }
    }

    private void FixedUpdate()
    {
        invincible();
        FlameATK();
        iceBomb();
    }

}

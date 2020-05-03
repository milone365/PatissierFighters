using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class ADV_SpiderBoss : MonoBehaviour,IShotable
{
    bool isdeath=false;
    Animation anim;
    [SerializeField]
    Transform player = null;
    public spiderAction currentAction = spiderAction.mime;
    [SerializeField]
    float moveSpeed=5;
    [SerializeField]
    float attackRange = 1;
    [SerializeField]
    float attackDelay = 3;
    float attackCounter;
    [SerializeField]
    float rotationSpeed=2;
    float cTime = 0f;
    [SerializeField]
    float hp = 1000;
    [SerializeField]
    Transform jumpPoint = null;
    bool jumping = false;
    GameObject broom;
    GameObject jumpingPoint;
    ADV_Boss_Platforms[] platforms;
    Vector3 playerPosition;
    #region UPDATE
    public event Action onBossDeath;
    void Start()
    {
        anim = GetComponent<Animation>();
        attackCounter = attackDelay;
        //find active player
        ADV_Player[] players = FindObjectsOfType<ADV_Player>();
        platforms = FindObjectsOfType<ADV_Boss_Platforms>();
        foreach(var p in platforms)
        {
            p.passBoss(this);
        }
        foreach(var p in players)
        {
            if (p.isActiveAndEnabled)
            {
                player = p.transform;
                playerPosition = player.transform.position;
            }
        } 
    }
    void Update()
    {
        if (isdeath) return;
        if (player == null)
        {
            anim.Play("Idle");
            return;
        }
        if (jumping) return;
        Move();
        rotation();
        
    }
    #endregion

    #region actions　
    //移動と攻撃
    private void Move()
    {
        switch (currentAction)
        {
            case spiderAction.idle:
                anim.Play("Idle");
                break;
            case spiderAction.chasing:
                if (player == null) return;
                
                anim.Play("Walk");
                float distance = Vector3.Distance(transform.position, player.position);
                if (distance > attackRange)
                {
                    Vector3 p = new Vector3(player.position.x, player.position.y, player.position.z);
                    transform.position = Vector3.MoveTowards(transform.position, p, moveSpeed * Time.deltaTime);
                }
                else
                {
                    changeAction(spiderAction.attack);
                }
                
                break;
            case spiderAction.attack:
                if (player == null) return;
                if(!anim.isPlaying)
                anim.Play("Idle");
                float d = Vector3.Distance(transform.position, player.position);
                if (d > attackRange)
                {
                    changeAction(spiderAction.chasing);
                   
                }
                else
                {
                    attackCounter -= Time.deltaTime;
                    if (attackCounter <= 0)
                    {
                        attackCounter = attackDelay;
                        attack();
                    }
                }
                break;
            case spiderAction.mime:
                if (jumpPoint == null)
                {
                    changeAction(spiderAction.none);
                    return;
                }
                else
                {
                    anim.Play("Walk");
                    transform.position = Vector3.MoveTowards(transform.position, jumpPoint.position, moveSpeed * Time.deltaTime);
                    Vector3 looktarget = jumpPoint.position;
                    Vector3 direction = looktarget - transform.position;
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotationSpeed);
                    float dist = Vector3.Distance(transform.position, jumpPoint.position);
                    if (dist < 1)
                    {
                       changeAction(spiderAction.idle);
                    }
                }
                break;
            case spiderAction.none:
                if(!anim.isPlaying)
                anim.Play("Idle");
                break;
        }

      
    }
    void rotation()
    {
        Vector3 looktarget = player.position;
        Vector3 direction = looktarget - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotationSpeed);
        }
    void attack()
    {
        string atk="";
        int rnd = UnityEngine.Random.Range(1, 4);
        switch (rnd)
        {
            case 1:atk = "Attack";
                break;
            case 2: atk = "Attack_Left";
                break;
            case 3:atk = "Attack_Right";
                break;
            default:atk = "Attack";
                break;
        }
        anim.Play(atk);
    }
    void changeAction(spiderAction ac)
    {
        currentAction = ac;
    }
    public enum spiderAction
    {
        idle,
        chasing,
        attack,
        mime,
        none
    }
    #endregion

    IEnumerator moveInArcCo(Vector3 startpos, Vector3 endPos, float speed, float height)
    {
        
        while (moveInArcToNextNode(startpos, endPos, speed, height))
        {
            yield return null;
        }
        cTime = 0;
        jumping = false;
    }
    //プレーヤーと同じ飛ぶ処理
    bool moveInArcToNextNode(Vector3 startpos, Vector3 endPos, float speed, float height)
    {
        cTime += speed * Time.deltaTime;
        Vector3 myPOSITION = Vector3.Lerp(startpos, endPos, cTime);
        //V3.y+=height*sin(0,time)*p greco
        myPOSITION.y += height * Mathf.Sin(Mathf.Clamp01(cTime) * Mathf.PI);
        return endPos != (transform.position = Vector3.Lerp(transform.position, myPOSITION, cTime));
    }
    public void addArcpointToRoute(Transform point, float height, float speed)
    {
        if (jumpPoint == point) return;
        jumping = true;
        jumpPoint = point;
        transform.LookAt(point, Vector3.up);
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        StartCoroutine(moveInArcCo(transform.position, point.position, speed, height));
    }
    //ダメージ処理
    public void interact(Vector3 hitPos)
    {
        if (isdeath) return;
        hp -= 10;
        if (EffectDirector.instance != null)
        {
            EffectDirector.instance.playInPlace(hitPos, StaticStrings.blood);
        }
        if (hp <= 0)
        {
            isdeath = true;
            if (onBossDeath != null)
            {
                onBossDeath();
            }
           StartCoroutine(endCo());
        }
    }
    //バトル終わり
    IEnumerator endCo()
    {
        anim.Play("Death");
        yield return new WaitForSeconds(2);
        foreach(var p in platforms)
        {
            if (p != null)
            {
                Destroy(p.gameObject);
            }
        }
        ADV_Hurt[] hurts = GetComponentsInChildren<ADV_Hurt>();
        foreach(var h in hurts)
        {
            if (h != null)
            {
                Destroy(h);
            }
        }
        yield return new WaitForSeconds(1);
        player.GetComponent<Rigidbody>().isKinematic = false;
        broom.SetActive(true);
        broom.GetComponent<Broom>().passPlayerAdJumpPoint(player.gameObject, jumpingPoint);
        if (Soundmanager.instance != null)
        {
            Soundmanager.instance.PlayBgmByName("Adventure");
        }
       
    }
    //broomにreferenceを渡し
    public void passObjects(GameObject b,GameObject jumpPoint)
    {
        broom = b;
        jumpingPoint = jumpPoint;
    }

}

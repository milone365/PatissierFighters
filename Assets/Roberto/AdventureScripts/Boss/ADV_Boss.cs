using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADV_Boss : MonoBehaviour
{
    public PhasepartActinon[] currentPhaseParts = null;
    
    [SerializeField]
    GameObject[] heads = null;
    [SerializeField]
    ADV_Bullet bullet = null;
    [SerializeField]
    float bulletSpeed = 50;
    [SerializeField]
    Transform upGameObject = null;
    public Transform get_UpObject()
    {
        return upGameObject;
    }
    [SerializeField]
    GameObject objectToActive = null;
    //プレーヤーの後ろあるきゅぶ
    [SerializeField]
    Transform cube = null;
    //段階
    [SerializeField]
    int phaseIndex = 0;
    public List<BossPhase> Phases=null;
    //カメラ
    [SerializeField]
    GameObject c_amera = null;
    //particle
    public Transform middlePortal = null;
    //hp
    BossHealth health;
    //update停止
    [HideInInspector]
    public bool wait = true;
    //削除したいオブジェクト
    [SerializeField]
    GameObject[] objectToDestroy = null;
    //player transform
    [SerializeField]
    Transform Lisa = null;
    //プレーヤーを渡すため
    public Transform getPlayer()
    {
        return Lisa;
    }
    //particleｓ
    ParticleSystem [] vortexes;
    //頭
    [SerializeField]
    Transform Head = null;

    //ラグビーロボット
    [SerializeField]
    GameObject[] robots = null;
    GameObject battleCanvas;
    //
    public Transform getHead()
    {
        return Head;
    }
    public Transform Get_player()
    {
        return Lisa;
    }
   //手
   public List<Transform> hands = new List<Transform>();
    //スポンしたい敵
    [SerializeField]
    GameObject enemy = null;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        //段 階に入る、アニメーターを渡す

        Phases[0].ENTERSTATE(this,anim);
        health = GetComponentInParent<BossHealth>();
        //イベント
        health.onDeath += onDead;
        wait = false;
        vortexes = GetComponentsInChildren<ParticleSystem>();

    }

    void Update()
    {
        if (wait) return;
        //現在の段階処理
        Phases[phaseIndex].UPDATE();
    }

    void onDead()
    {
        //処理止まる
        wait = true;
        //段階から出る前の関数を呼び出す
        Phases[phaseIndex].EXITSTATE();
        //次のへ行く
        phaseIndex++;
        StartCoroutine(ShowEnd());
        
    }

     IEnumerator ShowEnd()
    {
        SaveGame sv = FindObjectOfType<SaveGame>();
        if (sv != null)
        {
            sv.saveGame();
        }
        //最後の段階だったら削除、そではなければ次の段階に入ります
        if (phaseIndex > Phases.Count - 1)
        {
            yield return new WaitForSeconds(1);
            health.onDeath -= onDead;
            yield return new WaitForSeconds(2);
            if (objectToActive != null)
                objectToActive.SetActive(true);
            foreach (var o in objectToDestroy)
            {
                Destroy(o.gameObject);
            }
        }
            else
        {
            //段階に入る
            yield return new WaitForSeconds(1);
            Phases[phaseIndex].ENTERSTATE(this, anim);
                wait = false;
        }
        
    }

    #region Spawn&AttackFunctions
    //手からエネミーを作る
    public void spawnEnemy()
    {
        if (hands == null) return;
        foreach(var h in hands)
        {
            if (h == null)continue;
          GameObject newEnemy=  Instantiate(enemy,h.position,Quaternion.identity);
            IFollow follow = newEnemy.GetComponent<IFollow>();
            follow.follow(Lisa);
        }
    }
    //ゴキブリを作る
    public void spawnBug()
    {
        if (hands == null) return;
        foreach (var h in hands)
        {
            if (h == null) continue;
            GameObject newEnemy = Instantiate(enemy, h.position, Quaternion.identity);
            ADV_Bug bug = newEnemy.GetComponent<ADV_Bug>();
            bug.follow(cube);
            bug.GiveHead(getHead());
            
        }
    }
    //ロボットの攻撃
    public void robotsAttack()
    {
        StartCoroutine(robotAtkCo());
    }
    IEnumerator robotAtkCo()
    {
        foreach (var r in robots)
        {
            if (r != null)
            {
                r.GetComponent<Animator>().SetTrigger("ATK");
                yield return new WaitForSeconds(1.5f);
            }
        }
    }
    //敵の種類を変える
    public void changeEnemyToSpawn(GameObject g)
    {
        enemy = g;
    }
    //
    public void activeRobots()
    {
        foreach(var r in robots)
        {
            if (r != null)
                r.SetActive(true);
        }
    }
    #endregion


    #region battleDesignFunctions
    public void removeElementFromList(List<Transform> lista,Transform t)
    {
         lista.Remove(t);
       
    }
    public void addElentToList(Transform t)
    {
        hands.Add(t);
    }
    public void removeElementFromList(Transform t)
    {
        hands.Remove(t);
    }
    //particles active
    public void activeVortex()
    {
        foreach(var v in vortexes)
        {
            if (v != null)
            {
                v.Play();
            }
        }
    }
    //particle削除
    public void destroyVortex()
    {

        foreach (var v in vortexes)
        {
            if (v != null)
            {
                Destroy(v.gameObject);
            }
        }
    }
    public void deactiveCamera()
    {
        if (c_amera == null) return;
        c_amera.SetActive(false);
    }
    //バレットの攻撃
    public void RayAttack()
    {
        if (bullet == null) return;
        foreach(var h in heads)
        {
            if (h == null) continue;
            ADV_Bullet newBullet = Instantiate(bullet, h.transform.position, h.transform.rotation);
            targetShoot(newBullet);
        }
    }
   //ターゲットに攻撃
    void targetShoot(ADV_Bullet b)
    {
        b.shoot(Lisa, bulletSpeed);
    }
   void spreadShoot(ADV_Bullet b)
    {
        b.SpreadShoot(transform.right, bulletSpeed);
    }
  
    //activate robot parts hp change with parts num
    public void activePhaseParts()
    {
        foreach(var p in currentPhaseParts[phaseIndex].PhaseParts)
        {
            if (p != null)
            {
                p.GetComponent<Collider>().enabled = true;
                p.GetComponent<BossPart>().enabled = true;
            }
        }
    }
    //手active
    public void activeHeads()
    {
        foreach(var h in heads)
        {
            if (h != null)
            {
                h.SetActive(true);
            }
        }
    }
    #endregion
}

[System.Serializable]
public class PhasepartActinon
{
    public BossPart[] PhaseParts=null;
    
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using td;
public class TD_Character : MonoBehaviour
{
    [SerializeField]
    Transform hand=null;
    GameObject model = null;
    public Team team;
    bool HaveTheBall = false;
    bool canmove = false;
    protected Vector3 startPosition = Vector3.zero;
    protected Quaternion startRotation;
    public bool getBall() { return HaveTheBall; }
    public bool canMove { get {return canmove; } set {canmove=value; } }
    protected Animator anim;
    float respawnTime = 3;
    bool isDead = false;
    public virtual void haveBall(bool v)
    {
        HaveTheBall = v;
    }
    //最初の場へ戻る
    public virtual void reset()
    {
        canmove = true;
        transform.position = startPosition;
        transform.rotation = startRotation;
    }
    //リスポーン
    public virtual void respawning()
    {
        transform.position = startPosition;
        transform.rotation = startRotation;
    }
    public Transform getHand()
    {
        return hand;
    }
    
    public virtual void Init()
    {
        Transform[]  childs= GetComponentsInChildren<Transform>();
        foreach(var c in childs)
        {
            if (c.tag == "model")
            {
                model = c.gameObject;
                break;
            }
        }
        startRotation = transform.rotation;
        startPosition = transform.position;
        canmove = true;
    }
    public virtual void updating()
    {
        if (!canmove) return;
        if (isDead)
        {
            respawnTime -= Time.deltaTime;
            if (respawnTime <= 0)
            {
                Dead(false);
                GetComponent<TD_HealthManager>().respawn();
            }
        }
    }
    public virtual void block() { canmove = false; }
    private void Start()
    {
        Init();
    }
    private void Update()
    {
       
        updating();
    }
   //死
    public virtual void Dead(bool v)
    {
        if(v)
        respawning();

        isDead = v;
        if(model!=null)
        model.SetActive(!v);
        respawnTime = 3;
    }
}

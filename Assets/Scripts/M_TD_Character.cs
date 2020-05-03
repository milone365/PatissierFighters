using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using td;
using UnityEngine.Networking;
public class M_TD_Character :NetworkBehaviour,ITeam
{
    [SerializeField]
    Transform hand=null;
    GameObject model = null;
    public Team team;
    [SyncVar]
    public bool HaveTheBall = false;
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
    public virtual void reset()
    {
        canmove = true;
        transform.position = startPosition;
        haveBall(false);
    }
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
                GetComponent<M_TD_HealthManager>().respawn();
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
   
    public virtual void Dead(bool v)
    {
        if(v)
        respawning();

        isDead = v;
        if(model!=null)
        model.SetActive(!v);
        respawnTime = 3;
    }

    public Team getTeam()
    {
        return team;
    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;
using td;


public class SetupLocalPlayer : M_TD_Character,IHealth
{
    Slider hpBar;
    [SerializeField]
    int maxHp = 12;
    Text hptext;
    GameObject canvas;
    public Transform barPosition;
    //spawnpos
    public NetworkStartPosition respawnposition;
    [SyncVar(hook = "OnChangeAnimation")]
    public string animState = "idle";
    M_StateManager statemanager;
    [SyncVar(hook = "OnChangeHealth")]
    public int healthValue = 12;
    
	void Start () 
	{
        base.Init();
        MU_TD_GameManager gamemanager = FindObjectOfType<MU_TD_GameManager>();
        statemanager = GetComponent<M_StateManager>();
        if(gamemanager!=null)
        {
            gamemanager.setResetableObjects(this.gameObject);
        }
		if(isLocalPlayer)
		{
			GetComponent<M_InputHandler>().enabled = true;
        }
		else
		{
            GetComponent<M_InputHandler>().enabled = false;
		}
        anim = GetComponent<Animator>();
        
        hpBar = GetComponentInChildren<Slider>();
        hpBar.maxValue = maxHp;
        hpBar.value = healthValue;
        hptext = hpBar.GetComponentInChildren<Text>();
        hptext.text = healthValue + "/" + maxHp;
        //find respawnPoint
        NetworkStartPosition[] allStartPosition = FindObjectsOfType<NetworkStartPosition>();
        float maxDistance = float.MaxValue;
        foreach (var a in allStartPosition)
        {
            float dist = Vector3.Distance(transform.position, a.transform.position);
            if (dist <= maxDistance)
            {
                maxDistance = dist;
                respawnposition = a;
            }
        }
    }
    #region HEALTH
    private void Update()
    {
        
    }
    void OnChangeHealth(int n)
    {
        healthValue = n;
        hpBar.value = healthValue;
        hptext.text = healthValue + "/" + maxHp;
    }
    private void UpdateHealthBar(float value)
    {
        hpBar.value = value;
        hptext.text = healthValue + "/" + maxHp;
    }
    [Command]
    public void CmdChangeHealth(int hitValue)
    {
        TdMg(hitValue);
        RpcTakingDamage(hitValue);
    }
    [ClientRpc]
    void RpcTakingDamage(int v)
    {
        if(!isServer)
        TdMg(v);
    }
    void TdMg(int hitValue)
    {
        healthValue = healthValue + hitValue;

        if (healthValue <= 0)
        {
            EffectDirector.instance.playInPlace(transform.position, StaticStrings.SKULL);
            if (Soundmanager.instance != null)
                Soundmanager.instance.PlaySeByName(StaticStrings.deathMale);
            healthValue = 0;
            StartCoroutine(respawning());
        }
        hpBar.value = healthValue;
        hptext.text = healthValue + "/" + maxHp;
    }
    public void Healing(float heal)
    {
        healthValue += (int)heal;

        if (healthValue >= maxHp)
        {
            healthValue = maxHp;

        }
    }

    public void takeDamage(float damageToTake)
    {
       // if (!isLocalPlayer) return;
        CmdChangeHealth((int)-damageToTake);
    }
 
IEnumerator respawning()
{
    CmdChangeAnimState(StaticStrings.death);
        yield return new WaitForSeconds(2f);
        if (HaveTheBall)
        {
            haveBall(false);
            M_TD_PancakeBall.instance.loseBall();
        }
        teleportToSpawnPoint();
        Healing(maxHp);
        EffectDirector.instance.playInPlace(transform.position, StaticStrings.RESURRECTION);
}

public void teleportToSpawnPoint()
{
    transform.position = respawnposition.transform.position;
}

public void respawn()
{
    teleportToSpawnPoint();
}


#endregion
    
    #region animationSender
    void OnChangeAnimation(string s)
    {
        if (isLocalPlayer) return;
        UpdateAnimationState(s);
    }

    [Command]
    public void CmdChangeAnimState(string s)
    {
        UpdateAnimationState(s);
        RpcAnimateUpdating(s);
    }
    [ClientRpc]
    void RpcAnimateUpdating(string s)
    {
        if (!isServer)
        {
            UpdateAnimationState(s);
        }
    }
     void UpdateAnimationState(string s)
    {
       // if (animState == s) return;
        anim.SetTrigger(s);
    }
    #endregion

    public void spawnFromServer(GameObject obj)
    {
        NetworkServer.Spawn(obj);
    }

}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AttackingDetector : MonoBehaviour
{
   
    [SerializeField]
    float colliderRadius = 10;
    Team _team;
    CPUBrain brain;
    [SerializeField]
    float reactionTime = 40;
    bool interacting = true;
    float actionTime;
    private void Start()
    {
       _team = GetComponent<PancakeTower>().getTeam();
       gameObject.AddComponent<SphereCollider>();
       SphereCollider collider= GetComponent<SphereCollider>();
        collider.isTrigger = true;
        collider.radius = colliderRadius;
        actionTime = reactionTime;
        
    }

    private void Update()
    {

        if (!interacting)
        {
            actionTime -= Time.deltaTime;
            if (actionTime <= 0)
            {
                interacting = true;
                actionTime = reactionTime;
            }
        }
    }
   
    private void OnTriggerEnter(Collider other)
    {
        if (interacting) return;
        if (other.tag == StaticStrings.player ||other.tag==StaticStrings.bullet)
        {
            check(other,true);
            
        }
        if(other.tag == StaticStrings.helper)
        {
            check(other, false);
        }
        interacting = false;
    }
    
    //enemy check
    void check(Collider other, bool value)
    {
        Team team = other.GetComponent<ITeam>().getTeam();
        if (team != _team)
        {
             onTakingDamage(team,value);
        }
    }
    //脳にメッセージを送る
    public void onTakingDamage(Team team,bool isPlayer)
    {
        brain.onDamageReaction(team, isPlayer);
    }
    //reference
    public void brainConnetting(CPUBrain b)
    {
        brain = b;
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="td_enemyStatus", menuName = "Td_stat/new stat")]
public class TD_Status : ScriptableObject
{
    public float speed = 0;
    public int health = 3;
    public float reactionTime = 5;
    public float attack = 1;
    public float force = 50;
    public float patrollingTime = 5;
    public float patrollingCounter = 5;
    public Vector3[] patrollPoints = new Vector3[2];
    public Vector3 startPosition = Vector3.zero;
    public Quaternion startRotation = Quaternion.identity;
    public bool haveBall = false;
    public float chaseRange = 10;
    public float attackRange = 1;
    public float attackingCounter = 2;
    public float attackingTime = 2;
}

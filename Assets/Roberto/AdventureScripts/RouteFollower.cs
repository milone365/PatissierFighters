using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouteFollower : MonoBehaviour
{
    public RouteType route_type = RouteType.circuit;
    [SerializeField]
    bool dontGoUpCharacter = true;
    [SerializeField]
    List<Transform> wayPoints = new List<Transform>();
    List<Vector3> positions = new List<Vector3>();
    int pointIndex = 0;
    Vector3 destination = Vector3.zero;
    [SerializeField]
    float moveSpeed = 5;
    [SerializeField]
    float rotateSpeed = 2;
    [SerializeField]
    bool walkingCharacter = false;
    ADV_Player player;
    [SerializeField]
    float distanceRange = 4;
    //[Header("arc movement")]
    float cTime = 0f;
    bool canMove=true;
    Transform point;
    [SerializeField]
    float animationSpeed = 1;
    bool noWayPoints()
    {
        return positions.Count < 1;
    }
    private void Start()
    {
　　　 foreach (var point in wayPoints)
        {
            if (point == null) continue;
            positions.Add(point.position);
        }
        if (noWayPoints()) return;
        FindDestination();
    }
 
    void Update()
    {
        if (noWayPoints()) return;
        if (!canMove) return;
        move();
        rotate();
    }

    //移動
    void move()
    {
        float distance = Vector3.Distance(transform.position, destination);
        if (distance <= distanceRange)
        {
            routeTypeCheck();
            
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);
            if (walkingCharacter)
            {
                GetComponent<Animator>().SetFloat(StaticStrings.move, animationSpeed);
            }
        }

    }
    //ポイントへ回る
    void rotate()
    {
        if (transform.position == destination) return;
        transform.LookAt(destination);
        Vector3 look = destination - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(look), Time.deltaTime * rotateSpeed);
    }
    //飛ぶ
     IEnumerator  moveInArcCo(Vector3 startpos, Vector3 endPos, float speed,float height)
    {
        while (moveInArcToNextNode(startpos, endPos, speed,height)) 
        {
            yield return new WaitForEndOfFrame();
        }
        cTime = 0;
    }
    //次のポイントを探す
    public void FindDestination()
    {
        
        Vector3 dir = positions[pointIndex];
        Vector3 newDir;
        if (dontGoUpCharacter)
         newDir = new Vector3(dir.x, transform.position.y, dir.z);
        else 
            newDir= new Vector3(dir.x, dir.y, dir.z);
        destination = newDir;
    }
    //飛ぶの処理
    bool moveInArcToNextNode(Vector3 startpos, Vector3 endPos, float speed,float height)
    {
        cTime += speed * Time.deltaTime;
        Vector3 myPOSITION = Vector3.Lerp(startpos, endPos, cTime);
        //V3.y+=height*sin(0,time)*p greco
        myPOSITION.y += height * Mathf.Sin(Mathf.Clamp01(cTime) * Mathf.PI);
        return endPos != (transform.position = Vector3.Lerp(transform.position, myPOSITION, cTime));
    }
    void routeTypeCheck()
    {
        switch (route_type)
        {
            //最後のポイントについたらゼロからもう一度始まる
            case RouteType.circuit:
                pointIndex++;
                pointIndex %= positions.Count;
                FindDestination();
                break;
                //最後のポイントに着いたら止まる
            case RouteType.straight:
                if (pointIndex == positions.Count - 1)
                {
                    if (walkingCharacter)
                    {
                        GetComponent<Animator>().SetFloat(StaticStrings.move, 0);
                    }
                    return;
                }
                else
                {
                    pointIndex++;
                    FindDestination();
                }
               
                break;
                //永遠ランダムで動く
            case RouteType.random:
                int rnd = Random.Range(0, positions.Count - 1);
                pointIndex = rnd;
                FindDestination();
                break;
        }
        
    }
    //道を変更する関数
    #region ChangeRoutes

    public void changeRoute(List<Transform> newRoute)
    {
        pointIndex = 0;
        positions.Clear();
        canMove = false;
        foreach (var n in newRoute)
        {
            addWayPoint(n);
        }
        
    }
    public void addRoute(List<Transform> newRoute)
    {
        canMove = false;
        foreach (var n in newRoute)
        {
            addWayPoint(n);
        }

    }
    public void addWayPoint(Transform t)
    {
        if (t == null) return;
        positions.Add(t.position);
        FindDestination();
        canMove = true;
       
    }
    public void addWayPoint(Vector3 d)
    {
        positions.Add(d);
        FindDestination();
        canMove = true;
        
    }
    public void JumpToPoint(Transform point,float height,float speed)
    {
        if (this.point == point) return;
        this.point = point;
        canMove = false;
        transform.LookAt(point, Vector3.up);
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        StartCoroutine(moveInArcCo(transform.position, point.position, speed, height));
    }
    #endregion

 
    public enum RouteType
{
    circuit,
    straight,
    random
}
}
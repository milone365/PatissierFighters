using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class M_PancakeSpawner : NetworkBehaviour
{
    [SerializeField] float spawningTime = 5;
    float spawningTimer = 2;
    [SerializeField]
    GameObject pancake = null;
    [SerializeField]
    ParticleSystem smoke = null;
    public List<Transform> pancakes = new List<Transform>();
    public List<Transform> cookedList = new List<Transform>();
    public static M_PancakeSpawner instance;
    [SerializeField]
    Transform[] spawnpoints = null;
    List<Spawnpoints> sp_Points = null;
    bool start = false;

    public void startGame()
    {
        start = true;
    }

    [Command]
    public void CmdStartGame()
    {
        startGame();
    }
    [ClientRpc]
    void RpcStartGame()
    {
        if (!isServer)
        {
            startGame();
        }
    }
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        spawningTimer = spawningTime;
        sp_Points = new List<Spawnpoints>();
        for (int i = 0; i < spawnpoints.Length; i++)
        {
            Spawnpoints sp = new Spawnpoints();
            sp_Points.Add(sp);
            sp_Points[i].point = spawnpoints[i];
        }
        Invoke("CmdStartGame", 10);
    }
    void Update()
    {
        if (!start) return;
        spawningTimer -= Time.deltaTime;
        if (spawningTimer <= 0)
        {

            CmdSpawn();

        }

    }



    IEnumerator waitCo()
    {
        smoke.Play();
        yield return new WaitForSeconds(1);
        sortList();
        for (int i = cookedList.Count - 1; i > cookedList.Count - 5; i--)
        {
            if (cookedList[i].transform == null) continue;
            PancakeMovement m = cookedList[i].gameObject.GetComponent<PancakeMovement>();
            if (m != null)
            {
                m.go();
            }
        }

        for (int i = 0; i < sp_Points.Count; i++)
        {
            sp_Points[i].canSpawn = true;

        }

        yield return new WaitForSeconds(3);

        start = true;
        transform.Rotate(Vector3.up, 30);
    }
    public void spawn(Transform t)
    {
        GameObject newPancake = Instantiate(pancake, t.position, t.rotation) as GameObject;
        cookedList.Add(newPancake.transform);


    }

    public void RemovePancake(Transform obj)
    {
        pancakes.Remove(obj);

    }

    public void addTolist(Transform t)
    {

        pancakes.Add(t);
        cookedList.Remove(t);

    }
    void sortList()
    {
        List<Transform> temp = new List<Transform>();
        foreach (var item in cookedList)
        {
            if (item != null)
            {
                temp.Add(item);
            }
        }
        cookedList = temp;
    }

    public class Spawnpoints
    {
        public Transform point = null;
        public bool canSpawn = true;
    }

    void spawn()
    {
        spawningTimer = spawningTime;
        foreach (var s in sp_Points)
        {
            if (s.canSpawn)
            {
                spawn(s.point);
                s.canSpawn = false;
                if (sp_Points[sp_Points.Capacity - 1].canSpawn == false)
                {
                    start = false;
                    StartCoroutine(waitCo());
                }
                break;
            }
        }
    }

    [Command]
    void CmdSpawn()
    {
        spawn();
        RpcSpawn();
    }
    [ClientRpc]
    void RpcSpawn()
    {
        if (!isServer)
        {
            spawn();
        }
    }
}

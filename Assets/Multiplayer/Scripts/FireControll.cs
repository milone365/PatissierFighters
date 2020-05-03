using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class FireControll : NetworkBehaviour
{
    public GameObject bullet = null;
    public Transform bulletSpawn = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CmdShoot();
        }

    }
    [Command]
    void CmdShoot()
    {
        GameObject newBullet = Instantiate(bullet, bulletSpawn.position, bulletSpawn.rotation).gameObject;
        newBullet.GetComponent<Rigidbody>().velocity = bulletSpawn.forward * 50;
        NetworkServer.Spawn(newBullet);
        Destroy(newBullet, 3);
    }
}

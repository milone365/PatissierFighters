using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

	/*public GameObject bulletPrefab;
	public NetworkHash128 assetId { get; set; }

	void Start()
	{
		assetId = bulletPrefab.GetComponent<NetworkIdentity> ().assetId;
		ClientScene.RegisterSpawnHandler(assetId, SpawnBullet, UnSpawnBullet);
	}
	
	public GameObject SpawnBullet(Vector3 position, NetworkHash128 assetId)
	{
    	GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
		bullet.GetComponent<Rigidbody>().velocity = bulletSpawn.transform.forward*50;
		Destroy(bullet,3.0f);
		Debug.Log("Spawning Bullet");
    	return bullet;
	}
	
	public void UnSpawnBullet(GameObject b)
	{
    	Destroy(b);
	}*/
}

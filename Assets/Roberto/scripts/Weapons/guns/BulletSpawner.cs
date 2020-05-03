using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using td;
using SA;
public class BulletSpawner : MonoBehaviour
{
    [SerializeField]
    protected WPbullet weaponBullet = null;
    WPbullet currentBullet;
    [SerializeField]
    WPbullet specialWeaponBullet = null;
    [SerializeField]
    GameObject wpModel=null, SpecialWpModel=null;
    [SerializeField]
    Transform spawnPoint = null;
    public Sprite weaponImage = null;
    public AnimatorOverrideController controller;
    
    public Transform getSpawnpoint()
    {
        return spawnPoint;
    }
    B_Player p;
    Vector3 direction = new Vector3();
    int specialBullet = 0;
    public delegate void usingDifferentBullet();
    public usingDifferentBullet bulletSpawning;

    public void INITIALIZING(B_Player play)
    {
        p = play;
        p.BULLETSPAWNER = this;
        direction = transform.forward;
        bulletSpawning = shoot;
        currentBullet = weaponBullet;
        
    }

   public void changeWeapon()
    {
        if (specialWeaponBullet == null)
        {
            return;
        }
        if (!SpecialWpModel.activeInHierarchy)
        {
            SpecialWpModel.SetActive(true);
            wpModel.SetActive(false);
            currentBullet = specialWeaponBullet;

        }
        else
        {
            wpModel.SetActive(true);
            SpecialWpModel.SetActive(false);
            currentBullet = weaponBullet;
            
        } 
    }
    public void shot()
    {
        bulletSpawning();
        
    }

    void shoot()
    {
        WPbullet newBullet = Instantiate(currentBullet, spawnPoint.position, spawnPoint.transform.rotation);
        newBullet.passDirection(p,direction);
    }
    public void changeToSpecialShoot(int value, WPbullet bullet)
    {
        currentBullet = bullet;
        specialBullet = value;
        bulletSpawning = specialShoot;
    }

    public void specialShoot()
    {
        WPbullet newBullet = Instantiate(currentBullet, spawnPoint.position, spawnPoint.transform.rotation);
        newBullet.passDirection(p,direction);
        specialBullet--;
        if (specialBullet <= 0)
        {
            specialBullet = 0;
            bulletSpawning = shoot;
            currentBullet = weaponBullet;
            if (p.getCanvas() == null) return;
            p.getCanvas().resetIngredientImage();
        }
    }
    public void GiveDirection(Vector3 dir)
    {
        direction = dir;
    }
}

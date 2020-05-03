using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace td
{


    public class SV_PickUP : MonoBehaviour
    {
        public pickUpType Type = pickUpType.plin;
        bool isTaked = false;
        public Ammonation plin;
        public Ammonation bullet;
        public BOMB bomb;
        public Sprite bulleticon; 
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == StaticStrings.player)
            {
                if (isTaked) return;
                isTaked = true;
                switch (Type)
                {
                    //プリントラップをとる
                    case pickUpType.plin:
                        other.GetComponent<SkillKeaper>().addPlin(plin);
                        break;
                        //バレットを変える
                    case pickUpType.bullet:
                        TD_BulletSpawner spawner = other.GetComponentInChildren<TD_BulletSpawner>();
                        spawner.changeBullet(bullet);
                        spawner.canvas.changeIngredientImage(bulleticon);
                        break;
                        //爆弾追加
                    case pickUpType.bomb:
                        other.GetComponent<TD_StateManager>().addBomb(bomb);
                        break;
                }

                Destroy(gameObject);
            }
        }
        public enum pickUpType
        {
            plin,
            bullet,
            bomb
        }
    }
    
}
[System.Serializable]
public class Ammonation
{
   public GameObject obj;
   public int Ammo;
   public WPbullet bullet = null;
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace td
{
    public class TD_BulletSpawner : MonoBehaviour
    {
        //打つ関数を変更する為
        delegate void ONSHOOT();
        ONSHOOT onShot;
        [HideInInspector]
        public PlayerCanvas canvas;
        [SerializeField]
        protected WPbullet weaponBullet = null;
        WPbullet currentBullet;
        [SerializeField]
        Transform spawnPoint = null;
        int Ammo = 0;
        Camera cameraMain;
        bool isPlayer;
        string playerId = "P1_";
        Ammonation ammonation;
        public Transform getSpawnpoint()
        {
            return spawnPoint;
        }
        Transform tg;
        TD_StateManager statemanager;
        Vector3 direction = new Vector3();
        SkillKeaper keaper;

        public bool L2_input()
        {
            return Input.GetButton(playerId + StaticStrings.L2_key);
        }

        public void INITIALIZING(TD_StateManager state)
        {
            ammonation = new Ammonation();
            ammonation.bullet = weaponBullet;
            currentBullet = weaponBullet;
            statemanager = state;
            statemanager.BULLETSPAWNER = this;
            statemanager.status.getCurrentweapon.bullet = weaponBullet;
            direction = transform.forward;
            cameraMain = statemanager.FollowCamera;
            isPlayer = true;
            onShot = NormalShot;
            playerId = state.playerId;
            canvas = FindObjectOfType<PlayerCanvas>();
            keaper = GetComponentInParent<SkillKeaper>();
        }
        private void Update()
        {
            if (!isPlayer) return;
            //シュートの方向

            if (L2_input())
            {
                tg = cameraMain.transform;
                Ray ray = new Ray(tg.position, cameraMain.transform.forward);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    Vector3 newDir = hit.point;
                    direction = newDir - transform.position;
                }
            }
            else
            {
                tg =statemanager.transform ;
                Vector3 newDire = statemanager.transform.forward * 50;
                direction = newDire - transform.position;
            }
           
            
            Debug.DrawRay(tg.position, cameraMain.transform.forward * 50,Color.blue);
        }

    
       public void shoot()
        {
            onShot(); 
        }
        //ディフォルトの無限バレット
        public void NormalShot()
        {
            
            WPbullet newBullet = Instantiate(weaponBullet, spawnPoint.position, spawnPoint.transform.rotation);
            newBullet.passDirection(direction);
        }
        //軍需品を打つ
        void specialShot()
        {
            WPbullet newBullet = Instantiate(currentBullet, spawnPoint.position, spawnPoint.transform.rotation);
            newBullet.passDirection(direction);
            Ammo--;
            if (Ammo <= 0)
            {
                //普通の関数に戻る
               changeBullet(ammonation);
            }
        }
        //バレットの種類によって打つ関数は違う
        public void changeBullet(Ammonation a)
        {
            Ammo = a.Ammo;
            if (Ammo > 0)
            {
               onShot = specialShot;
                
            }
            else
            {
               onShot = NormalShot;
                if (canvas != null)
                {
                    canvas.changeIngredientImage(canvas.cream);
                    canvas.changeWeaponImage(false);
                }
                    
                if (keaper != null)
                {
                    keaper.deativeAura();
                }
            }
            currentBullet = a.bullet;
        }
    }

}

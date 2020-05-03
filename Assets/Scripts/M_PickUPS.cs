using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SA;
using UnityEngine.Networking;
using td;
public class M_PickUPS : NetworkBehaviour
{
    public itemType ItemType = itemType.bullet;
    [SerializeField]
    WPbullet bullet = null;
    [SerializeField]
    Sprite itemSprite = null;
    [SerializeField]
    int Ammo = 15;
    [SerializeField]
    GameObject itemCanvas = null;
   
   


    private void OnTriggerStay(Collider other)
    {
        
        if (other.tag==StaticStrings.player)
        {
            if (Input.GetButtonDown(StaticStrings.Circle_key))
            {
                take(other);
            }
            
        }
    }


    public void take(Collider other)
    {
      switch (ItemType)
        {
            case itemType.bomb:
                IpickUp pc = other.GetComponent<IpickUp>();
                if(pc!=null)
                pc.take(1);
                EffectDirector.instance.playInPlace(transform.position, StaticStrings.PICKUPSTAR);
                break;
            case itemType.bullet:
                M_BulletSpawner bs = other.GetComponent<M_BulletSpawner>();
                if (bs != null)
                    bs.CmdChangeBullet(bullet.gameObject.name);
                EffectDirector.instance.playInPlace(transform.position, StaticStrings.PICKUPDIAMOND);
             
                break;
            case itemType.ingredient:
                Imana mana = other.GetComponent<Imana>();
                if (mana != null)
                {
                    mana.addMp(50);
                }
                break;
                
        }

        NetWorkCleaner.Destroy(this.gameObject);
        RpcDestroy();
        Destroy(gameObject);

    }
    [ClientRpc]
    void RpcDestroy()
    {
        if (!isServer)
        {
            Destroy(gameObject);
        }
    }
   
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CPUBrain>())
        {
            take(other);
        }
    }


}

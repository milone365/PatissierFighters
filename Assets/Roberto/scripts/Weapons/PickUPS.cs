using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SA;

public class PickUPS : MonoBehaviour
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
    public Team tm;
   

    //丸を押したら取る
    private void OnTriggerStay(Collider other)
    {
        Inputhandler inp = other.GetComponent<Inputhandler>();
        if (inp)
        {
            if (Input.GetButtonDown(StaticStrings.Circle_key))
            {
                take(other);
            }
            if (itemCanvas != null)
                itemCanvas.SetActive(true);
        }
    }
    //メッセージを消す
    private void OnTriggerExit(Collider other)
    {
        Inputhandler inp = other.GetComponent<Inputhandler>();
        if (inp)
        {
           if (itemCanvas != null)
                itemCanvas.SetActive(false);
        }
    }

    public void take(Collider other)
    {
        B_Player p = other.GetComponentInChildren<B_Player>();
        if (p == null) return;
        switch (ItemType)
        {
            //爆弾追加
            case itemType.bomb:
                p.status.addBomb(1);
                EffectDirector.instance.playInPlace(transform.position, StaticStrings.PICKUPSTAR);
                break;
                //普通から特別なバレットに変更

            case itemType.bullet:
                if (bullet != null)
                    p.BULLETSPAWNER.changeToSpecialShoot(Ammo, bullet);
                EffectDirector.instance.playInPlace(transform.position, StaticStrings.PICKUPDIAMOND);
                //アイコンも変わる
                if (itemSprite != null)
                {
                    if (p.getCanvas() == null) return;
                    p.getCanvas().changeIngredientImage(itemSprite);
                }
                break;
                //材料追加
            case itemType.ingredient:
                Team t = p.getTeam();
                if (t == tm)
                {
                    p.onTakingIngredient();
                    EffectDirector.instance.playInPlace(transform.position, StaticStrings.PICKUPSMILE);
                }
                else
                {
                    return;
                }
                break;
        }
        Destroy(gameObject);

    }
    //アイだったら自動的に取る
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CPUBrain>())
        {
            take(other);
        }
    }


}

public enum itemType
{
    bullet,
    bomb,
    ingredient
}
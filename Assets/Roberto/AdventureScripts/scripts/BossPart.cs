using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BossPart : MonoBehaviour, IEnemy
{
    public barNumber bar;
    [SerializeField]
    Sprite icon=null;
    ADV_BossBar hpbar;
    string barName;
    Slider hpslider;
    public partType type = partType.button;
    [SerializeField]
    float endurance = 100;
    bool destroyed = false;
    [SerializeField]
    string effectName = "explosion";
    BossHealth health;
    [SerializeField]
    GameObject ObjToDeactive = null;
    [SerializeField]
    GameObject sparkle = null;
    [SerializeField]
    Transform vortex = null;
    ADV_Boss boss;
    
    private void Start()
    {
        health =FindObjectOfType<BossHealth>();
        boss = GetComponentInParent<ADV_Boss>();
        health.AddPartToList(this);
        setHpBar();
    }
   
    public void Healing(float heal)
    {
        throw new System.NotImplementedException();
    }

    //ｈｐバー活性
    void setHpBar()
    {
        switch (bar)
        {
            case barNumber.one:barName = "bossbar1";
                break;
            case barNumber.two:
                barName = "bossbar2";
                break;
            case barNumber.three:
                barName = "bossbar3";
                break;
            case barNumber.four:
                barName = "bossbar4";
                break;
        }
        hpbar = GameObject.Find(barName).GetComponent<ADV_BossBar>();
        if (hpbar == null) return;
        hpbar.activeBar(icon, endurance, endurance);
    }
    //ダメージ
    public void takeDamage(float damageToTake)
    {
        if (destroyed) return;
        endurance -= damageToTake;
        if (hpbar == null) return;
        hpbar.takeDamage(endurance);
        if (endurance <= 0)
        {
            destroyed = true;
            if (hpbar == null) return;
            hpbar.deactivateBar();
            if (EffectDirector.instance != null)
            {
                EffectDirector.instance.playInPlace(transform.position, effectName);
            }
            if (Soundmanager.instance != null)
            {
                Soundmanager.instance.PlaySeByName("EXP");
            }
            //部分による結果は違う
            switch (type)
            {
                case partType.button:
                    Destroy(gameObject);
                    break;
                case partType.hand:
                    Hand();
                    break;
                case partType.hat:
                    Hat();
                    break;
                case partType.robots:
                    Destroy(gameObject);
                    break;
                case partType.head:
                    break;
            }
            health.removePartToList(this);
            if (EffectDirector.instance != null)
            {
                EffectDirector.instance.EffectAndPopup(transform.position, "explosion",500);
            }
        }
        else
        {
            EffectDirector.instance.playInPlace(transform.position, "Spark");
        }
    }

    #region battle design functions
    void Hat()
    {
        boss.removeElementFromList(vortex);
        boss.activeRobots();
        Destroy(GetComponent<BoxCollider>());
        transform.tag = "Untagged";
        vortex.gameObject.SetActive(false);
        ObjToDeactive.SetActive(false);
       
    }
     void Hand()
    {
        ObjToDeactive.SetActive(false);
        if (sparkle == null) return;
        GameObject newSpaerkle = Instantiate(sparkle, transform.position, transform.rotation);
        newSpaerkle.transform.SetParent(this.transform);
        newSpaerkle.transform.localScale = new Vector3(0.005f, 0.005f, 0.005f);
        boss.removeElementFromList(vortex);
        vortex.gameObject.SetActive(false);
    }
    #endregion

    public enum barNumber
    {
        one,
        two,
        three,
        four
    }
}

public enum partType
{
    button,
    hand,
    hat,
    robots,
    head,
}


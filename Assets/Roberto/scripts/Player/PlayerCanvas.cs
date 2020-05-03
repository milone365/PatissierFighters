using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCanvas : MonoBehaviour
{
    #region values 
    public List<Sprite> messageIcons = new List<Sprite>();
    string playerID = "";
    [Header("Actions")]
    public GameObject actionMenu;
    //Gear
    [SerializeField]
    public Image arrow;
    public RectTransform arrowCenter;
    //バレットのアイコン
   
    public Sprite cream = null;
    //材料
    [Header("ingredients")]
    public GameObject ingredientsMenu;
    [SerializeField]
    Image ingredientImage = null;
    //パワーアップ
    public Image[] powerUpsImages;
    public Color col;
    //マナ
    [Header("MP,HP,Skill")]
    [SerializeField]
    Image manaImage = null;
    //HP
    [SerializeField]
    Slider healthSlider = null;
    [SerializeField]
    Text healthTXT = null;
    //tower HP
    [SerializeField]
    Text towerText = null;
    [SerializeField]
    Slider TowerSlider = null;
    //CrossAir 画像
    [Header("crossAir")]
    public GameObject crossAir = null;
    [SerializeField]
    //スキルメッセージ
    GameObject skillImage = null;
    [SerializeField]

    Sprite[] targetSprites = null;
    [SerializeField]
    Image[] targetImages = null;
    //小麦粉邪魔画像
    [SerializeField]
    Image flourImage = null;
    float flourTime = 5;
    //references
    B_Player player;
    HealthManager health;
    int currentImage = 0;
    bool initialized;
    bool isdirty = false;
    //爆弾
    [SerializeField]
    Text bombtext = null;
    float alpha = 1;
    [Header("weapons")]
    
    [SerializeField]
    Image weaponImage = null;
    
    public Sprite defaultWeaponImageSprite = null;
    public Sprite SpecialWeaponIcon;
    [SerializeField] Color chocolate=new Color(), vanilla = new Color(), matcha = new Color(), strawberry = new Color();
    [SerializeField]
    Image IngredientIMG = null;
    #endregion

    //CrossAir,小麦粉,skillImage処理
    private void Update()
    {
       crossAir.SetActive(Input.GetButton(playerID+StaticStrings.L2_key));
        updatePlayerUI();
      
        if (isdirty)
        {
            flourTime -= Time.deltaTime;
            alpha -= (0.2f*Time.deltaTime);
            if(flourImage!=null)
            flourImage.color = new Color(flourImage.color.r, flourImage.color.g, flourImage.color.b, alpha);
            if (flourTime <= 0)
            {
                isdirty = false;
                
            }
        }
        if (skillImage == null || player == null) return;
        skillImage.SetActive(player.status.getMP() >= 100);
    }
    //プレイヤーを渡す
    public void Init(B_Player p)
    {
      
        player=p;
        playerID = player.getID();
      if (manaImage != null)
            manaImage.fillAmount = p.status.getMP();

        actionMenu.SetActive(false);
        health = GetComponentInParent<HealthManager>();
        updateTowerUI();
        initialized = true;
        if (skillImage != null)
            skillImageActive(false);
    }
    //HP,MP update
    public void updatePlayerUI()
    {
        if (!initialized) return;
        manaImage.fillAmount = player.status.getMP() / 100;
        healthTXT.text = health.getHealth() +" / "+ health.getMaxHealth();
        healthSlider.maxValue = health.getMaxHealth();
        healthSlider.value = health.getHealth();
        if (bombtext == null) return;
        bombtext.text = player.status.BOMB.ToString();
    }
  
    //tower hp update
   public void updateTowerUI()
    {
        TowerSlider.maxValue = player.Tower.getMaxHealth();
        TowerSlider.value = player.Tower.getHealth();
        towerText.text = player.Tower.getHealth().ToString()+
            " / "+ player.Tower.getMaxHealth();  
    }

    //powerUpImage
    public void goToImage(int value)
    {
        currentImage=value;
        currentImage %= powerUpsImages.Length;
        foreach(var i in powerUpsImages)
        {
            i.color = col;
            
        }
        powerUpsImages[currentImage].color = Color.white;
    }
    
    //材料活性
    public void activeIngredientImage(bool v)
    {
        if (IngredientIMG == null) return;
        IngredientIMG.enabled = v;
    }
    //ぐるまの→の色を変更する
    public void changeArrowColor(Team tm)
    {
        Color c = new Color();
        switch (tm)
        {
            case Team.chocolate:c = chocolate; break;
            case Team.matcha: c = matcha; break;
            case Team.vanilla: c = vanilla; break;
            case Team.strawberry:c=strawberry; break;
        }

        arrow.color = c;
    }
    //グルまの中での画像を設定する
    public void giveTargetImage(int n, int i)
    {
      
       targetImages[i].sprite = targetSprites[i];

    }
    //武器イメージの変更
    public void changeWeaponImage(bool value)
    {
        if (weaponImage == null) return;
        if (value == false)
        {
            weaponImage.sprite = defaultWeaponImageSprite;
            weaponImage.preserveAspect = false;
        }
        else
        {
            weaponImage.sprite = SpecialWeaponIcon;
            weaponImage.preserveAspect = true;
        }
        
    }
    //材料画像の変更
    public void changeIngredientImage(Sprite s)
    {
        ingredientImage.sprite = s;
    }

    //スキル画像活性
    public void skillImageActive(bool v)
    {
        skillImage.SetActive(v);
    }

    public void resetIngredientImage()
    {
        if (cream == null) return;
        changeIngredientImage(cream);
    }
    //小麦粉活性
    public void activeFlour()
    {
        if (flourImage == null) return;
        alpha = 1;
        flourImage.color = new Color(flourImage.color.r, flourImage.color.g, flourImage.color.b, alpha);
        flourTime = 5;
        isdirty = true;
    }
}

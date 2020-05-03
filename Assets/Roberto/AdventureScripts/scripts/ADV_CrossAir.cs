using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADV_CrossAir : MonoBehaviour
{
    float xValue;
    float yValue;
    public Texture2D cursorImage;
    private int cursorWidth = 64;
    private int cursorHeight = 64;
    public float speed = 50f;
    [SerializeField]
    private Vector2 cursorPosition;
   
    float shotvalue;
    bool shot()
    {
        return Input.GetButtonDown(StaticStrings.X_key);
    }


    public Vector2 getCursorPos()
    {
        return cursorPosition;
    }
    float multipler = 25;
    private void Start()
    {
        if (EffectDirector.instance!= null)
        {
            EffectDirector.instance.PassCrossair(this);
        }
        cursorPosition = new Vector2(Screen.width / 2, Screen.height / 2);
       
    }
    //インプット
    private void FixedUpdate()
    {
        xValue = Input.GetAxisRaw(StaticStrings.Horizontal);
        yValue = Input.GetAxisRaw(StaticStrings.Vertical);
        yValue = yValue >= 0.2 ? 1f : yValue <= -0.2 ? -1f : 0;
        xValue = xValue>= 0.2 ? 1f : xValue <= -0.2 ? -1f : 0;
        xValue *= speed * Time.deltaTime*multipler;
        yValue *= speed * Time.deltaTime*multipler;
        cursorPosition.x += xValue;
        cursorPosition.y += yValue;
        
    }
    private void Update()
    {
         if(shot())
              NormalAttack();
  
    }
    
    //crossAirを描く
    private void OnGUI()
    {
         GUI.DrawTexture(new Rect(cursorPosition.x, Screen.height - cursorPosition.y, cursorWidth, cursorHeight), cursorImage);
           if (cursorPosition.y > Screen.height)
           {
               cursorPosition.y = Screen.height;
           }

      //  GUI.DrawTexture(new Rect(Input.mousePosition.x, Screen.height - Input.mousePosition.y, cursorWidth, cursorHeight), cursorImage);

    }

    //打つ
    void NormalAttack()
    {

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(cursorPosition);
        if (Physics.Raycast(ray, out hit))
        {
            IEnemy enemyHealth = hit.collider.GetComponent<IEnemy>();
            if (enemyHealth != null)
            {
                enemyHealth.takeDamage(10);
                return;
            }
            IShotable shootable = hit.collider.GetComponent<IShotable>();
            if (shootable != null)
            {
                shootable.interact(hit.point);
            }
            
        }
    }
}


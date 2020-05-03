using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using td;
namespace M
{
    public class M_Canvas : MonoBehaviour
    {
        M_Tower tower;
        M_HealthManager healthManager;
        [SerializeField]
        Slider Towerbar = null;
        [SerializeField]
        Slider canvasTowerBar = null;
        [SerializeField]
        Slider playerHealthBar= null;
        Text playerHealthtext = null;
        Text towerhealthtext = null;
        bool init = false;
        M_InputHandler hand;
        // Start is called before the first frame update
        void Start()
        {
            tower = GetComponentInChildren<M_Tower>();
            hand = GetComponentInChildren<M_InputHandler>();
            healthManager = hand.gameObject.GetComponent<M_HealthManager>();
            Towerbar.maxValue = tower.getMaxHealth();
            canvasTowerBar.maxValue = tower.getMaxHealth();
            playerHealthBar.maxValue = healthManager.getMaxHealth();
            towerhealthtext = canvasTowerBar.GetComponentInChildren<Text>();
            playerHealthtext = playerHealthBar.GetComponentInChildren<Text>();
            init = true;
        }


        // Update is called once per frame
        void Update()
        {
            if (!init) return;
            Towerbar.value = tower.getHealth();
            canvasTowerBar.value = tower.getHealth();
            playerHealthBar.value = healthManager.getHealth();
            playerHealthtext.text = healthManager.getHealth() + "/" + healthManager.getMaxHealth();
            towerhealthtext.text = tower.getHealth() + "/" + tower.getHealth();
        }
    }

}

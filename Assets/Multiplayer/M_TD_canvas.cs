using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class M_TD_canvas : MonoBehaviour
{
    [SerializeField]
    Image crossAir = null;
    [SerializeField]
    Image flourImage = null;
    Timer flourTimer;
    float alpha;

    private void Start()
    {
        flourTimer = new Timer(5,5);
    }
    public void activeCrossAir(bool v)
    {
        crossAir.enabled = v;
    }

    private void Update()
    {
        flourTimer.Tick(Time.deltaTime);
        if (flourTimer.go)
        {
            
            alpha -= (0.2f * Time.deltaTime);
            if (flourImage != null)
            flourImage.color = new Color(flourImage.color.r, flourImage.color.g, flourImage.color.b, alpha);

        }
    }
    public void actriveFlour()
    {
        if (flourImage == null) return;
        alpha = 1;
        flourImage.color = new Color(flourImage.color.r, flourImage.color.g, flourImage.color.b, alpha);
        flourTimer.go = true;
    }
    
}

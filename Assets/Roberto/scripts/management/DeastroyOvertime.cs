using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeastroyOvertime : MonoBehaviour
{
    //時間が経ったら削除

    public float timer = 2;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, timer);
    }
    
}

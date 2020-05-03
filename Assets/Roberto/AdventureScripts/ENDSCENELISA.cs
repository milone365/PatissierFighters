using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ENDSCENELISA : MonoBehaviour
{
    [SerializeField]
    Transform point = null;
    [SerializeField]
    float movespeed = 10;

    //移動
    void Update()
    {
        if (point == null) return;
        transform.position = Vector3.MoveTowards(transform.position, point.position, movespeed * Time.deltaTime);
    }
}

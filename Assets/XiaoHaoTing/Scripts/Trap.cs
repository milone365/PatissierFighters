using UnityEngine;
using System.Collections;

public class Trap : MonoBehaviour
{

    public GameObject enemy1;
    public GameObject enemy2;

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Debug.Log("enemy1");
            if(enemy1!=null)
            enemy1.SetActive(true);
            if(enemy2 != null)
                enemy2.SetActive(true);
           
        }
    }
}
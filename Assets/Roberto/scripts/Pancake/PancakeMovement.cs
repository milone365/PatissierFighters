using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PancakeMovement : MonoBehaviour
{
    Rigidbody rb=new Rigidbody();
    [SerializeField]
    float maxForce=25f, minForce=18;
    Vector3 direction;
    
    //パンケーキに力を入れる関数
   public void go()
    {
       
        transform.rotation = new Quaternion(transform.rotation.x, Random.Range(0, 180), transform.rotation.z,0);
        int randomChange = Random.Range(1, 5);
        float randForce = Random.Range (minForce, maxForce);
        //ランダムで決める
        switch (randomChange)
        {
            case 1:direction = transform.forward; break;
            case 2:direction = -transform.forward; break;
            case 3: direction = transform.right; break;
            case 4: direction = -transform.right; break;
           

        }
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.useGravity = true;
        transform.localScale = new Vector3(5, 5, 5);
        rb.AddForce(direction* randForce, ForceMode.Impulse);
        //リストに追加します
        Invoke("addTolist", 3);
        

    }

    void addTolist()
    {
        GetComponent<PancakePickUP>().Cantake();
        PancakeSpawner.instance.addTolist(this.transform);
        
    }

   
}

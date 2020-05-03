using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour,IShotable
{
    [SerializeField]
    Material greenMaterial=null;
    
    [SerializeField]
    int hp = 3;
    bool death = false;
    [SerializeField]
    float force = 2000;

    public void interact(Vector3 hitPos)
    {
        if (death) return;
        hp--;
        if (hp <= 0)
        {
            death = true;
            GetComponent<Rigidbody>().AddForce(transform.right * force);
            EffectDirector.instance.EffectAndPopup(hitPos, "HELPERMELEE", 0);
            GetComponent<BoxCollider>().isTrigger = true;
        }
    }
    //水の色を変わって、プレイヤーのスコアをエラス
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Water")
        {
            other.GetComponent<MeshRenderer>().sharedMaterial = greenMaterial;
            EffectDirector.instance.generatePopUp(-1000);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADV_RotatorOnShot : MonoBehaviour, IShotable
{
   protected int hp = 3;
   protected bool death = false;
   protected bool dangerous = false;
    [SerializeField]
   protected Quaternion newRotation= new Quaternion(-90, 0, 0, 0);

    //interface function
    public void interact(Vector3 hitPos)
    {
        Interaction();
    }
    //自分のダメージ
    public virtual void Interaction()
    {
        if (death) return;
        hp--;
        if (hp <= 0)
        {
            death = true;
            //回る
            transform.rotation = newRotation;

        }
    }
    //プレイヤーにダメージをつける
    private void OnTriggerEnter(Collider other)
    {
        if (death||!dangerous) return;
        if (other.tag == StaticStrings.player)
        {
            IHealth health = other.GetComponent<IHealth>();
            if (health != null)
            {
                health.takeDamage(1);
            }
        }
        if (other.GetComponent<Adv_Drive>())
        {
            other.GetComponent<Adv_Drive>().blinking();
        }
    }
}

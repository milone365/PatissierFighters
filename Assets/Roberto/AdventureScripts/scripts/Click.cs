using UnityEngine;
using System.Collections;

public class Click : MonoBehaviour
{
   
    public GameObject Bomb;
    public float BombLeft = 0;
    public float radius = 5f;
    public float force = 70f;
    public LayerMask Enemy;
    Animator anim;
    bool death = false;
    
    private void Start()
    {
        anim = GetComponent<Animator>();
       
    }

    
    void NormalAttack()
    {
        //raycastで打つ
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //interfaceを活性する
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
    //爆弾の効果
        void BombAttack()
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.layer == Enemy)
                {
                    foreach (Collider nearbyObject in colliders)
                    {
                        Rigidbody rb_o = nearbyObject.GetComponent<Rigidbody>();
                        if (rb_o != null)
                        {
                            rb_o.AddExplosionForce(force, transform.position, radius);
                            Debug.Log("Bomb!");
                        }
                    }
                }
            }

        }
    //打つインプット
        void Update()
        {
        if (death) return;
        if (Input.GetButtonDown("Fire1")|| Input.GetButtonDown(StaticStrings.X_key))
        {
            NormalAttack();
        }
     
    }

}


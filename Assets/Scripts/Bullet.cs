/// <summary>
/// 弾の処理
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 1;
	/*/// <summary>
	/// 当たり判定処理
	/// </summary>
	/// <param name="collision">当たったオブジェクト情報</param>
	private void OnCollisionEnter(Collision collision) 
	{
        if (collision.gameObject.tag == "Player")
        {
            // 当たったオブジェクトのデータを取得
            var hit = collision.gameObject;
            // 体力情報を取得
            var health = hit.GetComponent<Health>();
            // 体力情報が存在する場合処理する
            if (health != null)
            {
                // 体力を１０削る
                health.TakeDammage(damage);
            }
            // 弾を消滅させる
            Destroy(gameObject);
        }
		
	}*/
}

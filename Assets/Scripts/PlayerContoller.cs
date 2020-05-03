using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
namespace Duarte
{

/// <summary>
/// プレイヤーのコントロール
/// </summary>
public class PlayerContoller : NetworkBehaviour
{
	// 弾プレハブ
    public GameObject bulletPrefab;
	// 弾出現位置
    public Transform bulletSpawn;

    // Update is called once per frame
    void Update()
    {
		// ローカルプレイヤー以外は操作しない
        if (!isLocalPlayer)
        {
            return;
        }
		// キー入力の取得
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;
		// プレイヤーの回転
        transform.Rotate(0, x, 0);
		// プレイヤーの移動
        transform.Translate(0, 0, z);
		// スペースキーを押した場合
        if (Input.GetKeyDown(KeyCode.Space))
        {
			// 弾を打つ処理を入れる
            CmdFire();
        }
    }

	/// <summary>
	/// 初期状態のプレイヤーのカラーのみ変更する
	/// </summary>
    public override void OnStartLocalPlayer()
    {
		// プレイヤープレハブについたメッシュレンダラーのマテリアルカラーを変更
        GetComponent<MeshRenderer>()
            .material.color = Color.blue;
    }

	// 撃った情報をサーバーから送信するための設定
    [Command]
    private void CmdFire()
    {
		// 弾の実態を生成する
        var bullet = Instantiate<GameObject>(
            bulletPrefab,
            bulletSpawn.position,
            bulletSpawn.rotation
        );
		// 速度を与える（前に向かう速度として）
        bullet.GetComponent<Rigidbody>().velocity =
            bullet.transform.forward * 6;
		// 生成した弾を出現させる（ネットワーク同期）
        NetworkServer.Spawn(bullet);
		// 2秒後に自分を消す
        Destroy(bullet, 2.0f);
    }
}
}

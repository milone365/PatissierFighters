/// <summary>
/// NPC キャラクターの処理
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EnemySpawner : NetworkBehaviour 
{
    // 敵のキャラプレハブ
    public GameObject enemyPrefab;
    // 敵の番号
    public int numberOfEnemies;

    /// <summary>
    /// サーバー開始時に呼び出される初期化処理
    /// </summary>
    public override void OnStartServer()
    {
        // 敵の総数分繰り返しの処理を行う
        for (int i=0; i < numberOfEnemies; i++)
        {
            // キャラの出現座標を左右・前後±８の範囲でランダムに設定
            var spawnPosition = new Vector3(
                Random.Range(-8.0f, 8.0f),
                0.0f,
                Random.Range(-8.0f, 8.0f));
            // キャラの向きを0°～180°の間のランダムに設定
            var spawnRotation = Quaternion.Euler( 
                0.0f, 
                Random.Range(0,180), 
                0.0f);
            // 敵キャラを出現させる
            var enemy = (GameObject)Instantiate(enemyPrefab, spawnPosition, spawnRotation);
            // サーバーに敵の生成情報を送る
            NetworkServer.Spawn(enemy);
        }
    }
}

﻿/// <summary>
/// 体力処理
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Health : NetworkBehaviour
{
    // 最大体力値を設定
    public const int maxHealth = 100;
    // 死亡フラグ
	public bool destroyOnDeath;
    //spawnpos
    NetworkStartPosition[] respawnpositions;
    // 体力をネットワーク越しに連動させるための設定
    [SyncVar(hook = "OnChangeHealth")]
    public int currentHealth = maxHealth;
    
    // 体力バーのサイズ（これを拡縮して体力の増減を表現する）
    public Slider healthBar;

    private void Start()
    {
        respawnpositions = FindObjectsOfType<NetworkStartPosition>();
    }
    /// <summary>
    /// ダメージ処理
    /// </summary>
    /// <param name="amount">与えられたダメージ</param>
    public void TakeDammage(int amount)
    {
        // サーバーでないならば処理しない
        if (!isServer)
        {
            return;
        }
        // 体力を減らす
        currentHealth -= amount;
        // 体力がなくなっていたら
        if (currentHealth <= 0)
        {
            // 死亡フラグが立っていれば消滅させる（敵キャラに適用）
            if (destroyOnDeath)
            {
                Destroy(gameObject);
            }
            // プレイヤーの場合は体力がMAXになって再度画面に出現する
            else
            {
                currentHealth = maxHealth;
                RpcRespawn();
            }
        }
       /* // 体力バーを増減させる
        healthBar.sizeDelta = new Vector2(currentHealth, healthBar.sizeDelta.y);*/
    }

    /// <summary>
    /// ネットワーク越しに連動させる関数
    /// </summary>
    /// <param name="health">体力</param>
    void OnChangeHealth(int health)
    {
        healthBar.value = health;
        // 体力バーを増減させる
       /* healthBar.sizeDelta = new Vector2(health, healthBar.sizeDelta.y);*/
    }

    /// <summary>
    /// クライアント側で再生成の処理を行う
    /// </summary>
    [ClientRpc]
    private void RpcRespawn()
    {
        // クライアント大賞
        if (isLocalPlayer)
        {
            int rnd = Random.Range(0, respawnpositions.Length);
            Vector3 point = respawnpositions[rnd].transform.position;
            // 初期位置に戻す
            transform.position = point;
        }
    }
}

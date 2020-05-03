using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class BossHealth : MonoBehaviour
{
    //イベント
    public event Action onDeath;

    //体の部分のリストを作る
    List<BossPart> allParts=new List<BossPart>();
    //部分追加
    public void AddPartToList(BossPart p)
    {
　　　　allParts.Add(p);
    }
    //リストから削除、イベントを呼ぶ
    public void removePartToList(BossPart p)
    {
        allParts.Remove(p);
        if (allParts.Count < 1)
        {
            if (onDeath != null)
            {
                onDeath();
            }
        }
    }
}

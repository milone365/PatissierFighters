/// <summary>
/// 画像を常にカメラに向けるようにする
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		// オブジェクトの向いている向きをカメラに設定する
		transform.LookAt(Camera.main.transform);
	}
}

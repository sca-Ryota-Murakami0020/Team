using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationObjectC : MonoBehaviour
{
    //時間計測に使う変数
    private float countTime = 0.0f;
    //オブジェクトのポジション
    private Vector3 pos;

    void Start()
    {
        pos = this.transform.position;
    }

    void Update()
    {
        countTime += 0.01f;
        //オブジェクトを縦に往復移動させる
        this.transform.position = new Vector3(pos.x, pos.y, pos.z + Mathf.Sin(countTime) * 0.07f);
        this.transform.Rotate(0, 0, 0.05f);
    }
}

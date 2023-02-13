using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackPasueButton : MonoBehaviour
{
    //計測する周期
    private float countTime = 0.0f;
    //オブジェクトのポジション
    private Vector3 pos;
    //PasueDisplayC
    private PasueDisplayC pDC;

    void Start()
    {
        pos = this.transform.position;
        pDC = FindObjectOfType<PasueDisplayC>();
    }

    void Update()
    {
        //矢印を往復するように動かす
        countTime += 0.01f;
        this.transform.position = new Vector3(pos.x + Mathf.Sin(countTime) * 2, pos.y, pos.z);
    }

    //１ページ目から２ページ目に移動する
    public void BackFirstPasueUI()
    {
        pDC.OpenFirstMenuPage();
    }
}

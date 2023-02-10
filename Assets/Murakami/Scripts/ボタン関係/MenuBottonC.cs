using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBottonC : MonoBehaviour
{
    private PasueDisplayC pDC;

    void Start()
    {
        pDC = FindObjectOfType<PasueDisplayC>();
    }

    //ポーズ画面から操作説明画面に切り替える
    public void OnManual()
    {
        //操作画面の開示
        pDC.DisplayManual();
    }
}

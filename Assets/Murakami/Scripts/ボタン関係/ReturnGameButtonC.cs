using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnGameButtonC : MonoBehaviour
{
    private PasueDisplayC pDC;
    // Start is called before the first frame update
    void Start()
    {
        pDC = FindObjectOfType<PasueDisplayC>();
    }

    //ポーズ画面からゲーム画面に戻る
    public void ReturnGame()
    {
        //ポーズ画面を閉じる
        pDC.CloseMenu();
    }
}

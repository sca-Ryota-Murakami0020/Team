using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleButtonC : MonoBehaviour
{
    //TotalGameManager
    private totalGameManager tGM;

    private void Awake()
    {
        tGM = FindObjectOfType<totalGameManager>();
    }

    //タイトルに戻る
    public void GotoTitle()
    {
        //totalGameManagerがないタイトル画面を呼び出す
        SceneManager.LoadScene("AnyLoadTitle");
        //現在のプレイ時間を初期化する
        tGM.TotalTime = 0.0f;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeTextC : MonoBehaviour
{
    //totalGameOManager
    private totalGameManager totalGM;
    //表示する画像の配列
    [SerializeField] private Sprite[] numberImage;
    //1秒単位において表示する画像
    [SerializeField] private Image oneSecImage;
    //10秒単位において表示する画像
    [SerializeField] private Image tenSecImage;
    //1分単位において表示する画像
    [SerializeField] private Image oneMinImage;
    //10分単位において表示する画像
    [SerializeField] private Image tenMinImage;
    //移動するタイム表示
    [SerializeField] private RectTransform timer;
    //コルーチンで用いる変数
    private int counter = 0;
    //TherdScore
    private TherdScore th;

    // Start is called before the first frame update
    void Start()
    {
        totalGM = FindObjectOfType<totalGameManager>();
        th = FindObjectOfType<TherdScore>();

        //初期化
        //1秒単位の初期化（0～9）
        oneSecImage.sprite = numberImage[0];
        //10秒単位の初期化（0～5）
        tenSecImage.sprite = numberImage[0];
        //1分単位の初期化（0～9）
        oneMinImage.sprite = numberImage[0];
        //10分単位の初期化（0～5）
        tenMinImage.sprite = numberImage[0];
    }

    void Update()
    {
        //ハイスコアの表記
        //ゲーム時間の1秒単位の表示を行う
        oneSecImage.sprite = numberImage[Mathf.FloorToInt(totalGM.TotalTime % 10)];
        //ゲーム時間の10秒単位の表示を行う
        tenSecImage.sprite = numberImage[Mathf.FloorToInt((totalGM.TotalTime % 60) / 10)];
        //ゲーム時間の1分単位の表示を行う
        oneMinImage.sprite = numberImage[Mathf.FloorToInt(totalGM.TotalTime / 60)];
        //ゲーム時間の10分単位の表示を行う
        tenMinImage.sprite = numberImage[Mathf.FloorToInt(totalGM.TotalTime / 600)];
        //コルーチン開始
        StartCoroutine("StartText");
    }

    //ランキングのスライド移動
    private IEnumerator StartText()
    {
        yield return new WaitForSeconds(1.5f);
        //1ゲームのタイムを00:00表示で画面外から-x方向に500だけ動かす
        if (counter <= 480)
        {
            timer.position -= new Vector3(2.5f, 0, 0);
            counter++;
        }
        //1.5秒間待機する
        yield return new WaitForSeconds(1.5f);
        //待機終了後にTherdScoreのコルーチンを開始させる
        th.UpdateTherdScore();
        yield break;
    }
}

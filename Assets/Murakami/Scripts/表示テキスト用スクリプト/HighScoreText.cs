using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreText : MonoBehaviour
{
    //totalGameManager
    private totalGameManager totalGM;
    //1秒単位の画像貼り換えを行うImage(0～9)
    [SerializeField] private Image oneSecImage;
    //10秒単位の画像貼り換えを行うImage(0～5)
    [SerializeField] private Image tenSecImage;
    //1分単位の画像貼り換えを行うImage(0～9)
    [SerializeField] private Image oneMinImage;
    //10分単位の画像貼り換えを行うImage(0～5)
    [SerializeField] private Image tenMinImage;
    //時間表示で用いる数字の画僧配列
    [SerializeField] private Sprite[] numberImage;
    //テキストを動かすために必要な宣言
    [SerializeField] private RectTransform timer;
    //コルーチンでテキストを動かした分を計算する変数
    private int counter = 0;　

    void Start()
    {
        totalGM = FindObjectOfType<totalGameManager>();

        //1秒単位のImage画像の初期化
        oneSecImage.sprite = numberImage[0];
        //10秒単位のImage画像の初期化
        tenSecImage.sprite = numberImage[0];
        //1分単位のImage画像の初期化
        oneMinImage.sprite = numberImage[0];
        //10分単位のImage画像の初期化
        tenMinImage.sprite = numberImage[0];
    }

    public void UpdateHighScore()
    {
        //ハイスコアの表記
        if (totalGM.BestTime[0] <= 0.0f)
        {
            //１位のデフォルト表示ー＞00:00表示
            oneSecImage.sprite = numberImage[0];
            tenSecImage.sprite = numberImage[0];
            oneMinImage.sprite = numberImage[0];
            tenMinImage.sprite = numberImage[0];
        }

        else
        {
            //１位のランキング表示
            //1秒単位のImage画像の更新
            oneSecImage.sprite = numberImage[Mathf.FloorToInt(totalGM.BestTime[0] % 10)];
            //10秒単位のImage画像の更新
            tenSecImage.sprite = numberImage[Mathf.FloorToInt((totalGM.BestTime[0] % 60) / 10)];
            //1分単位のImage画像の更新
            oneMinImage.sprite = numberImage[Mathf.FloorToInt(totalGM.BestTime[0] / 60)];
            //10分単位のImage画像の更新
            tenMinImage.sprite = numberImage[Mathf.FloorToInt(totalGM.BestTime[0] / 600)];
        }
        //コルーチン作動
        StartCoroutine("StartHighScore");
    }

    //ランキングのスライド移動
    private IEnumerator StartHighScore()
    {
        //画面外から移動してくる
        if (counter <= 120 && counter >= 0)
        {
            timer.position -= new Vector3(5.0f, 0, 0);
            counter++;
        }
        //ある程度移動したら減速する
        if (counter <= 245 && counter >= 120)
        {
            timer.position -= new Vector3(3.0f, 0, 0);
            counter++;
        }
        //終着してから1秒間待機する
        yield return new WaitForSeconds(1);
        yield break;
    }
}

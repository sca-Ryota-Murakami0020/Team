using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SecondScore : MonoBehaviour
{
    //totalGameManager
    private totalGameManager totalGM;
    //HighScoreText
    private HighScoreText hst;

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
    private int counter;

    void Start()
    {
        totalGM = FindObjectOfType<totalGameManager>();
        hst = FindObjectOfType<HighScoreText>();

        //1秒単位のImage画像の初期化
        oneSecImage.sprite = numberImage[0];
        //10秒単位のImage画像の初期化
        tenSecImage.sprite = numberImage[0];
        //1分単位のImage画像の初期化
        oneMinImage.sprite = numberImage[0];
        //10分単位のImage画像の初期化
        tenMinImage.sprite = numberImage[0];

        counter = 0;

        //ハイスコア更新
        UpLoadSecondTime();
    }

    /*
    void Update()
    {

}   */

    public void UpLoadSecondTime()
    {
        //2位スコアの表記
        if (totalGM.BestTime[1] <= 0.0f)
        {
            //２位のデフォルト表示
            oneSecImage.sprite = numberImage[0];
            tenSecImage.sprite = numberImage[0];
            oneMinImage.sprite = numberImage[0];
            tenMinImage.sprite = numberImage[0];
        }
        else
        {
            //２位のランキング表示
            //1秒単位のImage画像の更新
            oneSecImage.sprite = numberImage[Mathf.FloorToInt(totalGM.BestTime[1] % 10)];
            //10秒単位のImage画像の更新
            tenSecImage.sprite = numberImage[Mathf.FloorToInt((totalGM.BestTime[1] % 60) / 10)];
            //1分単位のImage画像の更新
            oneMinImage.sprite = numberImage[Mathf.FloorToInt(totalGM.BestTime[1] / 60)];
            //10分単位のImage画像の更新
            tenMinImage.sprite = numberImage[Mathf.FloorToInt(totalGM.BestTime[1] / 600)];
        }
    }

    private IEnumerator StartSecondScore()
    {
        //画面外から移動してくる
        if (counter <= 300 && counter >= 0)
        {
            timer.position -= new Vector3(2.0f, 0, 0);
            counter++;
        }
        //ある程度進んだら減速する
        if (counter <= 500 && counter >= 300)
        {
            timer.position -= new Vector3(1.0f, 0, 0);
            counter++;
        }
        //一秒間待機する
        yield return new WaitForSeconds(1.5f);
        //待機が終わった後に1位のハイスコアのテキストを動かすコルーチンを作動させる
        hst.StartCoroutine("StartHighScore");
        yield break;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TherdScore : MonoBehaviour
{
    //totalGameManager
    private totalGameManager totalGM;
    //SecondScore
    private SecondScore ss;

    //タイム表示で用いる数字の画像
    [SerializeField] private Sprite[] numberImage;
    //1秒単位の画像貼り換えを行うImage(0～9)
    [SerializeField] private Image oneSecImage;
    //10秒単位の画像貼り換えを行うImage(0～5)
    [SerializeField] private Image tenSecImage;
    //1秒単位の画像貼り換えを行うImage(0～9)
    [SerializeField] private Image oneMinImage;
    //10秒単位の画像貼り換えを行うImage(0～5)
    [SerializeField] private Image tenMinImage;
    //テキストを動かすために必要な宣言
    [SerializeField] private RectTransform timer;
    //テキストの動いた距離を計測する変数
    private int counter;


    void Start()
    {
        totalGM = FindObjectOfType<totalGameManager>();
        ss = FindObjectOfType<SecondScore>();

        //1秒単位のImage画像の初期化
        oneSecImage.sprite = numberImage[0];
        //10秒単位のImage画像の初期化
        tenSecImage.sprite = numberImage[0];
        //1分単位のImage画像の初期化
        oneMinImage.sprite = numberImage[0];
        //10分単位のImage画像の初期化
        tenMinImage.sprite = numberImage[0];

        counter = 0;
    }

    public void UpdateTherdScore()
    {
        //2位スコアの表記
        if (totalGM.BestTime[2] <= 0.0f)
        {
            //３位のデフォルト表示ー＞00:00表紙
            oneSecImage.sprite = numberImage[0];
            tenSecImage.sprite = numberImage[0];
            oneMinImage.sprite = numberImage[0];
            tenMinImage.sprite = numberImage[0];
        }
        else
        {
            //３位のランキング表示"
            //1秒単位のImage画像の更新
            oneSecImage.sprite = numberImage[Mathf.FloorToInt(totalGM.BestTime[2] % 10)];
            //10秒単位のImage画像の更新
            tenSecImage.sprite = numberImage[Mathf.FloorToInt((totalGM.BestTime[2] % 60) / 10)];
            //1分単位のImage画像の更新
            oneMinImage.sprite = numberImage[Mathf.FloorToInt(totalGM.BestTime[2] / 60)];
            //10分単位のImage画像の更新
            tenMinImage.sprite = numberImage[Mathf.FloorToInt(totalGM.BestTime[2] / 600)];
        }
        StartCoroutine("StartTherdScore");
    }

    private IEnumerator StartTherdScore()
    {
        //画面外から移動してくる
        if (counter <= 255 && counter >= 0)
        {
            timer.position -= new Vector3(2.5f, 0, 0);
            counter++;
        }
        //ある程度移動したら減速する
        if (counter <= 333 && counter >= 255)
        {
            timer.position -= new Vector3(1.5f, 0, 0);
            counter++;
        }
        yield return new WaitForSeconds(1.5f);
        //1秒後2位のハイスコアテキストを移動させるコルーチンを作動させる
        ss.UpLoadSecondTime();
        yield break;
    }
}

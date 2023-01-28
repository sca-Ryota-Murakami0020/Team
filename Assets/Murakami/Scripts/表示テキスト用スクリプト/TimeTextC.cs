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
    private int counter;
    //TherdScore
    private TherdScore th;

    // Start is called before the first frame update
    void Start()
    {
        totalGM = FindObjectOfType<totalGameManager>();
        //00:00表示にする　＝　初期化
        oneSecImage.sprite = numberImage[0];
        tenSecImage.sprite = numberImage[0];
        oneMinImage.sprite = numberImage[0];
        tenMinImage.sprite = numberImage[0];

        counter = 0;
        th = FindObjectOfType<TherdScore>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetStartCowrotine()
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

    private IEnumerator StartText()
    {
        //1ゲームのタイムを00:00表示で画面外から-x方向に500だけ動かす
        if (counter <= 500)
        {
            timer.position -= new Vector3(2.0f, 0, 0);
            counter++;
        }
        yield return new WaitForSeconds(1);
        //設置完了1秒後にTherdScoreのコルーチンを開始させる
        th.StartCoroutine("StartTherdScore");
        yield break;
    }
}

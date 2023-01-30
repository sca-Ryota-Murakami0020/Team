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

    // Start is called before the first frame update
    void Start()
    {
        totalGM = FindObjectOfType<totalGameManager>();
        oneSecImage.sprite = numberImage[0];
        tenSecImage.sprite = numberImage[0];
        oneMinImage.sprite = numberImage[0];
        tenMinImage.sprite = numberImage[0];
        counter = 0;
        hst = FindObjectOfType<HighScoreText>();
        UpLoadSecondTime();
    }

    // Update is called once per frame
    void Update()
    {

    }

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
            oneSecImage.sprite = numberImage[Mathf.FloorToInt(totalGM.BestTime[1] % 10)];
            tenSecImage.sprite = numberImage[Mathf.FloorToInt((totalGM.BestTime[1] % 60) / 10)];
            oneMinImage.sprite = numberImage[Mathf.FloorToInt(totalGM.BestTime[1] / 60)];
            tenMinImage.sprite = numberImage[Mathf.FloorToInt(totalGM.BestTime[1] / 600)];
            //if(olt.LoadCout > 3)
            //Debug.Log("text2 " + olt.TimeText[1]);
        }
    }

    private IEnumerator StartSecondScore()
    {
        if (counter <= 300 && counter >= 0)
        {
            timer.position -= new Vector3(2.0f, 0, 0);
            counter++;
        }
        if (counter <= 500 && counter >= 300)
        {
            timer.position -= new Vector3(1.0f, 0, 0);
            counter++;
        }
        yield return new WaitForSeconds(1);
        hst.StartCoroutine("StartHighScore");
        yield break;
    }
}

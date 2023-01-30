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

    //private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        totalGM = FindObjectOfType<totalGameManager>();
        for (int i = 0; i <= 2; i++)
        {
            oneSecImage.sprite = numberImage[0];
            tenSecImage.sprite = numberImage[0];
            oneMinImage.sprite = numberImage[0];
            tenMinImage.sprite = numberImage[0];
        }
        counter = 0;
        ss = FindObjectOfType<SecondScore>();
        //anim = this.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(olt.OldSecondTime);
        //2位スコアの表記
        if (totalGM.BestTime[2] <= 0.0f)
        {
            //３位のデフォルト表示"
            oneSecImage.sprite = numberImage[0];
            tenSecImage.sprite = numberImage[0];
            oneMinImage.sprite = numberImage[0];
            tenMinImage.sprite = numberImage[0];
        }
        else
        {
            //３位のランキング表示"
            oneSecImage.sprite = numberImage[Mathf.FloorToInt(totalGM.BestTime[2] % 10)];
            tenSecImage.sprite = numberImage[Mathf.FloorToInt((totalGM.BestTime[2] % 60) / 10)];
            oneMinImage.sprite = numberImage[Mathf.FloorToInt(totalGM.BestTime[2] / 60)];
            tenMinImage.sprite = numberImage[Mathf.FloorToInt(totalGM.BestTime[2] / 600)];
            //if(olt.LoadCout > 3)
            //Debug.Log("text3 " + olt.TimeText[2]);
        }
    }

    private IEnumerator StartTherdScore()
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
        ss.StartCoroutine("StartSecondScore");
        yield break;
    }
}

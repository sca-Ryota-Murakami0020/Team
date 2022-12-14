using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeC : MonoBehaviour
{
    //総プレイ時間：
    private float totalTime;

    private PlayerC pl;
    private OverLoadTimer olt;

    public Text timeText;

    public float TotalTime
    {
        get { return this.totalTime; }
        set { this.totalTime = value; }
    }

    // Start is called before the first frame update
    void Start()
    {

        timeText = GetComponentInChildren<Text>();
        pl = GameObject.Find("Player").GetComponent<PlayerC>();
        olt = GameObject.Find("GameManager").GetComponent<OverLoadTimer>();
    }

    // Update is called once per frame
    void Update()
    {
        //
        if (pl.AliveFlag)
        {
            //ハイスコアを比較しやすくするために秒計算の変数を作る
            totalTime += Time.deltaTime;

            timeText.text = (totalTime/3600).ToString("00") + ":" + (totalTime/120).ToString("00") + ":" + (totalTime%60).ToString("00");
        }
        else this.GameOver();
    }

    public void GameOver()
    {
        olt.LoadGameOver();
    }

}       


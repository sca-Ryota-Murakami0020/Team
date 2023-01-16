using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeC : MonoBehaviour
{
    //総プレイ時間：
    //private float totalTime;

    //private PlayerC pl;
    //private OverLoadTimer olt;
    private totalGameManager totalGM;

    public Text timeText;


    // Start is called before the first frame update
    void Start()
    {
        timeText = GetComponentInChildren<Text>();
        //olt = GameObject.Find("GameManager").GetComponent<OverLoadTimer>();
        totalGM = FindObjectOfType<totalGameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //各ステージのタイム表示
        timeText.text = (totalGM.TotalTime/3600).ToString("00") + ":" + (totalGM.TotalTime/120).ToString("00") + ":" + (totalGM.TotalTime % 60).ToString("00");
        //Debug.Log("表示中");
    }

}       


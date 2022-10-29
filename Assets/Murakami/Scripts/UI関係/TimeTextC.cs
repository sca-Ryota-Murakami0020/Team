using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeTextC : MonoBehaviour
{
    private OverLoadTimer olt;
    public Text TimeText;

    // Start is called before the first frame update
    void Start()
    {
        olt = FindObjectOfType<OverLoadTimer>();
        TimeText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(olt.OldSecondTime);
        //ハイスコアの表記
        if (olt.SecondTime >= 1.0f && olt.SecondTime <= 9.9f)
            TimeText.text = olt.HourTime.ToString("00") + ":" + olt.MinuteTime.ToString("00") + ":" + ((int)olt.SecondTime).ToString("00");
        if (olt.SecondTime >= 10.0f) //Debug.Log(HTime + ":" + MTime.ToString("00") + ":" + STime.ToString("f0"));
            TimeText.text = olt.HourTime.ToString("00") + ":" + olt.MinuteTime.ToString("00") + ":" + ((int)olt.SecondTime).ToString("00");
    }
}

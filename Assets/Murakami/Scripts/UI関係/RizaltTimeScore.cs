using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RizaltTimeScore : MonoBehaviour
{
    private float bestTime;
    private float totalTime;
    private float secondTime;
    private int minuteTime;
    private int hourTime;
    private float oldSecondTime;
    private int oldMinuteTime;
    private int oldHourTime;

    private OverLoadTimer olt;

    public Text timeText;

    // Start is called before the first frame update
    void Start()
    {
        olt = FindObjectOfType<OverLoadTimer>();
    }

    // Update is called once per frame
    void Update()
    {
        timeRiza();
        olt.TotalTime = 0.0f;
    }

    void timeRiza()
    {
        this.secondTime = olt.SecondTime;
        this.minuteTime = olt.MinuteTime;
        this.hourTime = olt.HourTime;

        //¡‰ñ‚Ì‹L˜^‚Ì•\‹L
        if (secondTime >= 1.0f && secondTime <= 9.9f)
            timeText.text = hourTime.ToString("00") + ":" + minuteTime.ToString("00") + ":" + ((int)secondTime).ToString("00");
        if (secondTime >= 10.0f) //Debug.Log(HTime + ":" + MTime.ToString("00") + ":" + STime.ToString("f0"));
            timeText.text = hourTime.ToString("00") + ":" + minuteTime.ToString("00") + ":" + ((int)secondTime).ToString("00");
    }
}

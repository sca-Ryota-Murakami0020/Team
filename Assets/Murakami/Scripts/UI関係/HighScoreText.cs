using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreText : MonoBehaviour
{
    private OverLoadTimer olt;
    public Text bestTimeText;

    // Start is called before the first frame update
    void Start()
    {
        olt = FindObjectOfType<OverLoadTimer>();
        bestTimeText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(olt.OldSecondTime);
        //ハイスコアの表記
        if (olt.OldSecondTime >= 1.0f && olt.OldSecondTime <= 9.9f)
            bestTimeText.text = olt.OldHourTime.ToString("00") + ":" + olt.OldMinuteTime.ToString("00") + ":" + ((int)olt.OldSecondTime).ToString("00");
        if (olt.OldSecondTime >= 10.0f) //Debug.Log(HTime + ":" + MTime.ToString("00") + ":" + STime.ToString("f0"));
            bestTimeText.text = olt.OldHourTime.ToString("00") + ":" + olt.OldMinuteTime.ToString("00") + ":" + ((int)olt.OldSecondTime).ToString("00");
    }
}

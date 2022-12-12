using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreText : MonoBehaviour
{
    private OverLoadTimer olt;
    [SerializeField] Text[] bestTimeText;

    // Start is called before the first frame update
    void Start()
    {
        olt = FindObjectOfType<OverLoadTimer>();
        for (int i = 0; i <= 2; i++)
        {
            bestTimeText[i].text = "";
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(olt.OldSecondTime);
        //ハイスコアの表記
        for (int i = 0; i <= 2; i++)
        {
                bestTimeText[i].text = i + 1 + "位:" + "0:00:00";
            if (olt.OldSecondTime[i] >= 1.0f && olt.OldSecondTime[i] <= 9.9f)
                bestTimeText[i].text = i + 1 + "位:" + olt.OldHourTime[i].ToString("00") + ":" + olt.OldMinuteTime[i].ToString("00") + ":" + ((int)olt.OldSecondTime[i]).ToString("00");
            if (olt.OldSecondTime[i] >= 10.0f) //Debug.Log(HTime + ":" + MTime.ToString("00") + ":" + STime.ToString("f0"));
                bestTimeText[i].text = i + 1 + "位:" + olt.OldHourTime[i].ToString("00") + ":" + olt.OldMinuteTime[i].ToString("00") + ":" + ((int)olt.OldSecondTime[i]).ToString("00");
        }
    }

}

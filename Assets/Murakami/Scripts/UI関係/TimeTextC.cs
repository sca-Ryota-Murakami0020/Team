using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeTextC : MonoBehaviour
{
    private totalGameManager totalGM;
    public Text TimeText;


    // Start is called before the first frame update
    void Start()
    {
        totalGM = FindObjectOfType<totalGameManager>();
        TimeText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(olt.OldSecondTime);
        //�n�C�X�R�A�̕\�L
        //TimeText.text = (totalGM.TotalTime / 3600).ToString("00") + ":" + (totalGM.TotalTime / 120).ToString("00") + ":" + ((int)totalGM.TotalTime % 60).ToString("00");
        Debug.Log("���U���g��������");
        TimeText.text = totalGM.DispTime;
    }
}

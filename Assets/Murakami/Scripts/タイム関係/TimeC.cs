using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeC : MonoBehaviour
{
    //���v���C���ԁF
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
        //�e�X�e�[�W�̃^�C���\��
        timeText.text = (totalGM.TotalTime/3600).ToString("00") + ":" + (totalGM.TotalTime/120).ToString("00") + ":" + (totalGM.TotalTime % 60).ToString("00");
        //Debug.Log("�\����");
    }

}       


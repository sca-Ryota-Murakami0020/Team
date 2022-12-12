using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeC : MonoBehaviour
{
    //���v���C���ԁF
    private float totalTime;
    //�b�P��
    private float secondTime;
    //���P��
    private int minuteTime;
    //���ԒP��
    private int hourTime;

    private PlayerC pl;
    private OverLoadTimer olt;

    public Text timeText;

    public float TotalTime
    {
        get { return this.totalTime; }
        set { this.totalTime = value; }
    }

    public float Stime
    {
        get { return this.secondTime; }
        set { this.secondTime = value; }
    }

    public int Mtime
    {
        get { return this.minuteTime; }
        set { this.minuteTime = value; }
    }

    public int Htime
    {
        get { return this.hourTime; }
        set { this.hourTime = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        //���Ԍv���̏�����
        totalTime = 0.0f;
        secondTime = 0.0f;
        minuteTime = 0;
        hourTime = 0;
        timeText = GetComponentInChildren<Text>();
        pl = GameObject.Find("Player").GetComponent<PlayerC>();
        olt = GameObject.Find("GameManager").GetComponent<OverLoadTimer>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(totalTime);
        //
        if (pl.AliveFlag)
        {
            //�n�C�X�R�A���r���₷�����邽�߂ɕb�v�Z�̕ϐ������
            totalTime += Time.deltaTime;
            //���Ԃ�00:00:00�\�L���邽�߂̕ϐ��̉��Z
            //�b�v�Z
            secondTime += Time.deltaTime;
            if (secondTime >= 60.0f)
            {
                //���v�Z
                minuteTime += 1;
                secondTime = 0.0f;
                if (minuteTime >= 60)
                {
                    //���Ԍv�Z
                    hourTime += 1;
                    minuteTime = 0;
                }
            }
            //00:00:00�\�L����
            if (secondTime >= 1.0f && secondTime <= 9.9f)
                timeText.text = hourTime.ToString("00") + ":" + minuteTime.ToString("00") + ":" + ((int)secondTime).ToString("00");
            if (secondTime >= 10.0f) //Debug.Log(HTime + ":" + MTime.ToString("00") + ":" + STime.ToString("f0"));
                timeText.text = hourTime.ToString("00") + ":" + minuteTime.ToString("00") + ":" + ((int)secondTime).ToString("00");
        }
        else this.GameOver();
    }

    public void GameOver()
    {
        olt.LoadGameOver();
        Debug.Log("olt.LoadGameOver�̌Ăяo��");
    }

}

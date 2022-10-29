using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OverLoadTimer : MonoBehaviour
{
    //public static OverLoadTimer 
    private float bestTime;
    private float totalTime;
    private float secondTime;
    private int minuteTime;
    private int hourTime;
    //
    private float oldSecondTime;
    private int oldMinuteTime;
    private int oldHourTime;
    //�n�C�X�R�A�X�V�𑣂��p�̃t���O
    private bool counterFlag;
    private bool startFlag;
    
    private TimeC timeC;

    public float BestTime
    {
        get { return this.bestTime;}
        set { this.bestTime = value;}
    }

    public float TotalTime
    {
        get { return this.totalTime;}
        set { this.totalTime = value;}
    }

    public float SecondTime
    {
        get { return this.secondTime;}
        set { this.secondTime = value;}
    }

    public int MinuteTime
    {
        get { return this.minuteTime;}
        set { this.minuteTime = value;}
    }

    public int HourTime
    {
        get { return this.hourTime;}
        set { this.hourTime = value;}
    }

    public float OldSecondTime
    {
        get { return this.oldSecondTime; }
        set { this.secondTime = value; }
    }

    public int OldMinuteTime
    {
        get { return this.oldMinuteTime; }
        set { this.oldMinuteTime = value; }
    }

    public int OldHourTime
    {
        get { return this.oldHourTime; }
        set { this.oldHourTime = value; }
    }

    public bool CounterFlag
    {
        get { return this.counterFlag;}
        set { this.counterFlag = value;}
    }

    private void Awake()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Counter");
        DontDestroyOnLoad(this.gameObject);
        if(objects.Length > 1)
        Destroy(objects[1]);

    }
    // Start is called before the first frame update
    void Start()
    {
        timeC = FindObjectOfType<TimeC>();
        startFlag = false;
        SceneManager.sceneLoaded += StageLoaded;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(startFlag);
        if(startFlag == true)
        {
            totalTime = timeC.TotalTime;
            secondTime = timeC.Stime;
            minuteTime = timeC.Mtime;
            hourTime = timeC.Htime;
            float timer = 0.0f;
            timer += Time.deltaTime;
            if(timer >= 1.0f)
            { 
                if (secondTime >= 1.0f && secondTime <= 9.9f)
                Debug.Log(hourTime.ToString("00") + ":" + minuteTime.ToString("00") + ":" + ((int)secondTime).ToString("00"));
                if (secondTime >= 10.0f) //Debug.Log(HTime + ":" + MTime.ToString("00") + ":" + STime.ToString("f0"));
                Debug.Log(hourTime.ToString("00") + ":" + minuteTime.ToString("00") + ":" + ((int)secondTime).ToString("00"));
                timer = 0.0f;
            }
                //Debug.Log(totalTime);

            }
    }
    //public void CountTime()
    //{

    //}

    public void LoadGameOver()
    {
        //���v���C���̋L�^���L�^
        if (bestTime <= 0.0f)
        {
            bestTime = totalTime;
            oldSecondTime = secondTime;
            oldMinuteTime = minuteTime;
            OldHourTime = hourTime;
            Debug.Log("�o�^");
        }
        //2��ڈȍ~�n�C�X�R�A���o������L�^���X�V����
        if (bestTime > totalTime && bestTime >= 0.01f)
        {
            bestTime = totalTime;
            oldSecondTime = secondTime;
            oldMinuteTime = minuteTime;
            OldHourTime = hourTime;
            Debug.Log("�X�V");
        }
        totalTime = 0.0f;
        SceneManager.LoadScene("����pGameOver");
        if (secondTime >= 1.0f && secondTime <= 9.9f)
            Debug.Log(hourTime.ToString("00") + ":" + minuteTime.ToString("00") + ":" + ((int)secondTime).ToString("00"));
        if (secondTime >= 10.0f) //Debug.Log(HTime + ":" + MTime.ToString("00") + ":" + STime.ToString("f0"));
            Debug.Log(hourTime.ToString("00") + ":" + minuteTime.ToString("00") + ":" + ((int)secondTime).ToString("00"));
        if (oldSecondTime >= 1.0f && oldSecondTime <= 9.9f)
            Debug.Log(oldMinuteTime.ToString("00") + ":" + oldMinuteTime.ToString("00") + ":" + ((int)oldSecondTime).ToString("00"));
        if (oldSecondTime >= 10.0f) //Debug.Log(HTime + ":" + MTime.ToString("00") + ":" + STime.ToString("f0"));
             Debug.Log(oldHourTime.ToString("00") + ":" + oldMinuteTime.ToString("00") + ":" + ((int)oldSecondTime).ToString("00"));
    }

    void StageLoaded(Scene nextScene, LoadSceneMode mode)
    {
        if (nextScene.name == "����p")
        {
            //SceneManager.sceneLoaded -= StageLoaded;
            startFlag = true;
            //Stage2���ǂݍ��܂ꂽ�Ƃ��ɂ���������
            Debug.Log("�ǂݍ��܂�܂���");
            timeC = FindObjectOfType<TimeC>();
        }
        if(nextScene.name == "����pTitle")
        {
            startFlag = false;
            Debug.Log("���~");
        }
    }
}

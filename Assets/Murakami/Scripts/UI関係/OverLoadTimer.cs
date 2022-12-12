using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OverLoadTimer : MonoBehaviour
{
    //ハイスコア用変数
    private float[] bestTime;
    private float totalTime;
    private float secondTime;
    private int minuteTime;
    private int hourTime;
    //記録用変数
    private float[] oldSecondTime;
    private int[] oldMinuteTime;
    private int[] oldHourTime;
    private string[] timer;
    //ハイスコア更新を促す用のフラグ
    private bool counterFlag;
    private bool startFlag;
    //スコアの個数を数えるための変数
    private int loadCout;

    private TimeC timeC;

    public float[] BestTime
    {
        get { return this.bestTime; }
        set { this.bestTime = value; }
    }

    public float TotalTime
    {
        get { return this.totalTime; }
        set { this.totalTime = value; }
    }

    public float SecondTime
    {
        get { return this.secondTime; }
        set { this.secondTime = value; }
    }

    public int MinuteTime
    {
        get { return this.minuteTime; }
        set { this.minuteTime = value; }
    }

    public int HourTime
    {
        get { return this.hourTime; }
        set { this.hourTime = value; }
    }

    public float[] OldSecondTime
    {
        get { return this.oldSecondTime; }
        set { this.oldSecondTime = value; }
    }

    public int[] OldMinuteTime
    {
        get { return this.oldMinuteTime; }
        set { this.oldMinuteTime = value; }
    }

    public int[] OldHourTime
    {
        get { return this.oldHourTime; }
        set { this.oldHourTime = value; }
    }

    public bool CounterFlag
    {
        get { return this.counterFlag; }
        set { this.counterFlag = value; }
    }

    public string[] TimeText
    {
        get { return this.timer;}
        set { this.timer = value;}
    }

    private void Awake()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Counter");
        DontDestroyOnLoad(this.gameObject);
        if (objects.Length > 1)
            Destroy(objects[1]);

    }
    // Start is called before the first frame update
    void Start()
    {
        //timeC = FindObjectOfType<TimeC>();
        startFlag = false;
        SceneManager.sceneLoaded += StageLoaded;
        loadCout = 0;
        timer = new string[3];
        bestTime = new float[3];

        for(int i =0;i<3;i++)
        {
            timer[i] = i.ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(startFlag);
        if (startFlag == true)
        {
            totalTime = timeC.TotalTime;
            secondTime = timeC.Stime;
            minuteTime = timeC.Mtime;
            hourTime = timeC.Htime;
        }
    }

    public void LoadGameOver()
    {
        Debug.Log("LoadGameOverの呼び出し");
        //初プレイ時の記録を記録
        if(loadCout <= 3)
        {
            for (int count = 0; count <= loadCout; count++)
            {
                if (bestTime[count] <= 0.0f && bestTime[count] > 0.1f)
                {
                    bestTime[count] = totalTime;
                    oldSecondTime[count] = secondTime;
                    oldMinuteTime[count] = minuteTime;
                    OldHourTime[count] = hourTime;
                    if (oldSecondTime[count] >= 1.0f && oldSecondTime[count] <= 9.9f)
                        timer[count] = count + 1 + "位:" + oldHourTime[count].ToString("00") + ":" + oldMinuteTime[count].ToString("00") + ":" + ((int)oldSecondTime[count]).ToString("00");
                    if (oldSecondTime[count] >= 10.0f)
                        timer[count] = count + 1 + "位:" + oldHourTime[count].ToString("00") + ":" + oldMinuteTime[count].ToString("00") + ":" + ((int)oldSecondTime[count]).ToString("00");
                    Debug.Log("登録");
                }
            }
        }
        

        loadCout += 1;

        //4回目以降ハイスコアを出したら記録を更新する
        if (loadCout >= 4)
        {
            for (int count = 0; count <= 2; count++)
            {
                if (bestTime[count] > totalTime && bestTime[count] >= 0.01f)
                {
                    bestTime[count] = totalTime;
                    oldSecondTime[count] = secondTime;
                    oldMinuteTime[count] = minuteTime;
                    oldHourTime[count] = hourTime;
                    if (oldSecondTime[count] >= 1.0f && oldSecondTime[count] <= 9.9f)
                        timer[count] = count + 1 + "位:" + oldHourTime[count].ToString("00") + ":" + oldMinuteTime[count].ToString("00") + ":" + ((int)oldSecondTime[count]).ToString("00");
                    if (oldSecondTime[count] >= 10.0f) //Debug.Log(HTime + ":" + MTime.ToString("00") + ":" + STime.ToString("f0"));
                        timer[count] = count + 1 + "位:" + oldHourTime[count].ToString("00") + ":" + oldMinuteTime[count].ToString("00") + ":" + ((int)oldSecondTime[count]).ToString("00");
                    Debug.Log("更新");
                    break;
                }
            }
        }

        totalTime = 0.0f;
    }

    void StageLoaded(Scene nextScene, LoadSceneMode mode)
    {
        if (nextScene.name == "村上用")
        {
            //SceneManager.sceneLoaded -= StageLoaded;
            startFlag = true;
            //Stage2が読み込まれたときにしたい処理
            Debug.Log("読み込まれました");
            timeC = FindObjectOfType<TimeC>();
        }
        if (nextScene.name == "村上用Title")
        {
            startFlag = false;
            Debug.Log("中止");
        }
        if (nextScene.name == "村上用GameOver")
        {
            //r();
            Debug.Log("中止");
        }
    }
}

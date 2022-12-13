using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OverLoadTimer : MonoBehaviour
{
    //ハイスコア用変数
    private float[] bestTime;
    //１回のゲーム時間
    private float totalTime;
    //秒単位カウンター
    private float secondTime;
    private int minuteTime;
    private int hourTime;
    private string time;
    //記録用変数
    private float[] oldSecondTime;
    private int[] oldMinuteTime;
    private int[] oldHourTime;
    private string[] timer;
    //ハイスコア更新を促す用のフラグ
    private bool counterFlag;
    private bool startFlag;
    //スコアの個数を数えるための変数
    private int loadCount;

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
        get { return this.timer; }
        set { this.timer = value; }
    }

    public string Time
    {
        get { return this.time; }
        set { this.time = value; }
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
        loadCount = 0;
        timer = new string[4];
        bestTime = new float[4];
        oldSecondTime = new float[4];
        oldMinuteTime = new int[4];
        oldHourTime = new int[4];

        for (int i = 0; i <= 3; i++)
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
            for (int i = 0; i <= 3; i++) Debug.Log(timer[i] + "\n");
        }
    }

    public void LoadGameOver()
    {
        Debug.Log(loadCount);
        //初プレイ時の記録を記録
        if (loadCount <= 3)
        {
            if (bestTime[loadCount] <= 0.0f && bestTime[loadCount] < 0.1f)
            {
                bestTime[loadCount] = totalTime;
                oldSecondTime[loadCount] = secondTime;
                oldMinuteTime[loadCount] = minuteTime;
                OldHourTime[loadCount] = hourTime;
                if (bestTime.Length >= 2)
                {
                    for (int i = 0; i <= 3; i++)
                    {
                        for (int j = 1; j <= 4; j++)
                        {
                            if (bestTime[loadCount - 1] <= bestTime[loadCount])
                            {
                                float btt = bestTime[loadCount - 1];
                                bestTime[loadCount - 1] = totalTime;
                                bestTime[loadCount] = btt;

                                float stt = oldSecondTime[loadCount - 1];
                                oldSecondTime[loadCount - 1] = secondTime;
                                oldSecondTime[loadCount] = stt;

                                int mtt = oldMinuteTime[loadCount - 1];
                                oldMinuteTime[loadCount - 1] = minuteTime;
                                oldMinuteTime[loadCount] = minuteTime;

                                int htt = oldHourTime[loadCount - 1];
                                oldHourTime[loadCount - 1] = hourTime;
                                OldHourTime[loadCount] = htt;

                                Debug.Log("登録");
                                break;
                            }
                        }
                    }
                }
            }
        }

        if (oldSecondTime[loadCount] >= 1.0f && oldSecondTime[loadCount] <= 9.9f)
            timer[loadCount] = loadCount + 1 + "位:" + oldHourTime[loadCount].ToString("00") + ":" + oldMinuteTime[loadCount].ToString("00") + ":" + ((int)oldSecondTime[loadCount]).ToString("00");
        if (oldSecondTime[loadCount] >= 10.0f)
            timer[loadCount] = loadCount + 1 + "位:" + oldHourTime[loadCount].ToString("00") + ":" + oldMinuteTime[loadCount].ToString("00") + ":" + ((int)oldSecondTime[loadCount]).ToString("00");



        loadCount += 1;

        //4回目以降ハイスコアを出したら記録を更新する
        if (loadCount >= 4)
        {
            for (int count = 0; count <= 2; count++)
            {
                for (int i = 1; i <= 3; i++)
                {
                    if (bestTime[count] > totalTime && bestTime[count] >= 0.01f)
                    {
                        float bestTimeTem = bestTime[count];
                        bestTime[count] = totalTime;
                        bestTime[count + 1] = bestTimeTem;

                        float stt = oldSecondTime[count];
                        oldSecondTime[count] = secondTime;
                        oldSecondTime[count + 1] = stt;

                        int mtt = oldMinuteTime[count];
                        oldMinuteTime[count] = minuteTime;
                        oldMinuteTime[count + 1] = mtt;

                        int htt = oldHourTime[count];
                        oldHourTime[count] = hourTime;
                        oldHourTime[count + 1] = htt;

                        if (oldSecondTime[count] >= 1.0f && oldSecondTime[count] <= 9.9f)
                            timer[count] = count + 1 + "位:" + oldHourTime[count].ToString("00") + ":" + oldMinuteTime[count].ToString("00") + ":" + ((int)oldSecondTime[count]).ToString("00");
                        if (oldSecondTime[count] >= 10.0f) //Debug.Log(HTime + ":" + MTime.ToString("00") + ":" + STime.ToString("f0"));
                            timer[count] = count + 1 + "位:" + oldHourTime[count].ToString("00") + ":" + oldMinuteTime[count].ToString("00") + ":" + ((int)oldSecondTime[count]).ToString("00");
                        Debug.Log("更新");
                        break;
                    }
                }

            }
        }

        //ゲーム中に表示するタイマー表示に与える変数
        if (secondTime >= 1.0f && secondTime <= 9.9f)
            time = hourTime.ToString("00") + ":" + minuteTime.ToString("00") + ":" + ((int)secondTime).ToString("00");
        if (secondTime >= 10.0f)
            time = hourTime.ToString("00") + ":" + minuteTime.ToString("00") + ":" + ((int)secondTime).ToString("00");

        totalTime = 0.0f;
    }

    void StageLoaded(Scene nextScene, LoadSceneMode mode)
    {
        if (nextScene.name == "村上用")
        {
            startFlag = true;
            //Stage2が読み込まれたときにしたい処理
            //Debug.Log("読み込まれました");
            timeC = FindObjectOfType<TimeC>();
        }
        if (nextScene.name == "村上用Title")
        {
            startFlag = false;
        }

    }

}

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
    /*
    private float secondTime;
    private int minuteTime;
    private int hourTime;
    */
    private string time;
    private string[] timer;
    //記録用変数
    /*
    private float[] oldSecondTime;
    private int[] oldMinuteTime;
    private int[] oldHourTime;
    
    */
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
    /*
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
    }*/

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
        /*
        oldSecondTime = new float[4];
        oldMinuteTime = new int[4];
        oldHourTime = new int[4];
        */

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
            /*
            secondTime = timeC.Stime;
            minuteTime = timeC.Mtime;
            hourTime = timeC.Htime;
            */
        }
    }

    public void LoadGameOver()
    {
        //初プレイ時の記録を記録
        if (loadCount <= 3)
        {
            if (bestTime[loadCount] <= 0.0f && bestTime[loadCount] < 0.1f)
            {
                bestTime[loadCount] = totalTime;
                /*
                oldSecondTime[loadCount] = secondTime;
                oldMinuteTime[loadCount] = minuteTime;
                OldHourTime[loadCount] = hourTime;
                */
                if (loadCount >= 1)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 1; j < 4; j++)
                        {
                            if (bestTime[i] >= bestTime[j])
                            {
                                float btt = bestTime[j];
                                bestTime[j] = totalTime;
                                bestTime[i] = btt;
                                /*
                                float stt = oldSecondTime[loadCount - 1];
                                oldSecondTime[loadCount - 1] = secondTime;
                                oldSecondTime[loadCount] = stt;

                                int mtt = oldMinuteTime[loadCount - 1];
                                oldMinuteTime[loadCount - 1] = minuteTime;
                                oldMinuteTime[loadCount] = minuteTime;

                                int htt = oldHourTime[loadCount - 1];
                                oldHourTime[loadCount - 1] = hourTime;
                                OldHourTime[loadCount] = htt;
                                */
                                break;
                            }
                        }
                        timer[i] = i + 1 + "位:" + (bestTime[i] / 3600.0f).ToString("00") + ":" + (bestTime[i] / 60.0f).ToString("00") + ":" + ((int)bestTime[i] % 60.0f).ToString("00");
                    }
                }
            }
        }

        //4回目以降ハイスコアを出したら記録を更新する
        if (loadCount >= 4)
        {
            for (int i = 0; i <= 3; i++)
            {
                for (int j = 1; j <= 4; j++)
                {
                    if (bestTime[i] > totalTime && bestTime[i] >= 0.01f)
                    {
                        float bestTimeTem = bestTime[i];
                        bestTime[i] = totalTime;
                        bestTime[i + 1] = bestTimeTem;

                        /*
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
                        */
                        timer[loadCount] = loadCount + 1 + "位:" + (bestTime[loadCount] / 3600.0f).ToString("00") + ":" + (bestTime[loadCount] / 60.0f).ToString("00") + ":" + ((int)bestTime[loadCount] % 60.0f).ToString("00");
                        break;
                    }
                }
                timer[i] = i + 1 + "位:" + (bestTime[i] / 3600.0f).ToString("00") + ":" + (bestTime[i] / 60.0f).ToString("00") + ":" + ((int)bestTime[i] % 60.0f).ToString("00");
            }
        }

        //ゲーム中に表示するタイマー表示に与える変数
        time = (totalTime / 3600.0f).ToString("00") + ":" + (totalTime / 60.0f).ToString("00") + ":" + ((int)totalTime % 60.0f).ToString("00");
        loadCount += 1;
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

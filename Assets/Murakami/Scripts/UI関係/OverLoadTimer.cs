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
    //1プレイのリザルト用のタイム
    private string time;
    //ハイスコアのデータ格納
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

    public int LoadCout
    {
        get { return this.loadCount;}
        set { this.loadCount = value;}
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
        }
    }

    public void LoadGameOver()
    {
        //Debug.Log("LoadCount" + loadCount);

        if (loadCount == 0)
        {
            bestTime[loadCount] = totalTime;
            //timer[loadCount] = "1位:" + (bestTime[loadCount] / 3600).ToString("00") + ":" + (bestTime[loadCount] / 120).ToString("00") + ":" + ((int)bestTime[loadCount] % 60).ToString("00");
        }

        //２回目プレイ～３回目時の記録を記録
        if (loadCount >= 1 && loadCount <= 2)
        {
            bestTime[loadCount] = totalTime;
            if (loadCount >= 1)
            {
                for (int i = loadCount; i > 0; i--)
                {
                    for (int j = loadCount - 1; j >= 0; j--)
                    {
                        if (bestTime[j] > bestTime[i])
                        {
                            float btt = bestTime[j];
                            bestTime[j] = bestTime[i];
                            bestTime[i] = btt;
                        }
                    }
                    //timer[i] = i + 1 + "位:" + (bestTime[i] / 3600).ToString("00") + ":" + (bestTime[i] / 120).ToString("00") + ":" + ((int)bestTime[i] % 60).ToString("00");                      
                }
            }
        }

        //4回目以降ハイスコアを出したら記録を更新する
        if (loadCount >= 3)
        {
            bestTime[3] = totalTime;
                for (int i = 3; i >= 1; i--)
                {
                    for (int j = i -1; j >= 0; j--)
                    {
                        if (bestTime[j] > bestTime[i])
                        {                       
                            float bestTimeTem = bestTime[j];
                            bestTime[j] = bestTime[i];
                            bestTime[i] = bestTimeTem;
                            Debug.Log(bestTime[i]);
                            Debug.Log(bestTime[j]);
                        }
                    }
                //timer[i] = i + 1 + "位:" + (bestTime[i] / 3600).ToString("00") + ":" + (bestTime[i] / 120).ToString("00") + ":" + ((int)bestTime[i] % 60).ToString("00");
                }
        }

        //ゲーム中に表示するタイマー表示に与える変数
        time = (totalTime / 3600).ToString("00") + ":" + (totalTime / 120).ToString("00") + ":" + ((int)totalTime % 60).ToString("00");
        for(int i = 0; i <= loadCount; i++)
        {
            //if (loadCount >= 3) Debug.Log("olt"+ i + " " + bestTime[i]);
            timer[i] = i + 1 + "位:" + (bestTime[i] / 3600).ToString("00") + ":" + (bestTime[i] / 120).ToString("00") + ":" + ((int)bestTime[i] % 60).ToString("00");
        }
        loadCount += 1;
        totalTime = 0.0f;

    }

    void StageLoaded(Scene nextScene, LoadSceneMode mode)
    {
        if (nextScene.name == "村上用")
        {
            startFlag = true;
            //Stage2が読み込まれたときにしたい処理
            timeC = FindObjectOfType<TimeC>();
            //if(loadCount >= 3) bestTime[3] = 0.0f;
        }
        if (nextScene.name == "村上用Title")
        {
            startFlag = false;
        }

    }

}

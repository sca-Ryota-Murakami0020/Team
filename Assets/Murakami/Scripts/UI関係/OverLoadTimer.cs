using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OverLoadTimer : MonoBehaviour
{
    //�n�C�X�R�A�p�ϐ�
    private float[] bestTime;
    //�P��̃Q�[������
    private float totalTime;
    //�b�P�ʃJ�E���^�[
    private float secondTime;
    private int minuteTime;
    private int hourTime;
    private string time;
    //�L�^�p�ϐ�
    private float[] oldSecondTime;
    private int[] oldMinuteTime;
    private int[] oldHourTime;
    private string[] timer;
    //�n�C�X�R�A�X�V�𑣂��p�̃t���O
    private bool counterFlag;
    private bool startFlag;
    //�X�R�A�̌��𐔂��邽�߂̕ϐ�
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
        loadCout = 0;
        timer = new string[4];
        bestTime = new float[4];
        oldSecondTime = new float[4];
        oldMinuteTime = new int[4];
        oldHourTime = new int[4];

        for(int i =0;i<=3;i++)
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
            for(int i = 0;i <= 3; i++) Debug.Log(timer[i] + "\n");
        }
    }

    public void LoadGameOver()
    {
        Debug.Log(loadCout);
        //���v���C���̋L�^���L�^
        if(loadCout <= 3)
        {
            for (int count = 0; count <= loadCout; count++)
            {
                if (bestTime[count] <= 0.0f && bestTime[count] < 0.1f)
                {
                    bestTime[count] = totalTime;
                    oldSecondTime[count] = secondTime;
                    oldMinuteTime[count] = minuteTime;
                    OldHourTime[count] = hourTime;
                    if (oldSecondTime[count] >= 1.0f && oldSecondTime[count] <= 9.9f)
                        timer[count] = count + 1 + "��:" + oldHourTime[count].ToString("00") + ":" + oldMinuteTime[count].ToString("00") + ":" + ((int)oldSecondTime[count]).ToString("00");
                    if (oldSecondTime[count] >= 10.0f)
                        timer[count] = count + 1 + "��:" + oldHourTime[count].ToString("00") + ":" + oldMinuteTime[count].ToString("00") + ":" + ((int)oldSecondTime[count]).ToString("00");
                    Debug.Log("�o�^");
                }
            }
        }
        

        loadCout += 1;

        //4��ڈȍ~�n�C�X�R�A���o������L�^���X�V����
        if (loadCout >= 4)
        {
            for (int count = 0; count <= 2; count++)
            {
                for(int i = 1; i <= 3; i++)
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
                            timer[count] = count + 1 + "��:" + oldHourTime[count].ToString("00") + ":" + oldMinuteTime[count].ToString("00") + ":" + ((int)oldSecondTime[count]).ToString("00");
                        if(oldSecondTime[count] >= 10.0f) //Debug.Log(HTime + ":" + MTime.ToString("00") + ":" + STime.ToString("f0"));
                            timer[count] = count + 1 + "��:" + oldHourTime[count].ToString("00") + ":" + oldMinuteTime[count].ToString("00") + ":" + ((int)oldSecondTime[count]).ToString("00");
                        Debug.Log("�X�V");
                        break;
                    }
                }

            }
        }

        //�Q�[�����ɕ\������^�C�}�[�\���ɗ^����ϐ�
        if (secondTime >= 1.0f && secondTime <= 9.9f)
            time =  hourTime.ToString("00") + ":" + minuteTime.ToString("00") + ":" + ((int)secondTime).ToString("00");
        if (secondTime >= 10.0f)
            time =  hourTime.ToString("00") + ":" + minuteTime.ToString("00") + ":" + ((int)secondTime).ToString("00");

        totalTime = 0.0f;
    }

    void StageLoaded(Scene nextScene, LoadSceneMode mode)
    {
        if (nextScene.name == "����p")
        {
            startFlag = true;
            //Stage2���ǂݍ��܂ꂽ�Ƃ��ɂ���������
            //Debug.Log("�ǂݍ��܂�܂���");
            timeC = FindObjectOfType<TimeC>();
        }
        if (nextScene.name == "����pTitle")
        {
            startFlag = false;
        }

    }
}

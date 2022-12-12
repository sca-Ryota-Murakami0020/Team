using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OverLoadTimer : MonoBehaviour
{
    //�n�C�X�R�A�p�ϐ�
    private float[] bestTime;
    private float totalTime;
    private float secondTime;
    private int minuteTime;
    private int hourTime;
    //�L�^�p�ϐ�
    private float[] oldSecondTime;
    private int[] oldMinuteTime;
    private int[] oldHourTime;
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
        Debug.Log("LoadGameOver�̌Ăяo��");
        //���v���C���̋L�^���L�^
        for (int count = 0; count <= loadCout; count++)
        {
            if (bestTime[count] <= 0.0f && bestTime[count] > 0.1f)
            {
                bestTime[count] = totalTime;
                oldSecondTime[count] = secondTime;
                oldMinuteTime[count] = minuteTime;
                OldHourTime[count] = hourTime;
                Debug.Log("�o�^");
            }
        }

        loadCout += 1;

        //4��ڈȍ~�n�C�X�R�A���o������L�^���X�V����
        if (loadCout >= 4)
        {
            for (int i = 0; i <= 2; i++)
            {
                if (bestTime[i] > totalTime && bestTime[i] >= 0.01f)
                {
                    bestTime[i] = totalTime;
                    oldSecondTime[i] = secondTime;
                    oldMinuteTime[i] = minuteTime;
                    oldHourTime[i] = hourTime;
                    Debug.Log("�X�V");
                    break;
                }
            }
        }

        totalTime = 0.0f;
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
        if (nextScene.name == "����pTitle")
        {
            startFlag = false;
            Debug.Log("���~");
        }
    }
}

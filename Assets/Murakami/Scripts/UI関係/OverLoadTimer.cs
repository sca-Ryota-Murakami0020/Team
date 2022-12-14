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
    //1�v���C�̃��U���g�p�̃^�C��
    private string time;
    //�n�C�X�R�A�̃f�[�^�i�[
    private string[] timer;
    //�n�C�X�R�A�X�V�𑣂��p�̃t���O
    private bool counterFlag;
    private bool startFlag;
    //�X�R�A�̌��𐔂��邽�߂̕ϐ�
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
        Debug.Log("LoadCount" + loadCount);
        //���v���C���̋L�^���L�^
        if (loadCount < 3)
        {
            Debug.Log("LoadCount3����");
            if (bestTime[loadCount] <= 0.0f && bestTime[loadCount] < 0.1f)
            {
                bestTime[loadCount] = totalTime;
                if (loadCount >= 1)
                {
                    for (int i = loadCount; i >= 0; i--)
                    {
                        for (int j = loadCount - 1; j >= 0; j--)
                        {
                            if (bestTime[j] > bestTime[i])
                            {
                                float btt = bestTime[j];
                                bestTime[j] = bestTime[i];
                                bestTime[i] = btt;
                                Debug.Log("touroku");
                            }
                        }
                        timer[i] = i + 1 + "��:" + (bestTime[i] / 3600).ToString("00") + ":" + (bestTime[i] / 120).ToString("00") + ":" + ((int)bestTime[i] % 60).ToString("00");                      
                    }
                }
                //����v���C�̌��ʂ������L���O�ɔ��f������
                if (loadCount == 0)
                {
                    bestTime[loadCount] = totalTime;
                    timer[loadCount] = "1��:" + (bestTime[loadCount] / 3600).ToString("00") + ":" + (bestTime[loadCount] / 120).ToString("00") + ":" + ((int)bestTime[loadCount] % 60).ToString("00");
                }
                    
            }
        }

        //4��ڈȍ~�n�C�X�R�A���o������L�^���X�V����
        if (loadCount >= 3)
        {
            Debug.Log("LoadCount3�ȏ�");
            for (int i = 3; i >= 0; i--)
            {
                for (int j = 2; j >= 0; j--)
                {
                    if (bestTime[j] > bestTime[i])
                    {
                        float bestTimeTem = bestTime[j];
                        bestTime[j] = bestTime[i];
                        bestTime[i] = bestTimeTem;
                        Debug.Log("kousinn");
                    }
                }
                timer[i] = i + 1 + "��:" + (bestTime[i] / 3600).ToString("00") + ":" + (bestTime[i] / 120).ToString("00") + ":" + ((int)bestTime[i] % 60).ToString("00");
            }
        }

        //�Q�[�����ɕ\������^�C�}�[�\���ɗ^����ϐ�
        time = (totalTime / 3600).ToString("00") + ":" + (totalTime / 120).ToString("00") + ":" + ((int)totalTime % 60).ToString("00");
        loadCount += 1;
        totalTime = 0.0f;
    }

    void StageLoaded(Scene nextScene, LoadSceneMode mode)
    {
        if (nextScene.name == "����p")
        {
            startFlag = true;
            //Stage2���ǂݍ��܂ꂽ�Ƃ��ɂ���������
            timeC = FindObjectOfType<TimeC>();
        }
        if (nextScene.name == "����pTitle")
        {
            startFlag = false;
        }

    }

}

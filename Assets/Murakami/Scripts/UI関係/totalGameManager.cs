using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class totalGameManager : MonoBehaviour
{
    #region//���ʉ��֌W�X�e�[�^�X
    ////���ʉ��̔z��ݒ��AudioSource�Ăяo��
    [SerializeField] private AudioClip[] bgms;
    private AudioSource audios;

    //��ʐ؂�ς�鎞
    private bool displaySwitchingFlag = false;
    //�^�C���A�b�vor���S��
    private bool playerDeathFlag = false;
    //���U���g���̃����L���O���莞
    private bool rankingFlag = false;
    //�{�^���������Ƃ��̌��ʉ�(���莞)
    private bool decisionFlag = false;
    //�{�^���������Ƃ��̌��ʉ�(��������)
    private bool returnFlag = false;
    //�G�ɓ����������ɂȂ���ʉ��t���O
    private bool playerDamegeFlag = false;
    //�ǃW�����v�����Ƃ��̌��ʉ��t���O
    private bool wallJumpFlag = false;
    //���[�����O�����Ƃ��̌��ʉ��t���O
    private bool completeRollFlag = false;
    //�W�����v�����Ƃ��̌��ʉ��t���O
    private bool jumpFlag = false;
    //�ړ������Ƃ��̌��ʉ��t���O
    private bool moveFlag = false;
    //Hp���O�ɂȂ����Ƃ��̌��ʉ�
    private bool disappearHpFlag = false;
    #endregion

    #region//�^�C���֌W�X�e�[�^�X
    //�n�C�X�R�A�p�ϐ�
    private float[] bestTime;
    //�P��̃Q�[������
    private float totalTime;
    //1�v���C�̃��U���g�p�̃^�C��
    private string timeScore;
    //�n�C�X�R�A�̃f�[�^�i�[
    private string[] timer;
    //�n�C�X�R�A�X�V�𑣂��p�̃t���O
    private bool counterFlag;
    //�v���C�J�n�ƃv���C�I���̔��������t���O
    private bool startFlag;
    //�X�R�A�̌��𐔂��邽�߂̕ϐ�
    private int loadCount;

    [SerializeField] private Text nowPlayingText;

    private PlayerC pl;

    private TextMovement textM;
    #endregion

    #region//�v���p�e�B
    public bool PDFlag//�v���C���[�_���[�W
    {
        get { return this.playerDamegeFlag; }
        set { this.playerDamegeFlag = value; }
    }

    public bool WallJumpFlag//�ǃW�����v
    {
        get { return this.wallJumpFlag; }
        set { this.wallJumpFlag = value; }
    }

    public bool CompleteRollFlag//�O�]
    {
        get { return this.completeRollFlag; }
        set { this.completeRollFlag = value; }
    }

    public bool JumpFlag//�W�����v
    {
        get { return this.jumpFlag; }
        set { this.jumpFlag = value; }
    }

    public bool MoveFlag//�ړ�
    {
        get { return this.moveFlag; }
        set { this.moveFlag = value; }
    }

    public bool DisplaySwitchingFlag//��ʐ؂�ւ�
    {
        get { return this.displaySwitchingFlag; }
        set { this.displaySwitchingFlag = value; }
    }

    public bool PlayerDeathFlag//�v���C���[���S��
    {
        get { return this.playerDeathFlag; }
        set { this.playerDeathFlag = value; }
    }

    public bool RankingFlag//�����L���O���莞
    {
        get { return this.rankingFlag; }
        set { this.rankingFlag = value; }
    }

    public bool DecisionFlag//�{�^�����莞
    {
        get { return this.decisionFlag; }
        set { this.decisionFlag = value; }
    }

    public bool ReturnFlag//��������
    {
        get { return this.returnFlag; }
        set { this.returnFlag = value; }
    }

    public bool DisappeareHp//HP0�ɂȂ�����
    {
        get { return this.disappearHpFlag;}
        set { this.disappearHpFlag = value;}
    }

    public float[] BestTime//
    {
        get { return this.bestTime; }
        set { this.bestTime = value; }
    }

    public float TotalTime//
    {
        get { return this.totalTime; }
        set { this.totalTime = value; }
    }

    public bool CounterFlag//
    {
        get { return this.counterFlag; }
        set { this.counterFlag = value; }
    }

    public string[] TimeText//
    {
        get { return this.timer; }
        set { this.timer = value; }
    }

    public string DispTime//
    {
        get { return this.timeScore; }
        set { this.timeScore = value; }
    }

    public int LoadCout//
    {
        get { return this.loadCount; }
        set { this.loadCount = value; }
    }
    #endregion

    //�V���O���g��
    private void Awake()
    {
        //AudioSource�Ăяo��
        audios = GetComponent<AudioSource>();
        DontDestroyOnLoad(this.gameObject);
    }


    // Start is called before the first frame update
    void Start()
    {
        startFlag = false;
        //������
        SceneManager.sceneLoaded += StageLoaded;
        loadCount = 0;
        //�z��̗v�f���̒�`
        timer = new string[4];
        bestTime = new float[4];

        for (int i = 0; i <= 3; i++)
        {
            timer[i] = i.ToString();
        }
        nowPlayingText = GetComponentInChildren<Text>();
        pl = GameObject.Find("Player").GetComponent<PlayerC>();
    }

    // Update is called once per frame
    void Update()
    {
        #region //���ʉ��i�X���S���j
        //����̃t���O�������������̌��ʉ���炷
        if (playerDamegeFlag == true)
        {
            audios.clip = bgms[0];
            audios.Play();
            playerDamegeFlag = false;
        }

        if (wallJumpFlag == true)
        {
            audios.clip = bgms[1];
            audios.Play();
            wallJumpFlag = false;
        }

        if (completeRollFlag == true)
        {
            audios.clip = bgms[2];
            audios.Play();
            completeRollFlag = false;
        }

        if (jumpFlag == true)
        {
            audios.clip = bgms[3];
            audios.Play();
            jumpFlag = false;
        }

        if (moveFlag == true)
        {
            audios.clip = bgms[4];
            audios.Play();
            moveFlag = false;
        }

        if (displaySwitchingFlag == true)
        {
            audios.clip = bgms[5];
            audios.Play();
            displaySwitchingFlag = false;
        }

        if (playerDeathFlag == true)
        {
            audios.clip = bgms[6];
            audios.Play();
            playerDeathFlag = false;
        }

        if (rankingFlag == true)
        {
            audios.clip = bgms[7];
            audios.Play();
            rankingFlag = false;
        }

        if (decisionFlag == true)
        {
            audios.clip = bgms[9];
            audios.Play();
            decisionFlag = false;
        }

        if (returnFlag == true)
        {
            audios.clip = bgms[10];
            audios.Play();
            returnFlag = false;
        }

        if(disappearHpFlag == true)
        {
            audios.clip = bgms[11];
            audios.Play();
            disappearHpFlag = false;
        }
        #endregion

        #region //���Ԍv���i����S���j
        if (startFlag == true)
        {
            this.totalTime += Time.deltaTime;
            nowPlayingText.text = (totalTime / 3600).ToString("00") + ":" + (totalTime / 120).ToString("00") + ":" + ((int)totalTime % 60).ToString("00");
        }
        if(!pl.AliveFlag) LoadGameClear();
        #endregion

    }

    #region //���U���g���f�̏���
    public void LoadGameClear()
    {
        if (loadCount == 0)
        {
            bestTime[loadCount] = totalTime;
        }
        //�Q��ڃv���C�`�R��ڎ��̋L�^���L�^
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
                }
            }
        }
        //4��ڈȍ~�n�C�X�R�A���o������L�^���X�V����
        if (loadCount >= 3)
        {
            bestTime[3] = totalTime;
            for (int i = 3; i >= 1; i--)
            {
                for (int j = i - 1; j >= 0; j--)
                {
                    if (bestTime[j] > bestTime[i])
                    {
                        float bestTimeTem = bestTime[j];
                        bestTime[j] = bestTime[i];
                        bestTime[i] = bestTimeTem;
                        Debug.Log("i: " + bestTime[i]);
                        Debug.Log("j: " + bestTime[j]);
                    }
                }
            }
        }

        //�Q�[�����ɕ\������^�C�}�[�\���ɗ^����ϐ�
        timeScore = (totalTime / 3600).ToString("00") + ":" + (totalTime / 120).ToString("00") + ":" + ((int)totalTime % 60).ToString("00");

        for (int i = 0; i <= 2; i++)
        {
            timer[i] = i + 1 + "��:" + (bestTime[i] / 3600).ToString("00") + ":" + (bestTime[i] / 120).ToString("00") + ":" + ((int)bestTime[i] % 60).ToString("00");
        }
        loadCount += 1;
        totalTime = 0.0f;
    }
    #endregion

    #region//�Ăяo�����V�[���ɉ����Ă̏���
    void StageLoaded(Scene nextScene, LoadSceneMode mode)
    {
        if (nextScene.name == "����p")
        {
            startFlag = true;
            //Stage2���ǂݍ��܂ꂽ�Ƃ��ɂ���������
            //if(loadCount >= 3) bestTime[3] = 0.0f;
        }

        if (nextScene.name == "����pTitle")
        {
            startFlag = false;
        }

        if (nextScene.name == "GoalScene")
        {
            textM = FindObjectOfType<TextMovement>();
        }
    }
    #endregion
}

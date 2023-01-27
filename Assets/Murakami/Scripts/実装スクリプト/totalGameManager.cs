using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class totalGameManager : MonoBehaviour
{
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
    //private bool counterFlag;
    //�v���C�J�n�ƃv���C�I���̔��������t���O
    private bool timeCounter;
    //�X�R�A�̌��𐔂��邽�߂̕ϐ�
    private int loadCount;

    //[SerializeField] private Text nowPlayingText;

    private TextMovement textM;
    #endregion

    #region//�v���C���[�֌W
    //�����p���v���C���[��Hp
    private int playerHp = 3;
    //�����p���A�C�e���J�E���g�̕ϐ�
    private int playerItemCount = 0;
    #endregion

    #region//�v���p�e�B

    public float[] BestTime//�n�C�X�R�A�̐��l�i�[�z��
    {
        get { return this.bestTime; }
        set { this.bestTime = value; }
    }

    public float TotalTime//1�v���C����
    {
        get { return this.totalTime; }

        set { this.totalTime = value; }
    }

    /*public bool CounterFlag//
    {
        get { return this.counterFlag; }
        set { this.counterFlag = value; }
    }*/

    public string[] TimeText//�n�C�X�R�A�̕�����i�[�z��
    {
        get { return this.timer; }
        set { this.timer = value; }
    }

    public string DispTime//�v���C���ɕ\������^�C���\�L
    {
        get { return this.timeScore; }
        set { this.timeScore = value; }
    }

    public int LoadCout//�N���A������
    {
        get { return this.loadCount; }
        set { this.loadCount = value; }
    }

    public int PlayerHp//�v���C���[�̂g��
    {
        get { return this.playerHp;}
        set { this.playerHp = value;}
    }

    public int PlayerIC//�v���C���[�̌��̏�����
    {
        get { return this.playerItemCount;}
        set { this.playerItemCount = value;}
    }

    public bool TimeCounter//�v���������肷��
    {
        get { return this.timeCounter;}
        set { this.timeCounter = value;}
    }
    #endregion

    //�V���O���g��
    private void Awake()
    {
        //AudioSource�Ăяo��
        //audios = GetComponent<AudioSource>();
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        timeCounter = false;
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
        //nowPlayingText = GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        #region //���Ԍv���i����S���j
        if (timeCounter == true)
        {
            this.totalTime += Time.deltaTime;
            //Debug.Log(totalTime);
            //nowPlayingText.text = (totalTime / 3600).ToString("00") + ":" + (totalTime / 120).ToString("00") + ":" + ((int)totalTime % 60).ToString("00");
        }

        /*if(timeCounter == false) (totalTime / 3600).ToString("00") + ":" + (bestTime[i] / 3600).ToString("00") + ":" +
        {
            this.totalTime = 0.0f;
            //nowPlayingText.text = "00:00:00";
        }*/

        //if(!pl.AliveFlag) LoadGameClear();
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
        timeScore =(totalTime / 60).ToString("00") + ":" + ((int)totalTime % 60).ToString("00");

        for (int i = 0; i <= 2; i++)
        {
            timer[i] = i + 1 + "��:" +  (bestTime[i] / 60).ToString("00") + ":" + ((int)bestTime[i] % 60).ToString("00");
        }

        loadCount += 1;
    }
    #endregion

    #region//�Ăяo�����V�[���ɉ����Ă̏���
    void StageLoaded(Scene nextScene, LoadSceneMode mode)
    {
        //
        if (nextScene.name == "bill" || nextScene.name == "1�K" || nextScene.name == "2�K" || nextScene.name == "3�K")
        {
            timeCounter = true;
            //totalTime = 0.0f;
            //nowPlayingText = GetComponentInChildren<Text>();
        }

        if (nextScene.name == "LoadFirstStage" || nextScene.name == "LoadSecondStage" || nextScene.name == "LoadTherdStage")
        {
            timeCounter = false;
            //nowPlayingText = GetComponentInChildren<Text>();
        }

        if (nextScene.name == "GameOverScene" || nextScene.name == "GoalScene" || nextScene.name == "LoadBill")
        {
            timeCounter = false;
            playerHp = 3;
            playerItemCount = 0;
        }

        if (nextScene.name == "GoalScene")
        {
            textM = FindObjectOfType<TextMovement>();
        }
    }
    #endregion
}
/*
     #region//���ʉ��֌W�X�e�[�^�X
    ////���ʉ��̔z��ݒ��AudioSource�Ăяo��
    [SerializeField] private AudioClip[] bgms;
    private AudioSource audios =null;

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
    //private bool completeRollFlag = false;
    //�W�����v�����Ƃ��̌��ʉ��t���O
    private bool jumpFlag = false;
    //�ړ������Ƃ��̌��ʉ��t���O
    private bool moveFlag = false;
    //Hp���O�ɂȂ����Ƃ��̌��ʉ�
    private bool disappearHpFlag = false;
    //�������Ă鎞�Ɏg���ړ��p���ʉ�
    private bool accelWalkFlag = false;
    //���n�������̌��ʉ�
    private bool randingFlag = false;
    #endregion
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

    /*public bool CompleteRollFlag//�O�]
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
    get { return this.disappearHpFlag; }
    set { this.disappearHpFlag = value; }
}

public bool AccelWalkFlag//�������ړ�
{
    get { return this.accelWalkFlag; }
    set { this.accelWalkFlag = value; }
}

public bool RandingFlag//���n��
{
    get { return this.randingFlag; }
    set { this.randingFlag = value; }
        #region //���ʉ��i�X���S���j
        //����̃t���O�������������̌��ʉ���炷
        /*if (playerDamegeFlag == true)
        {
            audios.clip = bgms[0];
            audios.Play();
            playerDamegeFlag = false;
        }:*/

/*if (wallJumpFlag == true)
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
}*/
/*if(randingFlag == true)
{
    audios.clip = bgms[1];
    audios.Play();
    randingFlag = false;
}

if(accelWalkFlag == true)
{
    audios.clip = bgms[2];
    audios.Play();
    accelWalkFlag = false;
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
}

if(moveFlag == false)
{
    audios.clip = bgms[4];
    audios.Stop();
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
    audios.clip = bgms[8];
    audios.Play();
    decisionFlag = false;
}

if (returnFlag == true)
{
    audios.clip = bgms[9];
    audios.Play();
    returnFlag = false;
}

if(disappearHpFlag == true)
{
    audios.clip = bgms[10];
    audios.Play();
    disappearHpFlag = false;
}
#endregion
*/
 

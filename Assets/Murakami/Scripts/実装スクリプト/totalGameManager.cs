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
    private float totalTime = 0.0f;
    //�X�R�A�̌��𐔂��邽�߂̕ϐ�
    private int loadCount = 0;
    //�v���C�J�n�ƃv���C�I���̔��������t���O
    private bool timeCounter = false;
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
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        //�V�[���ɉ������������ł���悤�ɂ���
        SceneManager.sceneLoaded += StageLoaded;
        //�z��̗v�f���̒�`
        bestTime = new float[4];
    }

    // Update is called once per frame
    void Update()
    {
        #region //���Ԍv���i����S���j
        //�v��������t���OtimeCoounter��true�̊Ԃ͎��Ԍv�����s��
        if (timeCounter == true)
        {
            this.totalTime += Time.deltaTime;
        }
        #endregion
    }

    #region //���U���g���f�̏���
    public void LoadGameClear()
    {
        //����̃v���C�̋L�^���n�C�X�R�A��1�ʂ̔z��Ɋi�[����
        if (loadCount == 0)
        {
            //1�ʂ̃����L���O�̗v�f�Ɋi�[����
            bestTime[loadCount] = totalTime;
        }

        //�Q��ڃv���C�`�R��ڎ��̋L�^���L�^
        if (loadCount >= 1 && loadCount <= 2)
        {
            //�v���C�񐔂ɉ������Ԓn�ɃN���A���Ԃ�������
            //�v���C�񐔂ɉ������Ԓn�ɂ������R�͊��Ƀf�[�^���i�[����Ă���Ԓn�Ɋi�[���悤�Ƃ����
            //�f�[�^���㏑�������̂Ń����L���O������ɓ��삵�Ȃ�����
            bestTime[loadCount] = totalTime;
            //���̃o�u���\�[�g�Ń����L���O���X�V����
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
            //bestTime��3�Ԓn�͕K���ŐV�̃N���A���Ԃ�����
            bestTime[3] = totalTime;
            //�����̃o�u���\�[�g�Ń����L���O�̍X�V���s��
            for (int i = 3; i >= 1; i--)
            {
                for (int j = i - 1; j >= 0; j--)
                {
                    if (bestTime[j] > bestTime[i])
                    {
                        float bestTimeTem = bestTime[j];
                        bestTime[j] = bestTime[i];
                        bestTime[i] = bestTimeTem;
                    }
                }
            }
        }

        //���U���g��ʂɔ�񂾉񐔂𑫂�
        loadCount += 1;
    }
    #endregion

    #region//�Ăяo�����V�[���ɉ����Ă̏���
    void StageLoaded(Scene nextScene, LoadSceneMode mode)
    {
        //���ꂼ��̃v���C�V�[�����Ăяo���ꂽ��
        if (nextScene.name == "bill" || nextScene.name == "1�K" || nextScene.name == "2�K" || nextScene.name == "3�K" || nextScene.name == "LastScene")
        {
            //�^�C���v�����J�nor�ĊJ������
            timeCounter = true;
        }

        //���ꂼ��̃v���C�V�[���̑O�ɌĂяo�����[�h�V�[�����Ăяo���ꂽ��
        if (nextScene.name == "LoadFirstStage" || nextScene.name == "LoadSecondStage" || nextScene.name == "LoadTherdStage" || nextScene.name == "LoadLastStage")
        {
            //�^�C���v������U��~������
            timeCounter = false;
        }

        //�Q�[���I�[�o�[�V�[��or���U���g��ʂ��Ăяo���ꂽ��
        if (nextScene.name == "GameOverScene" || nextScene.name == "GoalScene")
        {
            //�^�C���v������U��~������
            timeCounter = false;
            //�v���C���[�ɗ^����̗͂�������
            playerHp = 3;
            //�A�C�e���̏�������������
            playerItemCount = 0;
        }

        //�������ʂ��烊�g���C���鏈�����󂯂���
        if ( nextScene.name == "LoadBill")
        {
            //�^�C���v������U��~����
            timeCounter = false;
            //�v���C���[�̗̑͂̏�����
            playerHp = 3;
            //�A�C�e���̏������̏�����
            playerItemCount = 0;
            //�P�Q�[���̃v���C���Ԃ̏�����
            totalTime = 0.0f;
        }
    }
    #endregion
}
 

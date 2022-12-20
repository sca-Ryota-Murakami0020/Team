using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GManager : MonoBehaviour
{
    //GM

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

    private bool ReturnFlag//��������
    {
        get { return this.returnFlag; }
        set { this.returnFlag = value; }
    }


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

    }

    // Update is called once per frame
    void Update()
    {
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

        if(displaySwitchingFlag == true)
        {
            audios.clip = bgms[5];
            audios.Play();
            displaySwitchingFlag = false;
        }

        if(playerDeathFlag == true)
        {
            audios.clip = bgms[6];
            audios.Play();
            playerDeathFlag = false;
        }

        if(rankingFlag == true)
        {
            audios.clip = bgms[7];
            audios.Play();
            rankingFlag = false;
        }

        if(decisionFlag == true)
        {
            audios.clip = bgms[9];
            audios.Play();
            decisionFlag = false;
        }

        if(returnFlag == true)
        {
            audios.clip = bgms[10];
            audios.Play();
            decisionFlag = false;
        }
    }
}



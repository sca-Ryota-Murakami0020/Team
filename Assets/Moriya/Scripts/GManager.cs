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
    //���C���[�𔭎˂����Ƃ��̌��ʉ��t���O
    private bool wireStartFlag = false;
    //���C���[�������������̌��ʉ��t���O
    private bool wireStopFlag = false;


    //�V���O���g��
    private void Awake()
    {
        //AudioSource�Ăяo��
        audios = GetComponent<AudioSource>();
        DontDestroyOnLoad(this.gameObject);
    }

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

    public bool WireStartFlag//���C���[�X�^�[�g
    {
        get { return this.wireStartFlag; }
        set { this.wireStartFlag = value; }
    }

    public bool WireStopFlag//���C���[�X�g�b�v
    {
        get { return this.wireStopFlag; }
        set { this.wireStopFlag = value; }
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

        if (wireStartFlag == true)
        {
            audios.clip = bgms[5];
            audios.Play();
            wireStartFlag = false;
        }

        if (wireStopFlag == true)
        {
            audios.clip = bgms[6];
            audios.Play();
            wireStopFlag = false;
        }
    }
}



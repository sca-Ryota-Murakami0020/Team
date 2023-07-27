using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class BillCameraC : MonoBehaviour
{
    //�Q�Ɛ�X�N���v�g
    //PasueDisplayC
    private PasueDisplayC pDC;

    //�ϐ��֌W 
    //�J�����ɉf���Ώ�
    [Header("�ǂ�������Ώہi�v���C���[�j")][SerializeField] 
    private GameObject player = null;
    //Player�̈ʒu
    private Vector3 playerPosition;
    //���݂̃J�����̃|�W�V����
    private Vector3 cameraPosition;
    //�J�����ƃv���C���[�Ƃ̋���
    private Vector3 cpDistance;
    //���̃J�����ɍs���������s���g���K�[�ɂȂ�I�u�W�F�N�g
    private GameObject oldCameraObject;
    //�J�����̈ړ������𔻕ʂ���I�u�W�F�N�g
    private GameObject disObjecto;
    //�ړ������𔻕ʂ��邽�߂̃I�u�W�F�N�g�̖��O���L������
    private string disObjectoName;
    //�ړ��O�̃J�����ړ��I�u�W�F�N�g�̖��O
    private string oldDisObjectName;
    //���������̃X�e�[�^�X
    enum STATE
    {
        NULL,
        X,
        Z,
        EMUP
    };
    //�X�e�[�^�X���Ǘ�����ϐ�
    private STATE state;
    //���݂̏�ʔԍ��̃X�e�[�^�X
    enum STAGE
    {
        Null,
        One,
        Two,
        Three,
        Fuor
    };
    //��ʔԍ��̃X�e�[�^�X�^�̕ϐ�
    private STAGE stage;


    //�v���p�e�B
    public GameObject OldCamera
    {
        get { return this.oldCameraObject;}
        set { this.oldCameraObject = value;}
    }

    public string DisObjectoName 
    {
        get { return this.disObjectoName;}
        set { this.disObjectoName = value;}
    }

    void Start()
    {
        //�X�N���v�g�̒�`
        pDC = FindObjectOfType<PasueDisplayC>();
        //�ϐ��֌W�̏�����
        state = STATE.Z;
        //��ʔԍ��ϐ��̏�����
        stage = STAGE.One;
        //�ʒu�̏�����
        //�v���C���[
        playerPosition = player.transform.position;
        //�J����
        cameraPosition = this.transform.position;
        cpDistance = cameraPosition - playerPosition;
        cameraPosition += cpDistance;
        this.transform.position = cameraPosition;     
    }

    void Update()
    {
        //�|�[�Y���Ă��Ȃ���
        if (pDC.MenuFlag == false)
        {
            //���Player�̈ʒu�ƃJ�����̈ʒu���擾����
            playerPosition = this.player.transform.position;
            //�v���C���[�����[���h���W��z�����Ɉړ����Ă���Ȃ�
            if(state == STATE.Z)
            {
                CameraMoveForStopX();
            }
            //�v���C���[�����[���h���W��x�����Ɉړ����Ă���Ȃ�
            if(state == STATE.X)
            {
                CameraMoveForStopZ();
            }
        }
        //Debug.Log(cpDistance);
    }

    public void CameraMover(string oldName, string getName)
    { 
        //string�^�̃��[�J���ϐ�change���`����
        //�[�����̊֐����ōs��switch���ɂ�鏈�����s���ۂɎg���ϐ��Ƃ��ėp����
        string change = null;
        //�����Ɋi�[���Ă��閼�O����v����
        // = �O�̃J�����̈ʒu�Ɉړ�����
        if (oldName == getName) change = "Back";
        //�����Ɋi�[���Ă��閼�O����v���Ȃ��A����ԍŏ��ɐG�ꂽ�̂�Empty�ł͂Ȃ��Ȃ�
        // = ���̃J�����̈ʒu�Ɉړ�����
        if(oldName != getName && getName != "Empty") change = "Next";
        //�����擾�������O��Empty�Ȃ�
        //�[�����̌�̃J�����̓����𐳏�Ȃ��̂ɂ���K�v�͂��邩��B
        if (oldName != getName && getName == "Empty") change = "Empty";
        switch (change)
        {
            case "Back":
                BackCamera();
                break;
            case "Next":
                NextCamera();
                break;
            case "Empty":
               NextCamera();
                break;
            default:
                break;
        }

        if(stage.ToString() == getName)
        {
            
        }
    }

    private void CameraMoveForStopX()
    {
        //this.transform.position = new Vector3(this.cameraPosition.x, playerPosition.y, playerPosition.z);
        //cameraPosition += cpDistance;
        this.transform.position = cameraPosition;
        //Debug.Log("Z���W�̈ړ�" + state);
    }

    private void CameraMoveForStopZ()
    {
        //this.transform.position = new Vector3(playerPosition.x, playerPosition.y, this.cameraPosition.z);
        //cameraPosition += cpDistance;
        this.transform.position = cameraPosition;
        //Debug.Log("X���W�̈ړ�" + state);
    }

    public void NextCamera()
    {
        //�v���C���[��y��������]���Ƃ���90�x��]����
        //transform.rotation = Quaternion.AngleAxis(this.transform.rotation.y - 90, player.transform.up);
        if (state == STATE.X)
        {
            state = STATE.Z;
        }
        if (state == STATE.Z)
        {
            state = STATE.X;
        }
        Debug.Log("Next");
    }

    public void BackCamera()
    {
        //�v���C���[��y��������]���Ƃ���-90�x��]����
        //transform.rotation = Quaternion.AngleAxis(this.transform.rotation.y + 90, player.transform.up);
        //�v���C���[�̈ړ��������X�V����
        if(state == STATE.X)
        {
            state = STATE.Z;
        }
        if(state == STATE.Z)
        {
            state = STATE.X;
        }
        Debug.Log($"Back");
    }
}

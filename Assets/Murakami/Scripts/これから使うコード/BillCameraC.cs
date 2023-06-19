using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class BillCameraC : MonoBehaviour
{
    //�g�p����o�[�`�����J�����ꗗ
    [SerializeField]
    private CinemachineVirtualCamera[] cameraList;
    //���̃J�����ɍs���������s���g���K�[�ɂȂ�I�u�W�F�N�g
    private GameObject oldCameraObject;
    //�J�����ɉf���Ώ�
    [SerializeField] private GameObject player = null;
    //Player�̈ʒu
    private Vector3 playerPosition;
    //���݂̃J�����̃|�W�V����
    private Vector3 cameraPosition;
    //�O�̃J�����̍��W
    private Vector3 oldCameraPosition;
    //PasueDisplayC
    private PasueDisplayC pDC;
    //BillCameraMveC
    private BillCameraMveC bCM;
    //�J�����̈ړ������𔻕ʂ���I�u�W�F�N�g
    private GameObject disObjecto;
    //�ړ������𔻕ʂ��邽�߂̃I�u�W�F�N�g�̖��O���L������
    private string disObjectoName;


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
        pDC = FindObjectOfType<PasueDisplayC>();
        bCM = FindObjectOfType<BillCameraMveC>();

    }

    private void Update()
    {
        //�|�[�Y���Ă��Ȃ���
        if (pDC.MenuFlag == false)
        {
            //���Player�̈ʒu�ƃJ�����̈ʒu���擾����
            playerPosition = this.player.transform.position;
        }
    }

    private void CameraMover(int number)
    {
        switch(number)
        {
            case 0:
                CameraMoveForStopX();
                break;
            case 1:
                CameraMoveForStopZ();
                break;
            case 2:
                CameraMoveForStopX();
                break;
            case 3:
                CameraMoveForStopZ();
                break;
            default:
                return;

        }
    }

    private void CameraMoveForStopX()
    {
        this.transform.position = new Vector3(this.cameraPosition.x, playerPosition.y, playerPosition.z);
        Debug.Log("Z���W�̈ړ�");
    }

    private void CameraMoveForStopZ()
    {
        this.transform.position = new Vector3(playerPosition.x, playerPosition.y, this.cameraPosition.z);
        Debug.Log("X���W�̈ړ�");
    }

    public void NextCamera()
    {
        //�ړ��O�̃J�������L������
        var oldVcamPrev = cameraList[cameraIndex];
        oldVcamPrev.Priority = unSelectCameraPriority;
        oldCameraPosition = oldVcamPrev.transform.position;

        //�����J�����̃C���f�b�N�X���J�����̗v�f���𒴂��Ă�����
        if (++cameraIndex > cameraList.Length)
        {
            cameraIndex = 0;
        }

        //���̖ʂ̃J�����Ɉڂ�
        var nowVcamPrev = cameraList[cameraIndex];
        nowVcamPrev.Priority = selectCameraPriority;
        Debug.Log("���̖ʂɈڂ�");
    }

    public void BackCamera()
    {
        //�ړ��O�̃J�������L������
        var oldVcamPrev = cameraList[cameraIndex];
        oldVcamPrev.Priority = unSelectCameraPriority;
        oldCameraPosition = oldVcamPrev.transform.position;

        //�����J�����̃C���f�b�N�X���J�����̗v�f���������
        if (--cameraIndex < 0)
        {
            cameraIndex = 3;
        }

        //���̖ʂ̃J�����Ɉڂ�
        var nowVcamPrev = cameraList[cameraIndex];
        nowVcamPrev.Priority = selectCameraPriority;
        Debug.Log("�O�̖ʂɈڂ�");
    }
}

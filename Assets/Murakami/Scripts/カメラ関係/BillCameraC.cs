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

    //�g���ĂȂ��J�����̗D��x
    private int unSelectCameraPriority = 0;
    //�g���Ă���J�����̗D��x
    private int selectCameraPriority = 10;
    //�I�𒆂̃J�����̃C���f�b�N�X
    private int cameraIndex = 0;
    //�O�̏�ʂɈڂ�
    private bool returnCamera = false;

    public GameObject OldCamera
    {
        get { return this.oldCameraObject;}
        set { this.oldCameraObject = value;}
    }

    //�o�[�`�����J�����̗D��x��������
    private void Awake()
    {
        if(cameraList == null || cameraList.Length <= 0)
        {
            return;
        }

        //�e�J�����̗D��x��������
        for(int i = 0; i < cameraList.Length; ++i)
        {
            //���ݎg���J�������J�����̗v�f���ƈꏏ�������ꍇ���̃J�����̗D��x��������
            cameraList[i].Priority = i == cameraIndex ? selectCameraPriority : unSelectCameraPriority;
        }
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
            cameraPosition = cameraList[cameraIndex].transform.position;

            //�g���Ă���J�����ɉ����������ɓK�p������
            for (int i = 0; i < cameraList.Length; i++)
            {
                if(cameraList[i].Priority == selectCameraPriority)
                {
                    CameraMover(i);
                }
            }
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
        cameraList[cameraIndex].transform.position = new Vector3(this.cameraPosition.x, playerPosition.y, playerPosition.z);
        Debug.Log("Z���W�̈ړ�");
    }

    private void CameraMoveForStopZ()
    {
        cameraList[cameraIndex].transform.position = new Vector3(playerPosition.x, playerPosition.y, this.cameraPosition.z);
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

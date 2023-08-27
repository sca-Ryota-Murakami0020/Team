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
    //�J�����̃|�W�V����
    [Header("�J�����̈ʒu")][SerializeField]
    private GameObject[] cameraPos;
    //�J�����̍��W���L������ϐ�
    private Vector3[] cameraPosVec;
    //�J�����̊p�x���L������ϐ�
    private Quaternion[] cameraPosRot;
    //�J�����̔Ԓn
    private int cameraPosNumber;
    //�J�����������ɓ����Ȃ��l�ɂ��邽�߂̃t���O
    private bool canMoveCamera = true;

    void Start()
    {
        //�X�N���v�g�̒�`
        pDC = FindObjectOfType<PasueDisplayC>();

        //�z�񐔂̒�`��
        cameraPosVec = new Vector3 [4];
        cameraPosRot = new Quaternion [4];
        for(int count = 0; count < 4; count++)
        {           
            //���W�̋L��
            cameraPosVec[count] = cameraPos[count].transform.position;
            Debug.Log("���_");
            //�p�x�̋L��
            cameraPosRot[count] = cameraPos[count].transform.rotation;
        }

        //�ʒu�̏�����
        //�J����
        cameraPosNumber = 0;
        this.transform.position = cameraPosVec[cameraPosNumber];
        this.transform.rotation = cameraPosRot[cameraPosNumber];
    }

    public void CameraMover(string oldName, string getName)
    { 
        //string�^�̃��[�J���ϐ�change���`����
        //�[�����̊֐����ōs��switch���ɂ�鏈�����s���ۂɎg���ϐ��Ƃ��ėp����
        string change = null;
        //�����Ɋi�[���Ă��閼�O����v����
        // = �O�̃J�����̈ʒu�Ɉړ�����
        if (oldName == getName && (getName != "Empty" || getName != "Start")) change = "Back";
        //�����Ɋi�[���Ă��閼�O����v���Ȃ��A����ԍŏ��ɐG�ꂽ�̂�EmptyorStart�ł͂Ȃ��Ȃ�
        // = ���̃J�����̈ʒu�Ɉړ�����
        if(oldName != getName && (getName != "Empty" || getName != "Start")) change = "Next";
        //�����擾�������O��EmptyorStart�Ȃ牽�����Ȃ��l�ɂ���
        //�[�����̌�̃J�����̓����𐳏�Ȃ��̂ɂ���K�v�͂��邩��B
        //�܂��A�Q�[���J�n���ɓ��ރI�u�W�F�N�g���K��Start�ɂȂ��Ă��邩��
        if (oldName == getName && (getName == "Empty" || getName == "Start")) change = "Empty";
        switch (change)
        {
            case "Back":
                BackCamera();
                break;
            case "Next":
                NextCamera();
                break;
            //�������Ȃ�
            case "Empty":
                break;
            default:
                break;
        }
    }

    public void NextCamera()
    {
        if(canMoveCamera)
        {
            //���̃J�����̃|�W�V�����Ɉړ�����
            cameraPosNumber++;
            this.transform.position = cameraPosVec[cameraPosNumber];
            //�ύX��̃I�u�W�F�N�g�̓J�����̌����ׂ��p�x�������Ă���̂Ŋp�x���ύX����B
            this.transform.rotation = cameraPosRot[cameraPosNumber];
            Debug.Log("Next");
        }       
    }

    public void BackCamera()
    {
        if(canMoveCamera)
        {
            //�O�̃J�����̃|�W�V�����Ɉړ�����
            cameraPosNumber--;
            this.transform.position = cameraPosVec[cameraPosNumber];
            //�ύX��̃I�u�W�F�N�g�̓J�����̌����ׂ��p�x�������Ă���̂Ŋp�x���ύX����B
            this.transform.rotation = cameraPosRot[cameraPosNumber];
            Debug.Log($"Back");
        }
    }

    private IEnumerator changeFlag()
    {
        //�J�����̍s����2�b�ԕs�ɂ���
        canMoveCamera = false;
        yield return new WaitForSeconds(3);
        //2�b��ɍs���\�ɂ���
        canMoveCamera = true;
        yield return null;
    }
}

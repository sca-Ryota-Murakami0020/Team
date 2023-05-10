using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillOnly : MonoBehaviour
{
    //�J�����ɉf���Ώ�
    [SerializeField] private GameObject player = null;
    //Player�̈ʒu
    private Vector3 playerPosition;
    //���̃J�����̈ʒu�ɂȂ�ꏊ
    [SerializeField] private GameObject[] nextCameraPosition;
    //��]����^����I�u�W�F�N�g
    [SerializeField] private GameObject axisObject;
    //��]����^����I�u�W�F�N�g�̃|�W�V����
    private Vector3 axisObjectPosition;

    //���f���Ă���ʂ̔ԍ�
    private int nowSceneNumber = 1;
    //1�O�ɉf���Ă����ʂ̔ԍ�
    private int oldSceneNumber = 1;
    //���̏�ʂɈړ����Ă鏈�����s���Ă���
    private bool moveFlag = false;

    //totalGameManager
    private totalGameManager tGM;
    //PasueDisplayC
    private PasueDisplayC pDC;

    #region//�v���p�e�B
    public int OldSN
    {
        get { return this.oldSceneNumber;}
        set { this.oldSceneNumber = value;}
    }

    public int NowSN
    {
        get { return this.nowSceneNumber;}
        set { this.nowSceneNumber = value;}
    }
    #endregion

    void Start()
    {
        tGM = FindObjectOfType<totalGameManager>();
        pDC = FindObjectOfType<PasueDisplayC>();
        axisObjectPosition = axisObject.transform.position;
        this.transform.position = nextCameraPosition[0].transform.position;
    }

    private void Update()
    {
        //���Player�̈ʒu���擾����
        playerPosition = this.player.transform.position;
        //���񏈗����Ă��Ȃ��Ԃ��|�[�Y���Ă��Ȃ���
        if (moveFlag == false && pDC.MenuFlag == false)
        {
            //�f���Ă�ʂ��X�^�[�g�������̖�or���̔��΂́i�ŏ��̕ǃW�����v������j�ʂȂ�
            if (nowSceneNumber == 1 || nowSceneNumber == 3)
            {
                this.transform.position = new Vector3(this.transform.position.x, playerPosition.y, playerPosition.z);
            }

            //�f���Ă���ʂ��X�^�[�g���������玟�̖�or����S�[�����z�u����Ă���ʂȂ�
            if (nowSceneNumber == 2 || nowSceneNumber == 4)
            {
                this.transform.position = new Vector3(playerPosition.x, playerPosition.y, this.transform.position.z);
            }
        }
    }

    //�J���������ɂǂ��̖ʂ��f�����̔��f
    public void Judge(int number)
    {
        Debug.Log("�����n������");
        switch(number)
        {
            case 1:
                oldSceneNumber = nowSceneNumber;
                nowSceneNumber = number;
                Debug.Log("����I��");
                StartCoroutine("FirstMoveCamera");
                break;
            case 2:
                oldSceneNumber = nowSceneNumber;
                nowSceneNumber = number;
                Debug.Log("����I��");
                StartCoroutine("SecondMoveCamera");
                break;
            case 3:
                oldSceneNumber = nowSceneNumber;
                nowSceneNumber = number;
                Debug.Log("����I��");
                StartCoroutine("TherdMoveCamera");
                break;
            case 4:
                oldSceneNumber = nowSceneNumber;
                nowSceneNumber = number;
                Debug.Log("����I��");
                StartCoroutine("ForceMoveCamera");
                break;
        }
    }

    //�X�^�[�g�������ɉf���Ă����X�e�[�W�Ɉړ�����
    private IEnumerator FirstMoveCamera()
    {
        yield return new WaitForSeconds(0.5f);
        //�ړ��̏���
        while(this.transform.position != nextCameraPosition[1].transform.position)
        {
            if (oldSceneNumber == 2 && this.transform.rotation.y <= 0)
            {
                transform.RotateAround(axisObjectPosition, Vector3.up, -2.0f);
            }

            if(oldSceneNumber == 4 && this.transform.rotation.y >= -180)
            {
                transform.RotateAround(axisObjectPosition, Vector3.up, 2.0f);
            }
        }
        yield break;
    }

    //�X�^�[�g���Ă��玟�ɉf��ʂɈړ�����
    private IEnumerator SecondMoveCamera()
    {
        yield return new WaitForSeconds(0.5f);
        //�ړ��̏���
        while (this.transform.position != nextCameraPosition[0].transform.position)
        {
            if (oldSceneNumber == 2 && this.transform.rotation.y <= 0)
            {
                transform.RotateAround(axisObjectPosition, Vector3.up, 2.0f);
            }

            if (oldSceneNumber == 4 && this.transform.rotation.y >= -180)
            {
                transform.RotateAround(axisObjectPosition, Vector3.up, 2.0f);
            }
        }
        yield break;
    }

    //�ŏ��ɕǃW�����v���g���ʂɈړ�����
    private IEnumerator TherdMoveCamera()
    {
        yield return new WaitForSeconds(0.5f);
    }

    //���ƃS�[�����ݒu���Ă���ʂɈړ�����
    private IEnumerator ForceMoveCamera()
    {
        yield return new WaitForSeconds(0.5f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillCameraOnlyContller : MonoBehaviour
{
    //��]�̒��S�_
    [SerializeField] private GameObject centerPosition;
    //��]��
    [SerializeField] private GameObject axisPosition;
    //�~�^������
    [SerializeField] private float period;
    //�J�����̈ړ������[���h���W��ŏc�ړ��ɂ���(1,3�ʂł̃J�����ړ�)
    private bool otFlag = true;
    //�J�����̈ړ������[���h���W��ŉ��ړ��ɂ���(2,4�ʂł̃J�����ړ�)
    private bool sfFlag = true;
    //PasueDisplayC
    private PasueDisplayC pDC;
    //BillCameraMveC
    private BillCameraMveC bCM;
    //�v���C���[�̈ʒu
    private Vector3 playerPosition;
    //�J�����̈ʒu
    private Vector3 cameraPosition;
    //�v���C���[
    [SerializeField] private GameObject player;

    #region//�v���p�e�B
    public bool OTFlag
    {
        get { return this.otFlag;}
        set { this.otFlag = value;}
    }

    public bool SFFlag
    {
        get { return this.sfFlag;}
        set { this.sfFlag = value;}
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        pDC = FindObjectOfType<PasueDisplayC>();
        bCM = FindObjectOfType<BillCameraMveC>();
    }

    // Update is called once per frame
    void Update()
    {
        //�|�[�Y���Ă��Ȃ���
        if (pDC.MenuFlag == false)
        {
            //���Player�̈ʒu�ƃJ�����̈ʒu���擾����
            playerPosition = this.player.transform.position;
            cameraPosition = this.transform.position;

            if(otFlag == true && sfFlag == false)
            {
                this.transform.position = new Vector3(this.cameraPosition.x, playerPosition.y, playerPosition.z);
                Debug.Log("Z���W�̈ړ�");
            }

            if(otFlag == false && sfFlag == true)
            {
                this.transform.position = new Vector3(playerPosition.x, playerPosition.y, this.cameraPosition.z);
                Debug.Log("X���W�̈ړ�");
            }
        }
    }

    public void GotoNextCameraPosition()
    {

    }

    public void GotoBeforeCameraPosition()
    {

    }
}

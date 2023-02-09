using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraC : MonoBehaviour
{
    #region//stage���ς���Ă��ς��Ȃ��ϐ�
    //���_�ƂȂ�I�u�W�F�N�g
    [SerializeField] private GameObject Player = null;
    //���񂵂�����x���W
    private float xpos;
    //���񂵂����̂����W
    private float zpos;
    //���ۂɃJ��������������W//�J�����ƃv���C���[�Ƃ̋���
    private Vector3 D;
    //Player��Ǐ]���Ȃ��ŃJ�����̌��������ς���͈�
    private float lookPlayerdistance = 0.1f;
    //�������̃}�E�X�̈ړ���
    private float mousex;
    //�c�����̃}�E�X�̈ړ���
    private float mousey; 
    //PLayer��Ǐ]���鑬�x
    private float cameraSpeed = 3.0f;
    //���_����J�����܂ł̋���
    private float cameraDistance = 4.6f;
    //�f�t�H���g�̍���
    private float cameraHeight = 2.5f;
    //����鎞�̑��x
    private float leaveCamera = 15.0f;
    //���_����J�����̋����̗V��
    private float cPDistance = 0.5f;
    //�c�ړ��p���x�{���i���l�������قǃJ�����̓����͒x���Ȃ�j
    private float verticalMag = 21.5f;
    //���ړ��p���x�{���i���l�������قǃJ�����̓����͒x���Ȃ�j
    private float holizontalMag = 21.4f;
    //�ǃW�����ŗp����X�N���v�g
    private PlayerWallCon pWC;
    //PasueDisplayC
    private PasueDisplayC pDC;
    #endregion

    #region//stage�ɂ���ĕς�鐔�l

    //���݂̃J�����̍���
    [SerializeField] private float nowCameraHeight;// = 1.0f;
    //�J�����̍Œፂ�x
    [SerializeField] private float cameraHeightMin;// = -5.0f;
    //�J�����̍ō����x
    [SerializeField] private float cameraHeightMax;// = 8.5f;

    #endregion

    void Start()
    {
        pWC = FindObjectOfType<PlayerWallCon>();
        pDC = FindObjectOfType<PasueDisplayC>();
        D = Player.transform.position;
    }

    //���_�ƃJ�������W�𐏎��X�V
    void Update()
    {
        if (Player == null) return;
        if(pDC.MenuFlag == false)
        {
            //�}�E�X�̈ړ��ʂ��擾
            mousex = Input.GetAxis("Mouse X");
            mousey = Input.GetAxis("Mouse Y");

            //�ʏ�̃J��������
            if ((Mathf.Abs(mousex) > 0.019f || Mathf.Abs(mousey) > 0.019f) && pWC.WallJumpHitFlag == false)
            {
                //135�s�ڂ���
                Roll(-mousex, -mousey);
            }

            //�ǃW�������̃J�����̑���pWC.WallJumpHitFlag == true && 
            if (Mathf.Abs(mousex) > 0.019f && pWC.WallJumpHitFlag == true)
            {
                //185�s�ڂ���
                PlayerDoWallJump(-mousex);
            }

            //���_�̃��Z�b�g
            if (Input.GetKey(KeyCode.RightShift))
            {
                //165�s�ڂ���
                Reset();
            }

            //94�s�ڂ���
            UpdateLookPosition();

            //109�s�ڂ���
            UpdateCameraPosition();

            //��Ƀv���C���[�̂������������
            this.transform.LookAt(D);
        }

        //�|�[�Y�������ʂŉ����{�^�����������Ƃ�
        if(pDC.OnlyFlag == true && Input.anyKey)
        {
            //���������ʂ����
            pDC.CloseManual();
        }
    }

    //�J�������_�̐���
    public void UpdateLookPosition()
    {
        //�ڕW�̎��_�ƌ��݂̎��_�̋��������߂�
        Vector3 vec = Player.transform.position - D;
        float distance = vec.magnitude;
        if (distance > lookPlayerdistance)
        {
            //���͈͂𒴂�����ڕW�Ɏ��_�ɋ߂Â���
            float move_distance = (distance - lookPlayerdistance) * (Time.deltaTime * cameraSpeed);
            D += vec.normalized * move_distance;
        }
    }

    //�J�������W�̐���
    public void UpdateCameraPosition()
    {
        //XZ���ʂɂ�����J�����Ǝ��_�̋����𓒓�
        Vector3 xz_vec = Player.transform.position - this.transform.position;
        xz_vec.y = 0;
        float distance = xz_vec.magnitude;

        //�J�����̈ړ����������߂�
        float move_distance = 0;
        if (distance > cameraDistance + cPDistance)
        {
            //�J�����̉�]����͈͂𒴂�����ǂ�������
            move_distance = distance - (cameraDistance + cPDistance);
            move_distance *= Time.deltaTime * cameraSpeed;
        }

        //�V�����J�����̈ʒu�����߂�
        Vector3 camera_pos = this.transform.position + (xz_vec.normalized * move_distance);

        //�����͌��ݎ��h����Ɉ��ňێ�����
        camera_pos.y = D.y + nowCameraHeight;
        this.transform.position = camera_pos;
    }

    //�J�����̉�]
    public void Roll(float x, float y)
    {
        //�ړ��O�̋�����ێ�
        float prev_distans = Vector3.Distance(Player.transform.position, this.transform.position);
        Vector3 pos = this.transform.position;

        //���ړ�
        pos += this.transform.right * x / holizontalMag;

        //�c�ړ�
        nowCameraHeight = Mathf.Clamp(nowCameraHeight + y / verticalMag, cameraHeightMin, cameraHeightMax);
        pos.y = D.y + nowCameraHeight;

        //�ړ���̋������擾
        float after_distance = Vector3.Distance(Player.transform.position, pos);

        //���_��Ώۂɋ߂Â���i�]�T���Ȃ����j
        D = Vector3.Lerp(D, Player.transform.position, 0.1f);

        //�J�����̍X�V
        this.transform.position = pos;
        this.transform.LookAt(D);

        //���s�ړ��ɂ��኱�������ς��̂ŕ␳����
        this.transform.position += transform.forward * (after_distance - prev_distans);
    }

    //�J�������Z�b�g
    public void Reset(float rate = 1.0f)
    {
        //���_�Ώۂɋ߂Â���
        D = Vector3.Lerp(D, Player.transform.position, rate);

        //�������f�t�H���g�ɕς���
        nowCameraHeight = Mathf.Lerp(nowCameraHeight, cameraHeight, rate);

        //�J�����̊�{�ʒu�ɋ߂Â���
        Vector3 pos_goal = Player.transform.position;
        pos_goal = Player.transform.position * cameraDistance;
        pos_goal.y = Player.transform.position.y + nowCameraHeight;
        this.transform.position = Vector3.Lerp(this.transform.position, pos_goal, rate);

        //���_���X�V����
        this.transform.LookAt(D);

    }

    //�ǃW�������̃J�����̉�]
    public void PlayerDoWallJump(float x)
    {
        //�ړ��O�̋�����ێ�
        float prev_distans = Vector3.Distance(Player.transform.position, this.transform.position);
        Vector3 pos = this.transform.position;

        //���ړ�
        pos += this.transform.right * x / holizontalMag;

        //�ړ���̋������擾
        float after_distance = Vector3.Distance(Player.transform.position, pos);

        //���_��Ώۂɋ߂Â���i�]�T���Ȃ����j
        //D = Vector3.Lerp(D, Player.transform.position, 0.1f);

        //�J�����̍X�V
        this.transform.position = pos;
        this.transform.LookAt(D);

        //���s�ړ��ɂ��኱�������ς��̂ŕ␳����
        this.transform.position += transform.forward * (after_distance - prev_distans);
    }
}

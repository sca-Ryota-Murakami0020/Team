using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraC : MonoBehaviour
{
    //���_�ƂȂ�I�u�W�F�N�g
    [SerializeField] private GameObject Player = null;
    //���񂵂�����x���W
    private float xpos;
    //���񂵂����̂����W
    private float zpos;
    //���ۂɃJ��������������W//�J�����ƃv���C���[�Ƃ̋���
    private Vector3 D = Vector3.zero;//Lookpos
    //Player��Ǐ]���Ȃ��ŃJ�����̌��������ς���͈�
    private float lookPlayerdistance = 0.1f;
    //PLayer��Ǐ]���鑬�x
    private float cameraSpeed = 4.0f;//=followSmooth 
    //���_����J�����܂ł̋���
    private float cameraDistance = 15.5f;
    //�f�t�H���g�̍���
    private float cameraHeight = 10.5f;
    //���݂̃J�����̍���
    private float nowCameraHeight = 3.0f;//currentCameraHeight = 1.0f;
    //���_����J�����̋����̗V��
    private float cPDistance = 0.5f;//cameraPlayDiatance = 0.3f; 
    //����鎞�̑��x
    private float leaveCamera = 10.0f;//leaveSmooth
    //�J�����̍Œፂ�x
    private float cameraHeightMin = -5.0f;
    //�J�����̍ō����x
    private float cameraHeightMax = 8.5f;
    //�������̃}�E�X�̈ړ���
    private float mousex;
    //�c�����̃}�E�X�̈ړ���
    private float mousey;

    private Vector3 rayHitPosition;

    //[SerializeField] private GameObject bullet;

    [SerializeField] private GameObject bulletSponePosition;

    private Vector3 dir;

    public Vector3 RayHitPosition
    {
        get { return this.rayHitPosition;}
        set { this.rayHitPosition = value;}
    }

    public Vector3 Dir
    {
        get { return this.dir;}
        set { this.dir = value;}
    }

    void Start()
    {
        Player = GameObject.Find("Player");
    }

    //���_�ƃJ�������W�𐏎��X�V
    void Update()
    {    
        if (Player == null) return;
        //�}�E�X�̈ړ��ʂ��擾
        mousex = Input.GetAxis("Mouse X");
        mousey = Input.GetAxis("Mouse Y");

        if (Mathf.Abs(mousex) > 0.008f || Mathf.Abs(mousey) > 0.005f)
        {
            Roll(mousex, mousey);
        }
        if (Input.GetMouseButtonDown(1))
       {
            GetShotVector();
        }

        UpdateLookPosition();
        UpdateCameraPosition();
        this.transform.LookAt(D);
    }

    //�J�������_�̐���pl.FuckFlag == true && 
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
        if (Input.GetKey(KeyCode.RightShift))
        {
            Reset();
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
        pos += this.transform.right * x;

        //�c�ړ�
        nowCameraHeight = Mathf.Clamp(nowCameraHeight + y, cameraHeightMin, cameraHeightMax);
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

    
    public void GetShotVector()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit isHit;// = Physics.Raycast((Vector3) ray.origin,(Vector3) ray.direction,isHit);
        if(Physics.Raycast(ray, out isHit, Mathf.Infinity))
        {
            // ray�̓��������ʒu - �{�[���ʒu�Ԃ̌v�Z���s���A�x�N�g�����擾�iy���W�̂݃{�[���̍��W���̗p�j
            rayHitPosition = new Vector3(isHit.point.x, isHit.point.y, isHit.point.z); 
            Debug.Log("rayHitPos" + rayHitPosition);
            dir = (isHit.point - bulletSponePosition.transform.position).normalized;
            //Instantiate(bullet, new Vector3(dir.x,dir.y,dir.z), Quaternion.identity);
            Debug.Log("�͂�����");
        }
        Debug.Log(dir);
        Debug.DrawRay(ray.origin,ray.direction * 10, Color.green, 5);

    }
    
}


/*
//�������Ɉ��ʈړ����Ă���Ή���]
if (Mathf.Abs(mousex) > 0.001f)
{
    //��]���̓��[���h���W�̂x��
    transform.RotateAround(Player.transform.position, Vector3.up, mousex);
}
if (Mathf.Abs(mousey) > 0.001f)
{
    //��]���̓��[���h���W��x��
    if (Player.transform.rotation.x <= 80.0f || Player.transform.rotation.x >= -80.0f)
    {
        transform.RotateAround(Player.transform.position, Vector3.up, mousey);
    }
}
*/


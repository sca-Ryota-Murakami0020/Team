using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireGun : MonoBehaviour
{
    //�R���|�[�l���g
    //private Animator animator;
    private SpringJoint springJoint;
    private LineRenderer lineRenderer;
    private Player player;
    private Transform cameraTransForm;

    private CameraC camera;

    [SerializeField] private GameObject bullet;
    private bool bulletShootingFalg = false;
    private Vector3 normalDirection;


    private float maxDistance;//����L�΂���ő勗��
    [SerializeField] private LayerMask wireLayers; //�������������郌�C���[
    [SerializeField] private Vector3 casterCenter = new Vector3(0.0f, 0.5f, 0.0f);//�I�u�W�F�N�g�̃��[�J�����W�ŕ\�������̎ˏo�ʒu

    [SerializeField] private float spring = 50.0f;// ���̕����I������S������SpringJoint��spring
    [SerializeField] private float damper = 20.0f;// ���̕����I������S������SpringJoint��damper
    [SerializeField] private float equilibrumLength;//�����k�߂����̎��R��

    private bool casting;//�����ˏo�����ǂ����̕\���t���O
    private bool needsUpdateSpring; // FixedUpdate����SpringJoint�̏�ԍX�V���K�v���ǂ�����\���t���O
    private float stringLength; // ���݂̎��̒���...���̒l��FixedUpdate����SpringJoint��maxDistance�ɃZ�b�g����
    private readonly Vector3[] stringAnchor = new Vector3[2]; // SpringJoint�̃L�����N�^�[���Ɛڒ��_���̖��[
    private Vector3 worldCasterCenter; // casterCenter�����[���h���W�ɕϊ���������

    public bool BulletShootingFalg
    {
        get { return this.bulletShootingFalg; }
        set { this.bulletShootingFalg = value; }
    }

    public Vector3 NormalDirection
    {
        get { return this.normalDirection; }
        set { this.normalDirection = value; }
    }

      
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        camera = FindObjectOfType<CameraC>();
        //animator = this.GetComponent<Animator>();
        cameraTransForm = Camera.main.transform;
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        // �܂���ʒ��S����^�����ʂɐL�т�Ray�����߁A�����worldCasterCenter����
        // ����Ray�̏Փ˓_�Ɍ�����Ray�����߂�...��������̎ˏo�����Ƃ���
        //this.worldCasterCenter = this.transform.TransformPoint(this.casterCenter);
        //Ray pointerRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Ray aimingRay = new Ray(this.worldCasterCenter, Physics.Raycast(pointerRay, out var focus, float.PositiveInfinity, this.wireLayers) ? focus.point - this.worldCasterCenter : pointerRay.direction);
        //���[�J�����W�����[���h���W�ɕϊ�
        //���ɐV����Ray�����(�X�N���[���̓_��ʂ��ăJ�������烌�C��ʂ�)
        //��������p��Ray��V�������
        //Ray(Ray�̔����n�_,Ray�̐i�ޕ���(���[���h���W��Ray����,hit�����I�u�W�F�N�g���t�H�[�J�X,Ray�̒����͐��̖����吔,�Փ˂���rayer�͎w�肵�����̂�
        //���̃|�C���g���烏�[���h���W�̃J����Ray�̊p�x������))


        if (Input.GetMouseButtonDown(0))
        {
            bulletShootingFalg = true;
            StartWireGun();
        }
    }

    private void StartWireGun()
    {
        normalDirection = (camera.RayHitPosition - transform.position).normalized;
        GameObject Bullet_obj = (GameObject)Instantiate(bullet, transform.position, Quaternion.Euler(0.0f, 0.0f, 0.0f));
        Bullet bullet_cs = Bullet_obj.GetComponent<Bullet>();
        bulletShootingFalg = false;

    }

    private void StopWireGun()
    {
      
    }
}

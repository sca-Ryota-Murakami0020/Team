using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireGun : MonoBehaviour
{
    private float bulletDisTime = 0;//�e�������銴�o
    //�R���|�[�l���g
    //private Animator animator;
    private LineRenderer lineRenderer;
    private SpringJoint springJoint;
    private float maxDistance = 100f;//�ő勗��
    private Color color = new Color32(248,168,133,1);//�f�t�H�̐F
    private GameObject target;//���C���������������̏�Q��

    private Player player;//�v���C���[

    [SerializeField]
    private Transform parentTran;//line�̏��߂̏ꏊ

    private CameraC camera;//�J������`

    [SerializeField] private GameObject bullet;//�e
    private bool bulletShootingFalg = false;//�e�����˂��鎞�̃t���O

    public bool BulletShootingFalg
    {
        get { return this.bulletShootingFalg; }
        set { this.bulletShootingFalg = value; }
    }

    public GameObject Target
    {
        get { return this.target; }
        set { this.target = value; }
    }
      
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        camera = FindObjectOfType<CameraC>();
        //animator = this.GetComponent<Animator>();
        lineRenderer = GetComponent<LineRenderer>();
      
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))//���N����������
        {

            lineRenderer.SetPosition(0, transform.position);//���C���[�̎n�_��WireGun�I�u�W�F�N�g����
            lineRenderer.transform.SetParent(parentTran);//�v���C���[���������Ƃ��̃|�W�V������LineRenderer�̎n�_������
            if (camera.RayTrueFlag == true)
            {
                // line�̏I�_
                lineRenderer.SetPosition(1, camera.IsHit.point);//�J�����̃��C������������
                target = camera.IsHit.collider.gameObject;//���������I�u�W�F�N�g

                Debug.Log(target);
                Debug.Log(camera.Dir);
                Vector3 dir = camera.Dir;//���C�̒P�ʃx�N�g��

                //if (bulletShootingFalg == true)
                //{
                ConnectWireCoroutine(target);//�X�v�����O�W���C���g
                bulletDisTime += Time.deltaTime;//�e�̔��ˊ��o�J�E���g
                /*if(bulletDisTime == 5.0f)
                  {
                    bulletDisTime = 0.0f;//�J�E���g���Ȃ���
                    bulletShootingFalg = false;//���˂���悤�Ƀt���O��܂�
                    camera.RayTrueFlag = false;//���C���������擾����p�Ƀt���O��܂�
                    DestroyWireCoroutine();
                  }
                }*/
            }
            else
            {
                //������Ԃɖ߂�
                lineRenderer.SetPosition(1, camera.CameraRay.origin + (camera.Dir * maxDistance));//lineRenderer�̃|�W�V�������X�V
                lineRenderer.startColor = color;
                lineRenderer.endColor = color;
            }
        }
    }

   void ConnectWireCoroutine(GameObject target)
   {
        Debug.Log(target);
        springJoint.connectedBody = target.GetComponent<Rigidbody>();
        springJoint.autoConfigureConnectedAnchor = false;
        springJoint.anchor = new Vector3(0f,1f,0f);
        springJoint.connectedAnchor = new Vector3(0f,-0.5f,0f);
        springJoint.spring =20f;
        springJoint.damper = 0.5f;
   }

    void DestroyWireCoroutine()
    {
        if (springJoint != null)
        {
            Destroy(springJoint);
        }
    }


}

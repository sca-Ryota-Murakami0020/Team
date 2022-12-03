using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyC : MonoBehaviour
{
    //�v���C���[�̏����擾���邽�߂Ɏg�p
    private PlayerC pl;
    //�ړ����x
    [SerializeField] private float enemySpeed = 0.3f;
    //�ʒu
    private Vector3 pos;
    //�ړ���
    private float wide = 8.0f;
    //RigidBody
    Rigidbody rb;
    //�m�b�N�o�b�N���x
    private float speed = 1.5f;
    //�������
    private float theta;
    //�X�^�[�g�n�_
    [SerializeField] private GameObject startPoint;
    //
    [SerializeField] private GameObject endPoint;

    // Start is called before the first frame update
    void Start()
    {
        var rb = GetComponent<Rigidbody>();
        pl = GameObject.Find("Player").GetComponent<PlayerC>();
        theta = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        //if()
        //this.pos = this.transform.position;
        var diff = pl.transform.position - transform.position;
        var axis = Vector3.Cross(transform.forward, diff);
        var angle = Vector3.Angle(transform.forward, diff) * (axis.y < 0 ? -1 : 1);
        if (angle <= 70.0f && angle >= -70.0f)
        {
            // ray cast
            RaycastHit hit;
            Vector3 temp = pl.transform.position - this.transform.position;
            Vector3 normal = temp.normalized;
            if (Physics.Raycast(this.transform.position, normal, out hit, 50))
            {
                if (hit.transform.gameObject == pl)
                {
                    // �Ώۂ̃I�u�W�F�N�g�����E�ɓ��������̏���
                }
                else this.transform.position += transform.forward * 0.7f;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            pl.Hp -= 1;
            Vector3 ver = (pl.transform.position - this.transform.position).normalized;
            ver.y = 0;
            ver = ver.normalized;
            collision.transform.Translate(ver * speed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == startPoint)
        {
            this.transform.LookAt(endPoint.transform.position);
        }

        else if(other.gameObject == endPoint)
        {
            this.transform.LookAt(startPoint.transform.position);
        }
    }
}

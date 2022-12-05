using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyC : MonoBehaviour
{
    //�v���C���[�̏����擾���邽�߂Ɏg�p
    private PlayerC pl;
    //�ړ����x
    [SerializeField] private float enemySpeed = 0.7f;
    //�ʒu
    private Vector3 pos;
    //RigidBody
    Rigidbody rb;
    //�m�b�N�o�b�N���x
    private float speed = 1.5f;
    //�ǐՒ��g���X�s�[�h
    private float addSpeed = 3.0f;
    //�ǐՃt���O
    private bool doEncount;
    //�X�^�[�g�n�_
    [SerializeField] private GameObject startPoint;
    //�I���_
    [SerializeField] private GameObject endPoint;

    public bool DoEn
    {
        get { return this.doEncount;}
        set { this.doEncount = value;}
    }

    // Start is called before the first frame update
    void Start()
    {
        var rb = GetComponent<Rigidbody>();
        pl = GameObject.Find("Player").GetComponent<PlayerC>();
        this.transform.position = startPoint.transform.position;
        doEncount = false;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(this.doEncount);

        if (this.doEncount)
        {
            this.transform.position += transform.forward * addSpeed * Time.deltaTime;//0.7f;
            Debug.Log("�ǐ�");
        }
        else
        {
            this.transform.position += transform.forward * enemySpeed * Time.deltaTime;
            Debug.Log("����");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            pl.Hp -= 1;
            Vector3 ver = (pl.transform.position - this.transform.position).normalized;
            ver.y = 0;
            ver = ver.normalized;
            collision.transform.Translate(ver * speed);
            Debug.Log("Dameze"); //* Time.deltaTime
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == startPoint && !this.doEncount)
        {
            this.transform.LookAt(endPoint.transform.position);
            //Debug.Log("start�J�n");
        }

        else if (other.gameObject == endPoint && !this.doEncount)
        {
            this.transform.LookAt(startPoint.transform.position);
            //Debug.Log("end�J�n");
        }

        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("start�J�n");
            doEncount = true;
        }
    }
}

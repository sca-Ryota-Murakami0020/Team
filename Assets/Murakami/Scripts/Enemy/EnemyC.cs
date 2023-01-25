using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO.MemoryMappedFiles;

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
    //����J�n�̃t���O
    private bool startFlag;
    //�X�^�[�g�n�_
    [SerializeField] private GameObject startPoint;
    //�I���_
    [SerializeField] private GameObject endPoint;
    //�\���̊Ǘ����s��
    private ResetEnemyPosition rEP;
    //ray�֌W
    private float rayDistance = 2.0f;
    //��]�����̔���
    private bool doTurn;
    //ray���΂��I�u�W�F�N�g
    [SerializeField] private GameObject shotRayPosition;
    //��]�̎���ݒ肷�邽�߂ɕK�v
    private Quaternion defaultRotation;
    //��]����
    private float rotateTime = 0.0f;
    //�ŏ��ɐU������������������ɂ���
    private bool noCountFlag;
    //��]��
    private int rotateCounter;
    //��]��������
    private float rotationDistance;

    //�G�̉�]�������
    enum RotationPar
    {
        NULL,
        RIGHT,
        LEFT,
        RESET,
        TURN,
    };
    
    RotationPar rotationState;

    #region//�v���p�e�B
    public GameObject StartP
    {
        get { return this.startPoint;}
        set { this.startPoint = value;}
    }

    public GameObject EndP
    {
        get { return this.endPoint;}
        set { this.endPoint = value;}
    }

    public bool DoEn
    {
        get { return this.doEncount;}
        set { this.doEncount = value;}
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        var rb = GetComponent<Rigidbody>();
        pl = GameObject.Find("Player").GetComponent<PlayerC>();
        rEP = GameObject.Find("startPos").GetComponent<ResetEnemyPosition>();
        this.transform.position = this.startPoint.transform.position;
        doEncount = false;
        startFlag = true;
        doTurn = false;
        noCountFlag = true;
        rotateCounter = 0;
        rotationState = RotationPar.NULL;
        rotationDistance = 0.0f;

        this.transform.LookAt(endPoint.transform.position);

    }

    // Update is called once per frameshotRayPosition.transform.position, 
    void Update()
    {
        //�����Ői�s���Player�����m����
        Vector3 rayPosition = shotRayPosition.transform.position;
        RaycastHit hit;
        Ray ray = new Ray(rayPosition, this.gameObject.transform.forward);
        Debug.DrawRay(shotRayPosition.transform.position, shotRayPosition.transform.forward * this.rayDistance, Color.red ,1.0f);
        if (Physics.Raycast(ray, out hit, 100.0f))
        {
            //Debug.Log("rayHit");
            if(hit.collider.gameObject.CompareTag("Player"))
            {
                //Debug.Log("ray");
                this.doEncount = true;
            }        
        }

        //���x�֌W
        //Debug.Log("doTurn: " + doTurn);
        if (this.doEncount) this.transform.position += transform.forward * addSpeed * Time.deltaTime;
        else
        {
            if(!doTurn)
            {
                this.transform.position += transform.forward * enemySpeed * Time.deltaTime;
            }
            else
            {
                this.transform.position += new Vector3(0,0,0);
            }
        }
        //Debug.Log("�G���J�E���g:" + rotationState);
    }

    private void OnCollisionEnter(Collision collision)
    {

        if ((collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("WallJumpPoint") || collision.gameObject.CompareTag("LimitWall")) && doEncount == true)
        {
            //Destroy(this.gameObject);
            //eSC.SponeEnemy();
            StartCoroutine("ResetEnemy");
            //Debug.Log("�����n��");
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {      
        if (other.gameObject == this.startPoint && noCountFlag == false)
        {
            returnLookEndPosition();
        }

        if (other.gameObject == this.endPoint)
        {
            returnLookStartPosition();
        }
    }

    public void returnLookStartPosition()
    {
        //endPoint�̕����Ɍ�����悤�ɂ��邽�߂ɒl��ύX����
        defaultRotation = this.transform.rotation;
        doTurn = true;
        rotationState = RotationPar.RIGHT;
        StartCoroutine("TurnLookForStartPoint");
        //Debug.Log("�n�_���瓮���o��");
    }

    public void returnLookEndPosition()
    {
        //startPoint�̕����Ɍ�����悤�ɂ��邽�߂ɒl��ύX����
        defaultRotation = this.transform.rotation;
        doTurn = true;
        rotationState = RotationPar.RIGHT;
        StartCoroutine("TurnLookForEndPoint");
        //Debug.Log("�I�_���瓮���o��");     
    }

    private IEnumerator ResetEnemy()
    { 
        this.transform.position = this.startPoint.transform.position;
        this.doEncount = false;
        rEP.StartCountDistance();
        this.gameObject.SetActive(false);
        //Debug.Log("�ҋ@��");
        yield break;
    }

    private IEnumerator TurnLookForStartPoint()
    {
        yield return new WaitForSeconds(1);       
        while(rotationState == RotationPar.RIGHT)
        {
            //��x����]
            this.transform.Rotate(0,1.0f,0);
            rotateCounter++;

            //�n�߂͏����ɂ₩�ɉ�]
            if (rotateCounter < 3) yield return new WaitForSeconds(0.1f);
            //f�ɂ̉�]���x�ŉ�]
            if (rotateCounter >= 3 && rotateCounter < 40) yield return new WaitForSeconds(0.01f);
            //�Ō�͏����ɂ₩�ɉ�]����
            if (rotateCounter >= 40) yield return new WaitForSeconds(0.075f);

            //��]�����x����45���𒴂�����
            if (rotateCounter >=45)
            {
                rotationState = RotationPar.LEFT;
                rotateCounter = 0;
                yield return new WaitForSeconds(1);
                Debug.Log("����]�Ɉڍs����");
                break;
            }         
        }

        //�������ɉ�]
        while(rotationState == RotationPar.LEFT)
        {
            //transform.rotation = Quaternion.AngleAxis(-2.0f, this.transform.up) * defaultRotation;]rotationDistance = defaultRotation.y + 75.0f;
            this.transform.Rotate(0, -1.0f, 0);
            rotateCounter++;

            if(rotateCounter < 3) yield return new WaitForSeconds(0.1f);

            if (rotateCounter >= 3 && rotateCounter < 85) yield return new WaitForSeconds(0.01f);

            if (rotateCounter >= 85) yield return new WaitForSeconds(0.075f);

            //Debug.Log("����]��");
            if (rotateCounter >= 90)
            {
                rotationState = RotationPar.RESET;
                rotateCounter = 0;
                yield return new WaitForSeconds(1);
                Debug.Log("���Z�b�g�Ɉڍs����");
                break;
            }
        }

        //���ʒu�ɖ߂�
        while(rotationState == RotationPar.RESET)
        {
            //transform.rotation = Quaternion.AngleAxis(-2.0f, this.transform.up) * defaultRotation;
            this.transform.Rotate(0, 1.0f, 0);
            rotateCounter++;

            if (rotateCounter < 3) yield return new WaitForSeconds(0.1f);

            if (rotateCounter >= 3 && rotateCounter < 40) yield return new WaitForSeconds(0.01f);

            if (rotateCounter >= 40) yield return new WaitForSeconds(0.075f);

            if (rotateCounter >= 45)
            {
                rotationState = RotationPar.TURN;
                rotateCounter = 0;
                yield return new WaitForSeconds(1);
                Debug.Log("OK,Go");
                break;
            }         
        }

        while(rotationState == RotationPar.TURN)
        {
            //transform.rotation = Quaternion.AngleAxis(2.0f, this.transform.up) * defaultRotation;
            this.transform.Rotate(0, 1.0f, 0);
            rotateCounter++;

            if (rotateCounter < 3) yield return new WaitForSeconds(0.01f);

            if (rotateCounter >=3 && rotateCounter <= 175) yield return new WaitForSeconds(0.00075f);

            if (rotateCounter >= 175) yield return new WaitForSeconds(0.075f);

            if (rotateCounter >= 180)
            {
                Debug.Log("TURN");
                rotateCounter = 0;
                yield return new WaitForSeconds(1);
                break;
            }           
        }
        
        doTurn = false;
        rotationState = RotationPar.NULL;
        if(noCountFlag == true)
        {
            noCountFlag = false;
        }
        yield break;
    }

    private IEnumerator TurnLookForEndPoint()
    {
        yield return new WaitForSeconds(1);
        while (rotationState == RotationPar.RIGHT)
        {
            //transform.rotation = Quaternion.AngleAxis(2.0f, this.transform.up) * defaultRotation;rotationDistance = defaultRotation.y - 75.0f;
            this.transform.Rotate(0, 1.0f, 0);
            rotateCounter++;

            if (rotateCounter < 3) yield return new WaitForSeconds(0.1f);

            if (rotateCounter >= 3 && rotateCounter < 40) yield return new WaitForSeconds(0.01f);

            if (rotateCounter >= 40) yield return new WaitForSeconds(0.075f);

            //Debug.Log("�E��]��");
            if (rotateCounter >= 45)
            {
                rotationState = RotationPar.LEFT;
                rotateCounter = 0;
                yield return new WaitForSeconds(1);
                Debug.Log("����]�Ɉڍs����");
                break;
            }
        }

        //�������ɉ�]
        while (rotationState == RotationPar.LEFT)
        {
            //transform.rotation = Quaternion.AngleAxis(-2.0f, this.transform.up) * defaultRotation;]rotationDistance = defaultRotation.y + 75.0f;
            this.transform.Rotate(0, -1.0f, 0);
            rotateCounter++;

            if (rotateCounter < 3) yield return new WaitForSeconds(0.1f);

            if (rotateCounter>= 3 && rotateCounter < 85) yield return new WaitForSeconds(0.01f);

            if(rotateCounter >= 80) yield return new WaitForSeconds(0.075f);

            //Debug.Log("����]��");
            if (rotateCounter >= 90)
            {
                rotationState = RotationPar.RESET;
                rotateCounter = 0;
                yield return new WaitForSeconds(1);
                Debug.Log("���Z�b�g�Ɉڍs����");
                break;
            }
        }

        //���ʒu�ɖ߂�
        while (rotationState == RotationPar.RESET)
        {
            //transform.rotation = Quaternion.AngleAxis(-2.0f, this.transform.up) * defaultRotation;
            this.transform.Rotate(0, 1.0f, 0);
            rotateCounter++;

            if (rotateCounter < 3) yield return new WaitForSeconds(0.1f);

            if (rotateCounter >= 3 && rotateCounter < 35) yield return new WaitForSeconds(0.01f);

            if (rotateCounter >= 35) yield return new WaitForSeconds(0.075f);

            if (rotateCounter >= 45)
            {
                rotationState = RotationPar.TURN;
                rotateCounter = 0;
                yield return new WaitForSeconds(1);
                Debug.Log("OK,Go");
                break;
            }
        }

        while (rotationState == RotationPar.TURN)
        {
            //transform.rotation = Quaternion.AngleAxis(2.0f, this.transform.up) * defaultRotation;
            this.transform.Rotate(0, 1.0f, 0);
            rotateCounter++;

            if (rotateCounter < 3) yield return new WaitForSeconds(0.01f);

            if (rotateCounter >= 3 && rotateCounter <= 175) yield return new WaitForSeconds(0.00075f);

            if (rotateCounter >= 175) yield return new WaitForSeconds(0.075f);

            if (rotateCounter >= 180)
            {
                Debug.Log("TURN");
                rotateCounter = 0;
                yield return new WaitForSeconds(1);
                break;
            }
        }

        Debug.Log("�I�_�Ɍ����ē����o��");
        doTurn = false;
        rotationState = RotationPar.NULL;
        yield break;
    }
}

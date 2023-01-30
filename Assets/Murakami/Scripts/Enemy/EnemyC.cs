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
    //���񋗗�
    [SerializeField] private float limitDistance;
    //���s����
    private float distance = 0.0f;
    //����ʒu[SerializeField] 
    private Transform[] turnPoint;
    //���Z�b�g���̃|�W�V����
    private Vector3 defaultPosition;
    //�\���̊Ǘ����s��
    private ResetEnemyPosition rEP;
    //Enemy�̑h������
    private SponeEnemy sE;
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
    
    private Vector3 enemyPos;

    private Transform[] turnPos;

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
    public bool DoEn
    {
        get { return this.doEncount;}
        set { this.doEncount = value;}
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pl = GameObject.Find("Player").GetComponent<PlayerC>();
        rEP = GameObject.Find("startPos").GetComponent<ResetEnemyPosition>();

        doEncount = false;
        startFlag = true;
        doTurn = false;
        noCountFlag = true;
        rotateCounter = 0;
        rotationState = RotationPar.NULL;
        rotationDistance = 0.0f;
        defaultPosition = this.transform.position;

        sE = FindObjectOfType<SponeEnemy>();

        for (int i = 0; i < 2; i++)
        {
            Debug.Log("����ʒu�̏�����");
            this.turnPoint[i] = sE.TurnPos[i];
        }
        this.transform.position = this.turnPoint[0].transform.position;

        this.transform.LookAt(turnPoint[1].transform.position);

        for (int i = 0; i < 2; i++)
        {
            turnPos[i] = turnPoint[i];
        }
    }

    // Update is called once per frameshotRayPosition.transform.position, 
    void Update()
    {
        Debug.Log("sE�̎擾�͂ł��Ă�" + sE.TurnPos[0]);
        //�����Ői�s���Player�����m����
        Vector3 rayPosition = shotRayPosition.transform.position;
        RaycastHit hit;
        Ray ray = new Ray(rayPosition, this.gameObject.transform.forward);
        Debug.DrawRay(shotRayPosition.transform.position, shotRayPosition.transform.forward * this.rayDistance, Color.red ,1.0f);
        if (Physics.Raycast(ray, out hit, 100.0f))
        {
            if(hit.collider.gameObject.CompareTag("Player"))
            {
                this.doEncount = true;
            }        
        }

        //���x�֌W
        if (this.doEncount) this.transform.position += transform.forward * addSpeed * Time.deltaTime;
        else
        {
            enemyPos = this.transform.position;

            Debug.Log("�G�ꂽ");
            if (distance >= limitDistance)
            {
                doTurn = true;
            }

            if (!doTurn)
            {
                this.transform.position += transform.forward * enemySpeed * Time.deltaTime;

            }

            else
            {
                returnLookPosition();
                this.transform.position += new Vector3(0,0,0);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        if ((collision.gameObject.CompareTag("Wall") ||
            collision.gameObject.CompareTag("WallJumpPoint") || 
            collision.gameObject.CompareTag("LimitWall") || 
            collision.gameObject.CompareTag("Ground")) && 
            doEncount == true)
        {
            Debug.Log("������");
            //StartCoroutine("ResetEnemy");
            sE.ReSponeEnemy();
            Destroy(this);
        }
    }

    /*
    private void OnTriggerEnter(Collider other)
    {      
        if ((other.gameObject == this.turnPoint[0] && noCountFlag == false) || 
            other.gameObject == this.turnPoint[1])
        {

        }
    }*/

    public void returnLookPosition()
    {
        //���̏I�_�ʒu�̕����Ɍ�����悤�ɂ��邽�߂ɒl��ύX����
        defaultRotation = this.transform.rotation;
        doTurn = true;
        rotationState = RotationPar.RIGHT;
        StartCoroutine("TurnLookPosition");
    }

    private IEnumerator ResetEnemy()
    { 
        this.transform.position = this.defaultPosition;
        doTurn = false;
        this.doEncount = false;
        rEP.StartCountDistance();
        this.gameObject.SetActive(false);
        Debug.Log("ResetEnemy��false��");
        yield break;
    }

    private IEnumerator TurnLookPosition()
    {
        yield return new WaitForSeconds(1);
        while (rotationState == RotationPar.RIGHT)
        {
            //��x����]
            this.transform.Rotate(0, 1.0f, 0);
            rotateCounter++;

            //�n�߂͏����ɂ₩�ɉ�]
            if (rotateCounter < 3) yield return new WaitForSeconds(0.1f);
            //f�ɂ̉�]���x�ŉ�]
            if (rotateCounter >= 3 && rotateCounter < 40) yield return new WaitForSeconds(0.01f);
            //�Ō�͏����ɂ₩�ɉ�]����
            if (rotateCounter >= 40) yield return new WaitForSeconds(0.075f);

            //��]�����x����45���𒴂�����
            if (rotateCounter >= 45)
            {
                rotationState = RotationPar.LEFT;
                rotateCounter = 0;
                yield return new WaitForSeconds(1);
                break;
            }
        }

        //�������ɉ�]
        while (rotationState == RotationPar.LEFT)
        {
            this.transform.Rotate(0, -1.0f, 0);
            rotateCounter++;

            if (rotateCounter < 3) yield return new WaitForSeconds(0.1f);

            if (rotateCounter >= 3 && rotateCounter < 85) yield return new WaitForSeconds(0.01f);

            if (rotateCounter >= 85) yield return new WaitForSeconds(0.075f);

            if (rotateCounter >= 90)
            {
                rotationState = RotationPar.RESET;
                rotateCounter = 0;
                yield return new WaitForSeconds(1);
                break;
            }
        }

        //���ʒu�ɖ߂�
        while (rotationState == RotationPar.RESET)
        {
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
                break;
            }
        }

        while (rotationState == RotationPar.TURN)
        {
            this.transform.Rotate(0, 1.0f, 0);
            rotateCounter++;

            if (rotateCounter < 3) yield return new WaitForSeconds(0.01f);

            if (rotateCounter >= 3 && rotateCounter <= 175) yield return new WaitForSeconds(0.00075f);

            if (rotateCounter >= 175) yield return new WaitForSeconds(0.075f);

            if (rotateCounter >= 180)
            {
                rotateCounter = 0;
                yield return new WaitForSeconds(1);
                break;
            }
        }

        doTurn = false;
        rotationState = RotationPar.NULL;

        if (noCountFlag == true)
        {
            noCountFlag = false;
        }
        yield break;
    }
}

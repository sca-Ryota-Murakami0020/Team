using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EnemyC : MonoBehaviour
{
    //�\���̊Ǘ����s��
    private ResetEnemyPosition rEP;
    //Enemy�̑h������
    private ReSponeEnemy rSE;
    //RigidBody
    private Rigidbody rb;

    //�ړ����x
    [SerializeField] private float enemySpeed = 0.7f;
    //���񋗗�
    [SerializeField] private float limitDistance;
    //�m�b�N�o�b�N���x
    private float speed = 1.5f;
    //�ǐՒ��g���X�s�[�h
    private float addSpeed = 3.0f;
    //��]��
    private int rotateCounter;
    //��]����
    private float rotateTime = 0.0f;
    //���s����
    private float distance = 0.0f;
    //��]��������
    private float rotationDistance;

    //�ǐՃt���O
    private bool doEncount;
    //����J�n�̃t���O
    private bool startFlag;
    //��]�����̔���
    private bool doTurn;
    //�ŏ��ɐU������������������ɂ���
    private bool noCountFlag;

    //�ʒu
    private Vector3 pos;
    //ray�֌W
    private float rayDistance = 2.0f;
    //ray���΂��I�u�W�F�N�g
    [SerializeField] private GameObject shotRayPosition;
    //��]�̎���ݒ肷�邽�߂ɕK�v
    private Quaternion defaultRotation;
    //���Z�b�g���̃|�W�V����
    private Vector3 defaultPosition;

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
        rEP = GameObject.Find("startPos").GetComponent<ResetEnemyPosition>();
        rSE = FindObjectOfType<ReSponeEnemy>();

        doEncount = false;
        startFlag = true;
        doTurn = false;
        noCountFlag = true;
        rotateCounter = 0;
        rotationState = RotationPar.NULL;
        rotationDistance = 0.0f;
        //defaultPosition = this.transform.position;

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
            if(hit.collider.gameObject.CompareTag("Player"))
            {
                this.doEncount = true;
            }        
        }

        //���x�֌W
        if (this.doEncount) this.transform.position += transform.forward * addSpeed * Time.deltaTime;
        else
        {
            if (!doTurn)
            {
                //this.transform.position += transform.forward * enemySpeed * Time.deltaTime;
                distance += 0.01f;
                this.transform.position = new Vector3(pos.x, pos.y, pos.z + Mathf.Sin(distance) * limitDistance * enemySpeed);
                if(rotateTime >= 1.0f)
                {
                    doTurn = true;
                    distance = 0.0f;
                    returnLookPosition();
                }
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
            rSE.SponeEnemy();
            Destroy(this);
        }
    }

    public void returnLookPosition()
    {
        //���̏I�_�ʒu�̕����Ɍ�����悤�ɂ��邽�߂ɒl��ύX����
        this.defaultRotation = this.transform.rotation;
        this.doTurn = true;
        this.doEncount = false;
        this.rotationState = RotationPar.RIGHT;
        StartCoroutine("TurnLookPosition");
    }

    private IEnumerator TurnLookPosition()
    {
        yield return new WaitForSeconds(1);

        while (rotationState == RotationPar.RIGHT)
        {
            /* 
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
            */

            rotateTime += 0.01f;
            this.transform.Rotate(new Vector3(0, Mathf.Sin(rotateTime), 0));

            if (rotateTime >= 0.25f)
            {
                rotationState = RotationPar.LEFT;
                rotateTime = 0.0f;
                yield return new WaitForSeconds(1);
                break;
            }
        }

        //�������ɉ�]
        while (rotationState == RotationPar.LEFT)
        {
            /*
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
            }*/

            rotateTime += 0.01f;
            this.transform.Rotate(new Vector3(0, Mathf.Sin(rotateTime) * -1, 0));

            if (rotateTime >= 0.5)
            {
                rotationState = RotationPar.RESET;
                rotateTime = 0.0f;
                yield return new WaitForSeconds(1);
                break;
            }
        }

        //���ʒu�ɖ߂�
        while (rotationState == RotationPar.RESET)
        {
            /*
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
            */
            rotateTime += 0.01f;
            this.transform.Rotate(new Vector3(0, Mathf.Sin(rotateTime), 0));

            if (rotateTime >= 0.25f)
            {
                rotationState = RotationPar.TURN;
                rotateTime = 0.0f;
                yield return new WaitForSeconds(1);
                break;
            }
        }

        while (rotationState == RotationPar.TURN)
        {
            /*
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
            */

            rotateTime += 0.01f;
            this.transform.Rotate(new Vector3(0, Mathf.Sin(rotateTime), 0));

            if (rotateTime >= 1.0f)
            {
                rotationState = RotationPar.TURN;
                rotateTime = 0.0f;
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

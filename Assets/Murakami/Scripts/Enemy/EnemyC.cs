using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EnemyC : MonoBehaviour
{
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
    private int rotateCounter = 0;
    //��]����
    private float rotateTime = 0.0f;
    //���s����
    private float distance = 0.0f;
    //��]��������
    private float rotationDistance = 0.0f;
    //����������
    private int invisibleTime = 0;
    //
    private Vector3 defSize;

    //�ǐՃt���O
    private bool doEncount = false;
    //��]�����̔���
    private bool doTurn = false;
    //�ŏ��ɐU������������������ɂ���
    private bool noCountFlag = true;
    //���ŏ�����
    private bool isInvisible = false;

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
    //���ŏ����̍ۂ�Enemy��߂��ʒu
    private Vector3 trueDefaultPosition;
    //�����������Enemy�̊p�x
    private Quaternion tDQ;

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

        rotationState = RotationPar.NULL;
        defaultPosition = this.transform.position;
        trueDefaultPosition = this.transform.position;
        tDQ = this.transform.rotation;
        defSize = this.transform.localScale;
    }

    // Update is called once per frameshotRayPosition.transform.position, 
    void Update()
    {
        //�v���C���[�̍��G
        if(isInvisible == false)
        {
            SearchPlayer();
        }

        //���x�֌W
        //Player�𔭌�������
        if (this.doEncount)
        {
            this.transform.position += transform.forward * addSpeed * Time.deltaTime;
        }

        //����s��
        //����ʒu�ɒ����܂ł̏���
        if (doTurn == false && isInvisible == false && this.doEncount == false)
        {
            MoveEnemy();
        }
    }

    //�ڐG����
    private void OnCollisionEnter(Collision collision)
    {
        //�ǂɂԂ�������
        if ((collision.gameObject.CompareTag("Wall") ||
            collision.gameObject.CompareTag("WallJumpPoint") || 
            collision.gameObject.CompareTag("LimitWall") || 
            collision.gameObject.CompareTag("OutSidePoint") ||
            collision.gameObject.CompareTag("Ground")) && 
            doEncount == true)
        {

            //���������J�n
            //rSE.SponeEnemy();
            //this.gameObject.SetActive(false);
            ReSetEnemy();
        }
    }

    #region//�֐��֌W

    //Ray��p�����v���C���[�̍��G
    public void SearchPlayer()
    {
        //�����Ői�s���Player�����m����
        Vector3 rayPosition = shotRayPosition.transform.position;
        RaycastHit hit;
        Ray ray = new Ray(rayPosition, this.gameObject.transform.forward);
        Debug.DrawRay(shotRayPosition.transform.position, shotRayPosition.transform.forward * this.rayDistance, Color.red, 1.0f);
        if (Physics.Raycast(ray, out hit, 100.0f))
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                this.doEncount = true;
            }
        }
    }

    //����s��
    public void MoveEnemy()
    {
        //������Enemy������s����������
        this.transform.position += transform.forward * enemySpeed * Time.deltaTime;
        Vector3 enamyPasoion = this.transform.position;
        //distance += 0.001f;
        //this.transform.position = new Vector3(0, 0, Mathf.Sin(distance) * limitDistance * enemySpeed);
        //�����_�ɂ������U��~����distance >= 0.25f
        if (enamyPasoion.z >= defaultPosition.z + limitDistance || 
            enamyPasoion.x >= defaultPosition.x + limitDistance ||
            enamyPasoion.z <= defaultPosition.z - limitDistance ||
            enamyPasoion.x <= defaultPosition.x - limitDistance)
        {
            //distance = 0.0f;
            //Debug.Log("��U��~");
            returnLookPosition();
        }
    }

    //���񏈗�
    public void returnLookPosition()
    {
        //����ʒu�����̏o���n�_�ɂ���
        defaultPosition = this.transform.position;
        //���̏I�_�ʒu�̕����Ɍ�����悤�ɂ��邽�߂ɒl��ύX����
        this.defaultRotation = this.transform.rotation;
        //���񒆂ɂ���
        this.doTurn = true;
        //���񂷂�������X�e�[�^�X�ŊǗ����Ă���̂ł����ŉ�]���������߂�
        this.rotationState = RotationPar.RIGHT;
        //Debug.Log("����J�n");
        //�e�����ɐ��񂷂�R���[�`��
        StartCoroutine("TurnLookPosition");
    }

    //
    private void ReSetEnemy()
    {
        //�ʒu�̏�����
        this.transform.position = trueDefaultPosition;
        //�����̏�����
        this.transform.rotation = tDQ;
        //�v���C���[����������������Z�b�g����
        doEncount = false;
        //������
        isInvisible = true;
        //�ŏ��Ƀu���b�N�ɐG��Ă������Ȃ��l�ɂ���
        noCountFlag = true;
        Debug.Log("�����������J�n");
        StartCoroutine("ResetEnemy");
    }
    #endregion

    #region//�R���[�`��
    //���Z�b�g�@�\
    private IEnumerator ResetEnemy()
    {
        Debug.Log("������������");
        while(invisibleTime <= 10)
        {
            this.transform.localScale = Vector3.zero;
            yield return new WaitForSeconds(1.0f);
            invisibleTime++;
        }
        Debug.Log("�����������I��");
        this.transform.localScale = defSize;
        isInvisible = false;
        invisibleTime = 0;
        yield break;
    }

    private IEnumerator TurnLookPosition()
    {
        //����O�ɏ����҂�
        yield return new WaitForSeconds(1.5f);

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
            /*
            Debug.Log("�E����");

            rotateTime += 0.001f;
            this.transform.Rotate(new Vector3(0, Mathf.Sin(rotateTime), 0));

            if (rotateTime >= 0.125f)
            {
                rotationState = RotationPar.LEFT;
                rotateTime = 0.0f;
                yield return new WaitForSeconds(1);
                break;
            }*/
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
            //Debug.Log("������");
            /*
            rotateTime += 0.001f;
            this.transform.Rotate(new Vector3(0, Mathf.Sin(rotateTime) * -1, 0));

            if (rotateTime >= 0.25)
            {
                rotationState = RotationPar.RESET;
                rotateTime = 0.0f;
                yield return new WaitForSeconds(1);
                break;
            }*/
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
            
            /*
             * Debug.Log("���ʂ�����");
            rotateTime += 0.001f;
            this.transform.Rotate(new Vector3(0, Mathf.Sin(rotateTime), 0));

            if (rotateTime >= 0.125f)
            {
                rotationState = RotationPar.TURN;
                rotateTime = 0.0f;
                yield return new WaitForSeconds(1);
                break;
            }*/
        }

        //���̏I�_�̕����ɐ���
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
            /*
            Debug.Log("�����𔽓]");
            rotateTime += 0.001f;
            this.transform.Rotate(new Vector3(0, Mathf.Sin(rotateTime), 0));

            if (rotateTime >= 0.5f)
            {
                rotationState = RotationPar.TURN;
                rotateTime = 0.0f;
                yield return new WaitForSeconds(1);
                break;
            }*/
        }

        doTurn = false;
        rotationState = RotationPar.NULL;

        if (noCountFlag == true)
        {
            noCountFlag = false;
        }
        yield break;
    }
    #endregion
}

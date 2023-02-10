using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EnemyC : MonoBehaviour
{
    //RigidBody
    private Rigidbody rb;

    #region//�X�e�[�^�X
    //�ړ����x
    private float enemySpeed = 0.7f;
    //�ړ�����
    [SerializeField] private float limitDistance;
    //�ǐՒ��g���X�s�[�h
    private float addSpeed = 3.0f;
    //��]��
    private int rotateCounter = 0;
    //���������ԁi�T�C�Y��0�ɂ��鎞�ԁj
    private int invisibleTime = 0;
    //����������Ɍ��̃T�C�Y�ɖ߂����߂Ɏg���ϐ�
    private Vector3 defSize;
    #endregion

    #region//�t���O
    //�ǐՃt���O
    private bool doEncount = false;
    //��]�����̔���
    private bool doTurn = false;
    //�ŏ��ɐU������������������ɂ���
    private bool noCountFlag = true;
    //���ŏ�����
    private bool isInvisible = false;
    #endregion

    #region//�v���C���ɕω�����Enemy�̈ʒu�E������
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
    #endregion

    //�G�̉�]�������
    enum RotationPar
    {
        NULL,
        RIGHT,
        LEFT,
        RESET,
        TURN,
    };
    
    //�p�����[�^
    RotationPar rotationState;

    #region//�v���p�e�B
    public bool DoEn
    {
        get { return this.doEncount;}
        set { this.doEncount = value;}
    }
    #endregion

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        //Enemy�̃X�e�[�^�X�֌W�̏�����
        rotationState = RotationPar.NULL;
        defaultPosition = this.transform.position;
        trueDefaultPosition = this.transform.position;
        tDQ = this.transform.rotation;
        defSize = this.transform.localScale;
    }

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

            //�G���I�u�W�F�N�g�ɓ�����������10�b��Ɍ��̏ꏊ�A���̌�����
            //�߂��Ă�����
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
        //�����_�ɂ������U��~����
        //������4����͓̂G�̓����������l�X������
        if (enamyPasoion.z >= defaultPosition.z + limitDistance || 
            enamyPasoion.x >= defaultPosition.x + limitDistance ||
            enamyPasoion.z <= defaultPosition.z - limitDistance ||
            enamyPasoion.x <= defaultPosition.x - limitDistance)
        {
            //���񏈗�
            returnLookPosition();
        }
    }

    //���񏈗�
    public void returnLookPosition()
    {
        //���ݒn�i����ʒu�j�����̏o���n�_�ɂ���
        defaultPosition = this.transform.position;
        //���̏I�_�ʒu�̕����Ɍ�����悤�ɂ��邽�߂ɒl��ύX����
        this.defaultRotation = this.transform.rotation;
        //���񒆂ɂ���
        this.doTurn = true;
        //���񂷂�������X�e�[�^�X�ŊǗ����Ă���̂ł����ŉ�]���������߂�
        this.rotationState = RotationPar.RIGHT;
        //�e�����ɐ��񂷂�R���[�`��
        StartCoroutine("TurnLookPosition");
    }

    //Enemy���������鉉�o���s��
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
        //�R���[�`���J�n
        StartCoroutine("ResetEnemy");
    }
    #endregion

    #region//�R���[�`��
    //���Z�b�g�@�\
    private IEnumerator ResetEnemy()
    {
        //�P�O�b��Enemy���k�����鏈��
        while(invisibleTime <= 10)
        {
            //Enemy�̃T�C�Y���O�ɂ���
            this.transform.localScale = Vector3.zero;
            //�P�b�ԑҋ@����
            yield return new WaitForSeconds(1.0f);
            //�R���[�`�����ł�Time�֐��͐��m�ł͖��������̂�int�^�̕ϐ���p���ĕb�����v������
            invisibleTime++;
        }
        //�P�O�b���Enemy�̃T�C�Y�����ɖ߂�
        this.transform.localScale = defSize;
        //�������̏������I�������̂Ńt���O��false�ɂ���
        isInvisible = false;
        //���̃��Z�b�g�����̂��߂ɂ����Ōv���ɗp�����ϐ�������������
        invisibleTime = 0;
        yield break;
    }

    //���񏈗�
    private IEnumerator TurnLookPosition()
    {
        //����O�ɏ����҂�
        yield return new WaitForSeconds(1.5f);

        while (rotationState == RotationPar.RIGHT)
        {
             
            //��x���E�����ɉ�]
            this.transform.Rotate(0, 1.0f, 0);
            //�X�����v�Z����
            rotateCounter++;

            //2�x�܂ł͏����ɂ₩�ɉ�]
            if (rotateCounter < 3) yield return new WaitForSeconds(0.1f);
            //3�x����39�x�܂ł͈��̉�]���x�ŉ�]
            if (rotateCounter >= 3 && rotateCounter < 40) yield return new WaitForSeconds(0.01f);
            //40�x�ȍ~�͏����ɂ₩�ɉ�]����
            if (rotateCounter >= 40) yield return new WaitForSeconds(0.075f);
            //��]�����x����45�x�𒴂�����
            if (rotateCounter >= 45)
            {
                //��]��Ԃ̃X�e�[�^�X���u�������v�ɕύX
                rotationState = RotationPar.LEFT;
                //�v�Z�����p�x�������Z�b�g
                rotateCounter = 0;
                //1.3�b�ҋ@
                yield return new WaitForSeconds(1.3f);
                break;
            }
        }

        //�������ɉ�]
        while (rotationState == RotationPar.LEFT)
        {
            //1�x���������ɉ�]����
            this.transform.Rotate(0, -1.0f, 0);
            //�X�����p�x���v�Z����
            rotateCounter++;

            //2�x�܂ł͏����ɂ₩�ɉ�]
            if (rotateCounter < 3) yield return new WaitForSeconds(0.1f);
            //3�x����85�x�܂ł͈��̉�]���x�ŉ�]����
            if (rotateCounter >= 3 && rotateCounter < 85) yield return new WaitForSeconds(0.01f);
            //85�x�ȍ~�͏����ɂ₩�ɉ�]����
            if (rotateCounter >= 85) yield return new WaitForSeconds(0.075f);
            //��]�����p�x��90�x�𒴂�����
            if (rotateCounter >= 90)
            {
                //��]��Ԃ̃X�e�[�^�X���u���Z�b�g�v�ɕύX����
                rotationState = RotationPar.RESET;
                //�v�Z�����p�x�������Z�b�g����
                rotateCounter = 0;
                //1.3�b�ҋ@
                yield return new WaitForSeconds(1.3f);
                break;
            }
        }

        //���ʒu�ɖ߂�
        while (rotationState == RotationPar.RESET)
        {
            //1�x����]����
            this.transform.Rotate(0, 1.0f, 0);
            //�X�����p�x���v�Z����
            rotateCounter++;

            //2�x�܂ł͏����ɂ₩�ɉ�]����
            if (rotateCounter < 3) yield return new WaitForSeconds(0.1f);
            //3�x����39�x�܂ł͈��̉�]���x�ŉ�]����
            if (rotateCounter >= 3 && rotateCounter < 40) yield return new WaitForSeconds(0.01f);
            //40�x�ȍ~�͏����ɂ₩�ɉ�]����
            if (rotateCounter >= 40) yield return new WaitForSeconds(0.075f);
            //��]�����p�x��45�x�𒴂�����
            if (rotateCounter >= 45)
            {
                //��]��Ԃ̃X�e�[�^�X���u�t�����Ɍ����v�ɂ���
                rotationState = RotationPar.TURN;
                //�v�Z�����p�x�������Z�b�g����
                rotateCounter = 0;
                //1.3�b�ҋ@
                yield return new WaitForSeconds(1.3f);
                break;
            }
        }

        //���̏I�_�̕����ɐ���
        while (rotationState == RotationPar.TURN)
        {
            //1�x����]����
            this.transform.Rotate(0, 1.0f, 0);
            //�X�����p�x���v�Z����
            rotateCounter++;

            //2�x�܂ł͏����ɂ₩�ɉ�]����
            if (rotateCounter < 3) yield return new WaitForSeconds(0.01f);
            //3�x����175�x�܂ł͈��̑��x�ŉ�]����
            if (rotateCounter >= 3 && rotateCounter <= 175) yield return new WaitForSeconds(0.00075f);
            //175�x�ȍ~�͏����ɂ₩�ɉ�]����
            if (rotateCounter >= 175) yield return new WaitForSeconds(0.075f);
            //��]�����p�x��180�x�𒴂�����
            if (rotateCounter >= 180)
            {
                //�v�Z�����p�x�������Z�b�g����
                rotateCounter = 0;
                //1.3�b�ҋ@
                yield return new WaitForSeconds(1.3f);
                break;
            }
        }

        //�����Ő��񏈗����I������̂�false�ɂ���
        doTurn = false;
        //��]��Ԃ̃X�e�[�^�X��������
        rotationState = RotationPar.NULL;
        //���Z�b�g�����������ۂɃu���b�N�ɐG��Ă������Ȃ��t���O��true�Ȃ�false�ɂ���
        if (noCountFlag == true)
        {
            noCountFlag = false;
        }
        yield break;
    }
    #endregion
}

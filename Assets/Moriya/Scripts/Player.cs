using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;


public class Player : MonoBehaviour
{
    #region//�v���C���[�X�e�[�^�X

    //�v���C���[�̑O��Hp��ۑ�����
    private int oldHp;
    //�ړ����x
    //���[�����O�W�����v��������x�����X�s�[�h
    private float jumpRollingSpeed = 4.0f;
    //�W�����v��������x�����X�s�[�h
    private float jumpingRunSpeed = 2.5f;
    //�ǃW�����v�������̃v���C���[�̃X�s�[�h
    private float wallJumpRunSpeed = 3.0f;
    //�W�����v��������y�����X�s�[�h
    private float jumpSpeed = 10.0f;
    //�������x
    private float runSpeed = 4.0f;
    //��������y�����X�s�[�h(�g���Ă��Ȃ�)
    private float fallSpeed = -0.1f;
    //�W�����v�����
    private int jumpCount = 0;
    //�v���C���[��ǂ�������J�����̑O�̃|�W�V�������L�^���邽�߂̕ϐ�(�r���X�e�[�W�ł̂ݎg�p����)
    private string oldName;

    //�����֌W
    //�����������ǂ����̃t���O
    private bool speedAccelerationFlag = false;
    //�J�E���g�p
    private float speedCTime = 0;
    //������������
    private float speedTime = 1000.0f;
    //��������l������ϐ�
    private float accelSpeed = 4.0f;
    //�������Z�b�g����ۂɎg���ϐ�
    private float defaultSpeed = 2.5f;

    #endregion

    //�v���C���[�A�j���[�V�����p�ϐ�
    [SerializeField]
    private Animator anime = null;

    #region//�󋵂ɉ����Ďg�p����t���O

    //���������Ƃ��̃_���[�W�����邩�ǂ����̃t���O
    private bool fallDamageHitFlag = false;
    //�����_���[�W���������t���O
    private bool fallDamageFlag = true;
    //�ړ�
    private bool moveFlag = false;
    //�W�����v
    private bool jumpFlag = false;
    //������
    private bool fallFlag = false;
    //���n��
    private bool landFlag = false;
    //���[�����O�W�����v�n�_�ɂӂꂽ�t���O
    private bool rollingJumpFlag = false;
    //���[�����O�W�����v�������t���O
    private bool rollingJumpDidFlag = false;
    //�������ɕǃW�����v�n�_�ɐG�ꂽ��
    private bool wallJumpHitFlag = false;
    //�ǃW�����p�t���O
    private bool wallJumpFlag = false;
    //�ǃW�����v�������t���O
    private bool wallJumpDidFlag = false;
    //�Q�[���I�[�o�[
    private bool gameOverFlag = false;
    //�J�����Ɉړ��E�������]���e�I�u�W�F�N�g�ɕǒ���t��
    private bool doInputButtonFlag = false;
    //�ǂɂ��锻��
    private bool doStayWall = false;

    #endregion

    //�@���C���΂��ꏊ
    [SerializeField]
    private Transform rayPosition;
    //�@�������Ƀ��C���΂�����
    [SerializeField]
    private float rayRange;
    //�@�����������C���΂�����
    [SerializeField]
    private float raySideRange;
    //�@������y���W
    private float fallenPosition;
    //�@�������Ă���n�ʂɗ�����܂ł̋���
    private float fallenDistance;
    //�@�ǂ̂��炢�̍�������_���[�W��^���邩
    [SerializeField]
    private float takeDamageDistance = 3f;

    //RigidBody�ƃ{�b�N�X�R���C�_�[�̒�`
    private Rigidbody rb;
    private BoxCollider bc;

    //GM�ƃ|�[�Y��ʊ֌W�̃X�v���N�g�̒�`
    private totalGameManager gm;
    private PasueDisplayC pasueDisplayC;
    private PlayerWallCon pWallC;

    //�@Time.timeScale�ɐݒ肷��l
    [SerializeField]
    private float timeScale = 0.1f;
    //�@���Ԃ�x�����Ă��鎞��
    private float slowTime = 2f;
    //�@�o�ߎ���
    private float elapsedTime = 0f;

    //�e�I�u�W�F�N�g
    private GameObject _parent;
    //�q�I�u�W�F�N�g
    private GameObject child;

    //�J����
    [SerializeField]
    private GameObject mainCamera;
    //�J�����̕�������
    private Vector3 mainCameraForwardDer;
    private Vector3 mainCameraRightDer;

    // �摜�`��p�̃R���|�[�l���g
    [SerializeField]
    SkinnedMeshRenderer smr;
    //�v���C���[�̏�Ԃ�F������
    STATE state;
    //�_�Ŋ��o
    [SerializeField]
    private float flashInterval;
    //�_�ł�����Ƃ��̃��[�v�J�E���g
    [SerializeField]
    private int loopCount;
    //�����������ǂ����̃t���O
    private bool isHit;

    //�v���C���[��hp��\�����邽�߂�UI�v���n�u
    [SerializeField]
    private GameObject[] heartArray = new GameObject[3];

    //���ʉ��֌W
    private AudioSource audios = null;
    [SerializeField]
    private AudioClip jumpSE;
    [SerializeField]
    private AudioClip randingSE;
    [SerializeField]
    private AudioClip damegeSE;
    [SerializeField]
    private AudioClip runSE;//�ړ��p
    [SerializeField]
    private AudioClip accelSE;//�����p
    [SerializeField]
    private AudioClip itemGetSE;
    //���ʉ����Ȃ�����
    private bool soundFlag = true;

    //�R���[�`���߂�l�p
    private Coroutine lineCast;


    //�v���C���[�̏�ԗp�񋓌^�i�m�[�}���A�_���[�W�A�Q��ށj
    enum STATE
    {
        NOMAL,
        DAMAGED,
    }

    #region//�Q�b�^�[&�Z�b�^�[
    public SkinnedMeshRenderer Smr
    {
        get { return this.smr; }
        set { this.smr = value; }
    }

    public int JumpCount
    {
        get { return this.jumpCount; }
        set { this.jumpCount = value; }
    }

    public bool RollingJumpFlag
    {
        get { return this.RollingJumpFlag; }
        set { this.RollingJumpFlag = value; }
    }

    public GameObject[] HeartArray
    {
        get { return this.heartArray; }
        set { this.heartArray = value; }
    }

    public bool DoInputButtonFlag
    {
        get { return this.doInputButtonFlag; }
        set { this.doInputButtonFlag = value; }
    }

    #endregion

    private void Awake()
    {
        audios = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //�R���|�[�l���g
        rb = GetComponent<Rigidbody>();
        bc = GetComponent<BoxCollider>();
        gm = FindObjectOfType<totalGameManager>();
        pasueDisplayC = FindObjectOfType<PasueDisplayC>();
        pWallC = FindObjectOfType<PlayerWallCon>();
        this.anime = GetComponent<Animator>();

        //hp������
        oldHp = gm.PlayerHp;

        //�e�I�u�W�F�N�g�擾
        _parent = transform.root.gameObject;

        //�q�I�u�W�F�N�g�擾
        child = transform.GetChild(2).gameObject;

        //�A�j���[�V����������
        anime.SetBool("doIdle", true);

        //���������Ɏg�����l���Z�b�g
        fallenDistance = 0f;
        fallenPosition = transform.position.y;
        fallFlag = false;

        //ray���ˊJ�n
        lineCast = StartCoroutine("StartLineCast");

        //���݂̃X�e�[�W���ʂ̔ԍ���������B
        //�[���X�e�[�W�̑��ʂ��ړ����邲�Ƃɔԍ����������Ă���four�����one�ɖ߂�
        oldName = "Start";
    }

    // Update is called once per frame
    void Update()
    {
        //�v���C���[hp�\�������邽�߂�for��
        for (int i = 0; i < gm.PlayerHp; i++)
        {
            heartArray[i].gameObject.SetActive(true);
        }

        Debug.DrawLine(rayPosition.position, rayPosition.position + Vector3.down * rayRange, Color.red, 1.0f);

        Debug.DrawLine(rayPosition.position, rayPosition.position + Vector3.forward * rayRange, Color.red, 1.0f);
        Debug.DrawLine(rayPosition.position, rayPosition.position + Vector3.left * rayRange, Color.red, 1.0f);
        Debug.DrawLine(rayPosition.position, rayPosition.position + Vector3.right * rayRange, Color.red, 1.0f);
        Debug.DrawLine(rayPosition.position, rayPosition.position + Vector3.back * rayRange, Color.red, 1.0f);

        // �X�e�[�g���_���[�W�Ȃ烊�^�[��
        if (state == STATE.DAMAGED)
        {
            return;
        }

        //�J�����̊p�x�擾�ƒP�ʃx�N�g����
        mainCameraForwardDer = mainCamera.transform.forward.normalized;
        mainCameraRightDer = mainCamera.transform.right.normalized;
        Vector3 cameraDreNoY = new Vector3(mainCameraForwardDer.x, 0, mainCameraForwardDer.z);
        cameraDreNoY = cameraDreNoY.normalized;

        //�����Ȃ�������ҋ@���[�V����
        //anime.SetBool("doIdle", true);

        //hp�����������̏���
        //hp��0�܂ł͂���
        if (gm.PlayerHp < oldHp && gm.PlayerHp >= 1)
        {
            //�A�j���[�V���������ʉ�����
            PlaySE(damegeSE);
            //Hp�\���Ɠ_�ŕ\��
            HpDisplay();
            oldHp = gm.PlayerHp;
        }

        //�Q�[���I�[�o�[�V�[���ɔ�Ԏ�
        //hp��0�ɂȂ����炱���ɓ���
        if (gm.PlayerHp == 0)
        {
            gameOverFlag = true;
            anime.SetTrigger("lose");
            if (gameOverFlag == true)
            {
                StartCoroutine(GameOver());
            }
        }

        //�m�b�N�o�b�N
        //KnockBack();

        //�@�����Ă�����
        //�������̏���(�قڃA�j���[�V����)
        FallAnime();

        //�������ɕǃW�����n�_�ɓ���������
        if (doStayWall == true)
        {
            //Debug.Log("�����ɓ�������");
            jumpCount = 0;
            rollingJumpFlag = false;
            //���N���b�N�����ꂽ��
            if (Input.GetMouseButton(1))
            {
                anime.SetTrigger("LiftWall");
                //�d�͂���p������
                rb.useGravity = true;
                //�ǃW�����v�ł���t���O������
                wallJumpFlag = false;
                //�J�����ɓn���t���O������
                doInputButtonFlag = false;
                //�ǂ͂�����̃t���O������
                doStayWall = false;
                //�ǂ��痣�ꂽ�̂Ńt���O������
                pWallC.WallJumpHitFlag = false;
            }
        }

        //�A�j���[�V�������������
        if (speedAccelerationFlag == true)
        {
            //Debug.Log("����������");
            //�������Ԍv�Z�Ƒ��x�ϊ�
            speedCTime++;
            runSpeed = accelSpeed;
            if (speedTime < speedCTime)
            {
                //Debug.Log("�����I�������");
                //�������ԁ����x���Z�b�g
                runSpeed = defaultSpeed;
                speedCTime = 0;
                speedAccelerationFlag = false;
            }

        }

        #region//�ړ����W�����v���@
        //�\���L�[����
        //���̏�����WASD�ǂ������
        //�������Ɍ����Ĉړ�������
        if (Input.GetKey(KeyCode.A) && doInputButtonFlag == false && landFlag == false)
        {
            moveFlag = true;

            //�������ł͂Ȃ���Ε������[�V���������Ă�
            if (fallFlag == false)
            {
                anime.SetBool("doIdle", false);
                anime.SetBool("doWalk", true);
            }

            //���ʂ̕����X�s�[�h
            if (jumpFlag == false && rollingJumpDidFlag == false && wallJumpDidFlag == false)
            {
                _parent.transform.position -= mainCameraRightDer * runSpeed * Time.deltaTime;
            }

            //�ʏ�W�����v�̃X�s�[�h
            if (jumpFlag == true)
            {
                _parent.transform.position -= mainCameraRightDer * jumpingRunSpeed * Time.deltaTime;
            }

            //���[�����O�W�����v�̃X�s�[�h
            if (rollingJumpDidFlag == true)
            {
                _parent.transform.position -= mainCameraRightDer * jumpRollingSpeed * Time.deltaTime;
            }

            //�ǃW�����v�̃X�s�[�h
            if (wallJumpDidFlag == true)
            {
                _parent.transform.position -= mainCameraRightDer * wallJumpRunSpeed * Time.deltaTime;
            }

            //�J�����̌����ɍ��킹�ĉ�]����
            transform.rotation = Quaternion.LookRotation(-mainCameraRightDer);
        }

        //�E�����Ɍ����Ĉړ�������
        if (Input.GetKey(KeyCode.D) && doInputButtonFlag == false && landFlag == false)
        {
            moveFlag = true;
            if (fallFlag == false)
            {
                anime.SetBool("doIdle", false);
                anime.SetBool("doWalk", true);
            }
            //���ʂ̕����X�s�[�h
            if (jumpFlag == false && rollingJumpDidFlag == false && wallJumpDidFlag == false)
            {
                _parent.transform.position += mainCameraRightDer * runSpeed * Time.deltaTime;
            }

            //�ʏ�W�����v�̃X�s�[�h
            if (jumpFlag == true)
            {
                _parent.transform.position += mainCameraRightDer * jumpingRunSpeed * Time.deltaTime;
            }

            //���[�����O�W�����v�̃X�s�[�h
            if (rollingJumpDidFlag == true)
            {
                _parent.transform.position += mainCameraRightDer * jumpRollingSpeed * Time.deltaTime;
            }

            //�ǃW�����v�̃X�s�[�h
            if (wallJumpDidFlag == true)
            {
                _parent.transform.position += mainCameraRightDer * wallJumpRunSpeed * Time.deltaTime;
            }

            //�J�����̌����ɍ��킹�ĉ�]����
            transform.rotation = Quaternion.LookRotation(mainCameraRightDer);
        }

        //������Ɍ����Ĉړ�������
        if (Input.GetKey(KeyCode.W) && doInputButtonFlag == false && landFlag == false)
        {
            moveFlag = true;
            if (fallFlag == false)
            {
                anime.SetBool("doIdle", false);
                anime.SetBool("doWalk", true);
            }

            //���ʂ̕����X�s�[�h
            if (jumpFlag == false && rollingJumpDidFlag == false && wallJumpDidFlag == false)
            {
                _parent.transform.position += cameraDreNoY * runSpeed * Time.deltaTime;

            }

            //�ʏ�W�����v�̃X�s�[�h
            if (jumpFlag == true)
            {
                _parent.transform.position += cameraDreNoY * jumpingRunSpeed * Time.deltaTime;
            }

            //���[�����O�W�����v�̃X�s�[�h
            if (rollingJumpDidFlag == true)
            {
                _parent.transform.position += cameraDreNoY * jumpRollingSpeed * Time.deltaTime;
            }

            //�ǃW�����v�̃X�s�[�h
            if (wallJumpDidFlag == true)
            {
                _parent.transform.position += cameraDreNoY * wallJumpRunSpeed * Time.deltaTime;
            }

            //�J�����̌����ɍ��킹�ĉ�]����
            transform.rotation = Quaternion.LookRotation(cameraDreNoY);
        }

        //�������Ɍ����Ĉړ�������
        if (Input.GetKey(KeyCode.S) && doInputButtonFlag == false && landFlag == false)
        {
            moveFlag = true;

            if (fallFlag == false)
            {
                anime.SetBool("doIdle", false);
                anime.SetBool("doWalk", true);
            }

            //���ʂ̕����X�s�[�h
            if (jumpFlag == false && rollingJumpDidFlag == false && wallJumpDidFlag == false)
            {
                _parent.transform.position -= cameraDreNoY * runSpeed * Time.deltaTime;
            }

            //�ʏ�W�����v�̃X�s�[�h
            if (jumpFlag == true)
            {
                _parent.transform.position -= cameraDreNoY * jumpingRunSpeed * Time.deltaTime;
            }

            //���[�����O�W�����v�̃X�s�[�h
            if (rollingJumpDidFlag == true)
            {
                _parent.transform.position -= cameraDreNoY * jumpRollingSpeed * Time.deltaTime;
            }

            //�ǃW�����v�̃X�s�[�h
            if (wallJumpDidFlag == true)
            {
                _parent.transform.position -= cameraDreNoY * wallJumpRunSpeed * Time.deltaTime;
            }

            //�J�����̌����ɍ��킹�ĉ�]����
            transform.rotation = Quaternion.LookRotation(-cameraDreNoY);
        }

        //�����L�[��������Ă��Ȃ���΂����ɓ���
        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
        {
            moveFlag = false;
            rb.angularVelocity = Vector3.zero;
            if (doStayWall == false)
            {
                if (fallFlag == false)
                {
                    //�������łȂ���Αҋ@���[�V�����ɓ���
                    anime.SetBool("doIdle", true);
                    anime.SetBool("doWalk", false);
                }
            }
        }

        //�W�����v�̓���
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount == 0 && fallFlag == false && landFlag == false)
        {
            //�W�����v�̌��ʉ��𗬂�
            PlaySE(jumpSE);

            //���[�����O�W�����v���ł��Ȃ���Ԋ��ǃW�����v���ł��Ȃ���ԂȂ�
            if (rollingJumpFlag == false && wallJumpFlag == false)
            {
                //�W�����v��
                anime.SetBool("doJump", true);
                this.rb.AddForce(new Vector3(0, jumpSpeed * 30, 0));
                jumpFlag = true;
                jumpCount++;
            }

            //���[�����O�W�����v���ł����ԂȂ�
            if (rollingJumpFlag == true)
            {
                //���[�����O�W�����v��
                rollingJumpDidFlag = true;
                anime.SetTrigger("RollingJump");
                this.rb.AddForce(new Vector3(0, jumpSpeed * 30, 0));
                jumpCount++;
                rollingJumpFlag = false;
            }

            //�ǃW�����v���ł���Ȃ�
            if (wallJumpFlag == true)
            {
                //�ǃW�����v�����t���O�ƃA�j���[�V�����֌W
                wallJumpDidFlag = true;
                anime.SetTrigger("DoWallJump");
                this.rb.AddForce(new Vector3(0, jumpSpeed * 40, 0));

                jumpCount++;

                //�d�͂���p������
                rb.useGravity = true;
                //�ǃW�����v�ł���t���O������
                wallJumpFlag = false;
                //�J�����ɓn���t���O������
                doInputButtonFlag = false;
                //�����t���O�����Ă�
                speedAccelerationFlag = true;
                //�ǂ͂�����̃t���O������
                doStayWall = false;
                //�ǂ��痣�ꂽ�̂Ńt���O������
                pWallC.WallJumpHitFlag = false;
            }

        }

        #endregion
    }

    //isTrigger�����ĂȂ����̔���
    private void OnCollisionEnter(Collision other)
    {
        //�G�ɓ���������
        if (other.gameObject.CompareTag("Enemy"))
        {
            gm.PlayerHp--;
            anime.SetTrigger("domazeed");
        }

        //�n�ʂɓ���������
        if (other.gameObject.CompareTag("Ground"))
        {
            rollingJumpFlag = false;
            pWallC.WallJumpHitFlag = false;
            jumpCount = 0;
        }
      
        //���[�����O�W�����v�|�C���g�ɓ���������
        if (other.gameObject.CompareTag("RollingJumpPoint"))
        {
            rollingJumpFlag = true;
            pWallC.WallJumpHitFlag = false;
            jumpCount = 0;
        }

        //�V�[���ړ�
        if (other.gameObject.name == "goalPoint")
        {
            gm.LoadGameClear();
            SceneManager.LoadScene("GoalScene");
        }

        //���̃X�e�[�W�ɍs�����߂�if��
        //�P�K�̃X�e�[�W�V�[���ɔ��
        if (other.gameObject.name == "LoadFirstStagePoint" && gm.PlayerIC >= 1)
        {
            SceneManager.LoadScene("LoadFirstStage");
        }

        //�Q�K�̃X�e�[�W�V�[���ɔ��
        if (other.gameObject.name == "LoadSecondPoint" && gm.PlayerIC >= 2)
        {
            SceneManager.LoadScene("LoadSecondStage");
        }

        //�R�K�̃X�e�[�W�V�[���ɔ��
        if (other.gameObject.name == "LoadTherdPoint" && gm.PlayerIC >= 3)
        {
            SceneManager.LoadScene("LoadTherdStage");
        }

        //����X�e�[�W�V�[���ɔ��
        if (other.gameObject.name == "LoadLastPoint")
        {
            SceneManager.LoadScene("LoadLastStage");
        }
    }

    //isTrigger�����Ă��鎞�̏���
    private void OnTriggerEnter(Collider other)
    {
        //�A�C�e���ɓ���������
        if (other.gameObject.CompareTag("Item"))
        {
            PlaySE(itemGetSE);
            gm.PlayerIC++;
            other.gameObject.SetActive(false);
        }
        //�J�����𓮂����|�W�V�����ɒ�������
        if(other.gameObject.CompareTag("ChangeCameraPos"))
        {
            //�����ŃI�u�W�F�N�g�̖��O���L������
            string getCameraPosName = other.gameObject.name;
            //BillCameraC�̃X�N���v�g���擾����
            BillCameraC bc  = mainCamera.gameObject.GetComponent<BillCameraC>();
            //�֐��̌Ăяo��
            bc.CameraMover(oldName,getCameraPosName);
            //�����Ŋi�[���Ă������O���擾�����I�u�W�F�N�g�̖��O�ɕύX����
            oldName = getCameraPosName;
        }
    }

    //Hp���������̏���
    private void HpDisplay()
    {
        heartArray[gm.PlayerHp].gameObject.SetActive(false);
    }

    //���ʉ��𗬂�����
    public void PlaySE(AudioClip clip)
    {
        if (audios != null)
        {
            audios.PlayOneShot(clip);
        }
        else
        {
        }
    }

    //����Ƃ��̌��ʉ��̏���
    public void ActiveWalkSE()
    {
        if (speedAccelerationFlag == true)
        {
            PlaySE(accelSE);
        }
        if (speedAccelerationFlag == false)
        {
            PlaySE(runSE);
        }

    }

    //
    public void PlayerLanding()
    {
        landFlag = false;
    }

    //�m�b�N�o�b�N
    /*private void KnockBack()
    {
        RaycastHit hit;
        if (Physics.Linecast(rayPosition.position, rayPosition.position + Vector3.forward * raySideRange, out hit, LayerMask.GetMask("Bill")))
        {
            Debug.Log("�m�b�N�o�b�N�O�C��");
            this.transform.position += new Vector3(0,0,0.2f);
        }
        if (Physics.Linecast(rayPosition.position, rayPosition.position + Vector3.back * raySideRange, out hit, LayerMask.GetMask("Bill")))
        {
            Debug.Log("�m�b�N�o�b�N���C��");
            this.transform.position += new Vector3(0, 0, -0.2f);
        }
        if (Physics.Linecast(rayPosition.position, rayPosition.position + Vector3.left * raySideRange, out hit, LayerMask.GetMask("Bill")))
        {
            Debug.Log("�m�b�N�o�b�N���C��");
            this.transform.position += new Vector3(0.2f, 0, 0);
        }
        if (Physics.Linecast(rayPosition.position, rayPosition.position + Vector3.right * raySideRange, out hit, LayerMask.GetMask("Bill")))
        {
            Debug.Log("�m�b�N�o�b�N�E�C��");
            this.transform.position += new Vector3(-0.2f, 0, 0);
        }
    }*/

    //�n�ʂɂӂꂽ���̃A�j���[�V�����֌W
    public void FallAnime()
    {
        if (fallFlag)
        {
            //�A�j���[�V�����֌W�̏�����
            ResetAnime();

            //�ǃW�����v�ł���ǂɐG�ꂽ�炱���ɓ���
            if (pWallC.WallJumpHitFlag)
            {
                ClingToWall();
            }

            //�����n�_�ƌ��ݒn�̋������v�Z�i�W�����v���ŏ�ɔ��ŗ��������ꍇ���l������ׂ̏����j
            //�����n�_ = �����n�_���v���C���[�̗����n�_�̍ő�l
            fallenPosition = Mathf.Max(fallenPosition, transform.position.y);
            RaycastHit hit;
            //���C����null = ���C���΂��󋵂Ȃ�
            if (lineCast != null)
            {
                //�@�n�ʂɃ��C���͂��Ă�����
                if (Physics.Linecast(rayPosition.position, rayPosition.position + Vector3.down * rayRange, out hit, LayerMask.GetMask("Ground")))
                {
                    //�@�����������v�Z
                    fallenDistance = fallenPosition - transform.position.y;

                    if (fallenDistance >= takeDamageDistance)
                    {

                        //�A�j���[�V�����t���O������
                        anime.SetBool("doFall", false);
                        //�X���[���[�V�����ڍs
                        StartCoroutine("StartSlowmotion",hit);

                    }
                    //�_���[�W�Ȃ��̒��n
                    if (fallenDistance <= takeDamageDistance)
                    {
                        //�ʏ풅�n���[�V����������
                        anime.SetBool("doFall", false);
                        anime.SetBool("doLanding", true);

                        //��Ɠ��������Ă���
                        if (hit.transform.gameObject.CompareTag("Ground"))
                        {
                            GroundAnime();
                        }

                        //��Ɠ���
                        if (hit.transform.gameObject.CompareTag("RollingJumpPoint"))
                        {
                            RollingPointAnime();
                        }
                    }
                    //�����Ńt���O���聕���n�̌��ʉ������Ă���
                    fallFlag = false;
                    landFlag = true;
                    PlaySE(randingSE);
                }
            }
        }
        else
        {
            //���C���΂���󋵂ɂ���Ƃ�
            //fallFlag��false��Ԃł��v���C���[���n�ʂ��痣�ꂽ����fallFlag��true�ɂ���
            //�ǂɂ͂���Ă��Ȃ������n�ʂɃ��C���͂��Ă��Ȃ���Η����n�_��ݒ�
            if (lineCast != null)
            {
                //���C���͂��Ȃ��Ȃ�
                if (!Physics.Linecast(rayPosition.position, rayPosition.position + Vector3.down * rayRange, LayerMask.GetMask("Ground")) && doStayWall == false)
                {
                    //�n�ʂ�����ł�LineCast�̐������ꂽ�Ƃ� = ������ԂƂ���
                    //���̎��ɗ�����Ԃ𔻕ʂ��邽��fallFlag��true�ɂ���
                    //�ŏ��̗����n�_��ݒ�
                    fallenPosition = transform.position.y;
                    fallenDistance = 0;
                    //�t���O�𗧂Ă�
                    fallFlag = true;
                    //Debug.Log("�n�ʂ��痣�ꂽ��");
                }
            }
        }
    }

    //�A�j���[�V�����̃��Z�b�g
    private void ResetAnime()
    {
        //���[�����O���n�Ɋւ��l�����Z�b�g
        anime.SetBool("doFall", true);
        anime.SetBool("doLandRolling", false);
        anime.SetBool("doLanding", false);

        //�W�����v�⃍�[�����O�W�����v���Ȃ��������ɂ����ɓ���
        if (rollingJumpDidFlag == false && jumpFlag == false && moveFlag == true)
        {
            anime.SetBool("doWalk", true);
            anime.SetBool("doJump", false);
        }

        //�W�����v�����炱���ɓ���
        if (jumpFlag == true)
        {
            anime.SetBool("doWalk", false);
            anime.SetBool("RollingAriIdle", false);
        }

        //���[�����O�W�����v�������ɂ����ɓ���
        if (rollingJumpDidFlag == true)
        {
            anime.SetBool("doWalk", false);
            anime.SetBool("doJump", false);
            anime.SetBool("RollingAriIdle", true);
        }

        //�ǃW�����v���Ȃ炱���ɓ���
        if (wallJumpDidFlag == true)
        {
            anime.SetBool("doWalk", false);
            anime.SetBool("doJump", false);
            anime.SetBool("RollingAriIdle", true);
        }

    }

    //�ǂ͂���̊֐�
    private void ClingToWall()
    {
        //���}�E�X�{�^����������Ă�����
        if (Input.GetMouseButton(0))
        {
            wallJumpFlag = true;
            //�v���C���[�̍��W�Œ聕�������]
            doInputButtonFlag = true;
            rb.useGravity = false;
            rb.velocity = new Vector3(0, 0f, 0);
            //�ǂɒ���t���Ă�t���O�𗧂Ă�
            doStayWall = true;
            //�A�j���[�V�����֌W
            anime.SetTrigger("WallJumpHit");
            StartCoroutine("StartRotate");
            //���ʂ̃W�����v�����Ă�����
            if (jumpFlag == true)
            {
                jumpFlag = false;
                anime.SetBool("doJump", false);
            }

            //���[�����O�W�����v�����Ă�����
            if (rollingJumpDidFlag == true)
            {
                //���[�����O�󒆑ҋ@���[�V��������ҋ@���[�V������
                anime.SetBool("RollingAriIdle", false);
                rollingJumpDidFlag = false;
            }

            //�ǃW���������Ă�����
            if (wallJumpDidFlag == true)
            {
                wallJumpDidFlag = false;
            }

            //�t���O�֌W
            PlaySE(randingSE);
            fallFlag = false;
        }
    }

    //�n�ʔ���ɐG�ꂽ���̃A�j������
    private void GroundAnime()
    {
        //���ʂ̃W�����v�����Ă�����
        if (jumpFlag == true)
        {
            jumpFlag = false;
            anime.SetBool("doJump", false);
            //���n���[�V��������ҋ@���[�V������
            if (jumpFlag == false)
            {
                anime.SetBool("doIdle", true);
            }
        }

        //���[�����O�W�����v�����Ă�����
        if (rollingJumpDidFlag == true)
        {
            //���[�����O�󒆑ҋ@���[�V��������ҋ@���[�V������
            anime.SetBool("RollingAriIdle", false);
            anime.SetBool("doIdle", true);
            rollingJumpDidFlag = false;
        }

        //�ǃW�������Ă�����
        if (wallJumpDidFlag == true)
        {
            anime.SetBool("RollingAriIdle", false);
            anime.SetBool("doIdle", true);
            wallJumpDidFlag = false;
        }
    }

    //���[�����O�W�����v�n�_�ɐG�ꂽ���̃A�j������
    private void RollingPointAnime()
    {
        //���ʂ̃W�����v�����Ă�����
        if (jumpFlag == true)
        {
            jumpFlag = false;
            anime.SetBool("doJump", false);
            //���n���[�V��������ҋ@���[�V������
            if (jumpFlag == false)
            {
                anime.SetBool("doIdle", true);
                rollingJumpFlag = true;
            }
        }
        //���[�����O�W�����v�����Ă�����
        if (rollingJumpDidFlag == true)
        {
            // //���[�����O�󒆑ҋ@���[�V��������ҋ@���[�V������
            anime.SetBool("RollingAriIdle", false);
            anime.SetBool("doIdle", true);
            rollingJumpFlag = true;
            rollingJumpDidFlag = false;
        }

        //�ǃW�������Ă�����
        if (wallJumpDidFlag == true)
        {
            anime.SetBool("RollingAriIdle", false);
            anime.SetBool("doIdle", true);
            wallJumpDidFlag = false;
        }
    }

    //�����_���[�W�����ɓ�������
    private void OnDamaged(RaycastHit hitray)
    {
        gm.PlayerHp--;
        state = STATE.DAMAGED;
        StartCoroutine(_hit());
        fallDamageHitFlag = false;
        anime.SetBool("doLanding", true);
        //landFlag = true;

        //�n�ʂɒ��n������
        if (hitray.transform.gameObject.CompareTag("Ground"))
        {
            GroundAnime();
        }

        //���[�����O�W�����v�|�C���g�ɒ��n������
        if (hitray.transform.gameObject.CompareTag("RollingJumpPoint"))
        {
            RollingPointAnime();
        }
    }

    private void NoDamaged(RaycastHit hitray)
    {
        //���[�����O���n�̃t���O�𗧂Ă�
        anime.SetBool("doLandRolling", true);
        //�n�ʂɒ��n������
        if (hitray.transform.gameObject.CompareTag("Ground"))
        {
            GroundAnime();
        }

        //���[�����O�W�����v�|�C���g�ɒ��n������
        if (hitray.transform.gameObject.CompareTag("RollingJumpPoint"))
        {
            RollingPointAnime();
        }

        //��������t���O�����Ă�
        speedAccelerationFlag = true;

    }

    #region//�R���[�`��
    //���C�𓊎˂���R���[�`��
    private IEnumerator StartLineCast()
    {
        while (true)
        {
            if (fallFlag == true)
            {
                break;
            }
            if (fallFlag == false)
            {
                yield return null;
            }
        }
    }

    //�X���[���[�V�����̌��R���[�`��
    private IEnumerator StartSlowmotion(RaycastHit _hit)
    {
        //���C�͔�΂��Ȃ�
        lineCast = null;

        //�x������
        Time.timeScale = timeScale;

        //�X���[���[�V�������Ɉ��ł��E�N���b�N�������ꂽ���ǂ����̔���
        bool isClicked = false;

        //���Ԍv���{�L�[����
        while (elapsedTime < slowTime)
        {
            //1�b���Ȃ��Ȃ�X���[���[�V�����ɂ���
            Time.timeScale = timeScale;
            //�X���[���[�V�����̐������ԗp
            elapsedTime += Time.unscaledDeltaTime;
            //�@�����ɂ��_���[�W���������鋗���𒴂���ꍇ�ɉE�N���b�N��������Ă��邩�ǂ���
            if (Input.GetMouseButton(1) == true)
            {
                isClicked = true;
            }
            //�X���[���[�V��������
            if (elapsedTime > slowTime)
            {
                //�E�N���b�N����Ă��Ȃ���΁C�_���[�W�����������
                if (isClicked == false)
                {
                    Debug.Log("������");
                    OnDamaged(_hit);
                }
                if(isClicked == true)
                {
                    Debug.Log("�_���[�W�Ȃ�");
                    NoDamaged(_hit);
                }
                //Debug.Log("�Ƃ���");
                Time.timeScale = 1f;
                elapsedTime = 0.0f;
                lineCast = StartCoroutine(StartLineCast());//���C����
                break;
            }
            yield return null;
        };

        //slowmotion�{�̂��X�g�b�v
        StopCoroutine("StartSlowmotion");
    }

    //�_���[�W�����������̓_�ŏ���
    private IEnumerator _hit()
    {
        isHit = true;
        //�_�Ń��[�v�J�n
        for (int i = 0; i < loopCount; i++)
        {
            if (isHit == false)
            {
                continue;
            }
            //flashInterval�҂��Ă���
            yield return new WaitForSeconds(flashInterval);
            //spriteRenderer���I�t
            smr.enabled = false;

            //flashInterval�҂��Ă���
            yield return new WaitForSeconds(flashInterval);
            //spriteRenderer���I��
            smr.enabled = true;
        }
        //�f�t�H���g��Ԃɂ���
        state = STATE.NOMAL;
        //�_�Ń��[�v���������瓖����t���O��false(�������ĂȂ����)
        isHit = false;
    }

    //�Q�[���I�[�o�[���̏���
    private IEnumerator GameOver()
    {
        while (true)
        {
            //�Q�[���I�[�o�[�V�[���ɂ���
            yield return new WaitForSeconds(1);
            //PlayerRisetController();
            SceneManager.LoadScene("GameOverScene");
            break;
        }
    }

    //��]����
    private IEnumerator StartRotate()
    {
        while (true)
        {
            this.transform.Rotate(0, 180.0f, 0);
            yield return null;
            break;
        }
    }

    #endregion
}
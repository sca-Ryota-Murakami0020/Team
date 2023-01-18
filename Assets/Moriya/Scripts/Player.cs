using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;


public class Player : MonoBehaviour
{
    #region//�v���C���[�X�e�[�^�X
    private int oldHp;
    //�ړ����x
    //���[�����O�W�����v��������x�����X�s�[�h
    private float jumpRollingSpeed = 5.0f;
    //�W�����v��������x�����X�s�[�h
    private float jumpingRunSpeed = 2.5f;
    //�W�����v��������y�����X�s�[�h
    private float jumpSpeed =10.0f;
    //�������x
    private float runSpeed = 5.0f;
    //��������y�����X�s�[�h(�g���Ă��Ȃ�)
    private float fallSpeed = -0.1f; 
    //�W�����v�����
    private int jumpCount = 0;
    #endregion

    //�v���C���[�A�j���[�V�����p�ϐ�
    [SerializeField]
    private Animator anime = null;

    #region//�󋵂ɉ����Ďg�p����t���O
    //���������Ƃ��̃_���[�W�����邩�ǂ����̃t���O
    private bool fallDamageHitFlag = false;
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
    //�ǂ͂���W�����v���ł���n�_�ɂӂꂽ�t���O
    private bool wallClingJumpFlag = false;
    //�ǂ͂���W�����v�������t���O
    private bool wallClingJumpDidFlag = false;
    //�Q�[���I�[�o�[
    private bool gameOverFlag = false;
    //�g��Ȃ����ǃR���[�`���p
    //private bool sameTransFlag = false;
    #endregion

    //�@���C���΂��ꏊ
    [SerializeField]
    private Transform rayPosition;
    //�@���C���΂�����
    [SerializeField]
    private float rayRange;
    //�@�������ꏊ
    private float fallenPosition;
    //�@�������Ă���n�ʂɗ�����܂ł̋���
    private float fallenDistance;
    //�@�ǂ̂��炢�̍�������_���[�W��^���邩
    [SerializeField]
    private float takeDamageDistance = 3f;

    //�����֌W
    //�����������ǂ����̃t���O
    private bool speedAccelerationFlag = false;
    //�J�E���g�p
    private float speedCTime = 0;
    //������������
    private float speedTime = 1000.0f;
    //��������l������ϐ�
    private float accelSpeed;
    //�������Z�b�g����ۂɎg���ϐ�
    private float defaultSpeed = 5.0f;

    //RigidBody�ƃ{�b�N�X�R���C�_�[�̒�`
    private Rigidbody rb;
    private BoxCollider bc;

    //GM�ƃ|�[�Y��ʊ֌W�̃X�v���N�g�̒�`
    private totalGameManager gm;
    private PasueDisplayC pasueDisplayC;
    private PlayerWallCon playerWallConC;

    //�@Time.timeScale�ɐݒ肷��l
    [SerializeField]
    private float timeScale = 0.1f;
    //�@���Ԃ�x�����Ă��鎞��
    private float slowTime = 1f;
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
        get {return this.jumpCount; }
        set {this.jumpCount = value; }
    }

    public GameObject[] HeartArray
    {
        get { return this.heartArray; }
        set { this.heartArray = value; }
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
        playerWallConC = FindObjectOfType<PlayerWallCon>();
        this.anime = GetComponent<Animator>();

        //hp������
        oldHp = gm.PlayerHp;
 
        //�e�I�u�W�F�N�g�擾
        _parent = transform.root.gameObject;

        //�q�I�u�W�F�N�g�擾
        child =transform.GetChild(2).gameObject;
        
        //�A�j���[�V����������
        anime.SetBool("doIdle",true);

        //���������Ɏg�����l���Z�b�g
        fallenDistance = 0f;
        fallenPosition = transform.position.y;
        fallFlag= false;

        //�����Ɏg�����̑��x
        accelSpeed = runSpeed * 1.5f;
    }

    // Update is called once per frame
    void Update()
    {
        //�v���C���[hp�\�������邽�߂�for��
        for (int i = 0; i < gm.PlayerHp; i++)
        {
            heartArray[i].gameObject.SetActive(true);
        }

        Debug.Log(rollingJumpFlag);

        Debug.DrawLine(rayPosition.position, rayPosition.position + Vector3.down * rayRange, Color.red, 1.0f);

        // �X�e�[�g���_���[�W�Ȃ烊�^�[��
        if (state == STATE.DAMAGED)
        {
            return;
        }

        //�J�����̊p�x�擾�ƒP�ʃx�N�g����
        mainCameraForwardDer = mainCamera.transform.forward.normalized;
        mainCameraRightDer = mainCamera.transform.right.normalized;
        Vector3 cameraDreNoY = new Vector3(mainCameraForwardDer.x,0,mainCameraForwardDer.z);
        cameraDreNoY = cameraDreNoY.normalized;

        //�����Ȃ�������ҋ@���[�V����
        //anime.SetBool("doIdle", true);

        //hp�����������̏���
        //hp��0�܂ł͂���
        if (gm.PlayerHp < oldHp && gm.PlayerHp >= 1)
        {
            //�A�j���[�V���������ʉ�����
            Debug.Log("a");
            PlaySE(damegeSE);
            //Hp�\���Ɠ_�ŕ\��
            HpDisplay();
            oldHp = gm.PlayerHp;
            state = STATE.DAMAGED;
            StartCoroutine(_hit());
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

        /*if(moveFlag == true)
        {
            if (fallFlag == false)
            {
                if (speedAccelerationFlag == false)
                {
                    if (moveSEFlag == false)
                    {
                        gm.MoveFlag = true;
                        Debug.Log("mi");
                        moveSEFlag = true;
                    }

                }
                if (speedAccelerationFlag == true)
                {
                    gm.AccelWalkFlag = true;
                }
            }
        }*/

        #region//�������
        //�@�����Ă�����
        //�X�^�[�g�ł͗�����Ԃł͂Ȃ��̂�fallFlag��false�ƂȂ��Ă���

        if (fallFlag == true)
        {
            //���[�����O���n�Ɋւ��l�����Z�b�g
            anime.SetBool("doFall", true);
            anime.SetBool("doLandRolling",false);

            //�W�����v�⃍�[�����O�W�����v���Ȃ��������ɂ����ɓ���
            if (rollingJumpDidFlag == false && jumpFlag == false && moveFlag == true)
            {
                anime.SetBool("doWalk", true);
                anime.SetBool("doJump",false);
            }

            //���[�����O�W�����v�������ɂ����ɓ���
            if(rollingJumpDidFlag == true)
            {
                anime.SetBool("doWalk",false);
                anime.SetBool("doJump",false);
                anime.SetBool("RollingAriIdle", true);
            }

            //�����n�_�ƌ��ݒn�̋������v�Z�i�W�����v���ŏ�ɔ��ŗ��������ꍇ���l������ׂ̏����j
            //�����n�_ = �����n�_���v���C���[�̗����n�_�̍ő�l
            fallenPosition = Mathf.Max(fallenPosition, transform.position.y);
            //Debug.Log("fallenPosition" + fallenPosition);

            RaycastHit hit;
            //�@�n�ʂɃ��C���͂��Ă�����
            if (Physics.Linecast(rayPosition.position, rayPosition.position + Vector3.down * rayRange, out hit))
            {
                //�@�����������v�Z
                fallenDistance = fallenPosition - transform.position.y;
                if (fallenDistance >= takeDamageDistance)
                {
                    //�X���[���[�V�����ڍs
                    StartCoroutine("StartSlowmotion");
                    //�_���[�W������t���O���o������
                    if (fallDamageHitFlag == false)
                    {
                        //���[�����O���n�̃t���O�𗧂Ă�
                        anime.SetBool("doFall", false);
                        anime.SetBool("doLandRolling", true);
                        //�n�ʂɒ��n������
                        if (hit.transform.gameObject.CompareTag("Ground"))
                        {
                            //���ʂ̃W�����v�����Ă�����
                            if(jumpFlag == true)
                            {
                                jumpFlag = false;
                                anime.SetBool("doJump", false);
                                //���n���[�V��������ҋ@���[�V������
                                if (jumpFlag == false)
                                {
                                    anime.SetBool("doIdle", true);
                                    /*Debug.Log("Landing" + anime.GetBool("doLanding"));
                                    Debug.Log("doIdle" + anime.GetBool("doIdle"));
                                    Debug.Log("doFall"+anime.GetBool("doFall"));*/
                                }
                            }
                            //���[�����O�W�����v�����Ă�����
                            if (rollingJumpDidFlag == true)
                            {
                                //���[�����O�󒆑ҋ@���[�V��������ҋ@���[�V������
                                anime.SetBool("doIdle", true);
                                anime.SetBool("RollingAriIdel", false);
                                rollingJumpDidFlag = false;
                            }
                            /*if(wallClingJumpDidFlag == true)
                            {
                                wallClingJumpDidFlag == false;
                            }*/
                        }
                        //���[�����O�W�����v�|�C���g�ɒ��n������
                        if (hit.transform.gameObject.CompareTag("RollingJumpPoint"))
                        {
                            //���ʂ̃W�����v�����Ă�����
                            if (jumpFlag == true)
                            {
                                jumpFlag = false;
                                //���n���[�V��������ҋ@���[�V������
                                if (jumpFlag == false)
                                {
                                    anime.SetBool("doIdle", true);
                                    rollingJumpFlag = true;
                                    /*Debug.Log("Landing" + anime.GetBool("doLanding"));
                                    Debug.Log("doIdle" + anime.GetBool("doIdle"));
                                    Debug.Log("doFall"+anime.GetBool("doFall"));*/
                                }
                            }
                            //���[�����O�W�����v�����Ă�����
                            if (rollingJumpDidFlag == true)
                            {
                                // //���[�����O�󒆑ҋ@���[�V��������ҋ@���[�V������
                                anime.SetBool("doIdle", true);
                                anime.SetBool("RollingAriIdel", false);
                                rollingJumpFlag = true;
                                rollingJumpDidFlag = false;
                                /*Debug.Log("Landing" + anime.GetBool("doLanding"));
                                Debug.Log("doIdle" + anime.GetBool("doIdle"));
                                Debug.Log("doFall"+anime.GetBool("doFall"));*/
                            }
                            /*if(wallClingJumpDidFlag == true)
                            {
                                wallClingJumpDidFlag = false;
                                rollingJumpFlag = true;
                            }*/
                        }
                        //��������t���O�����Ă�
                        speedAccelerationFlag = true;
                    }
                    //�_���[�W������t���O���o������
                    if (fallDamageHitFlag == true)
                    {
                        gm.PlayerHp--;
                        anime.SetBool("doFall", false);
                        fallDamageHitFlag = false;
                    }
                }
                else//�_���[�W�Ȃ��̒��n
                {
                    //�ʏ풅�n���[�V����������
                    anime.SetBool("doFall", false);
                    anime.SetBool("doLanding", true);
                   
                    //��Ɠ��������Ă���
                    if (hit.transform.gameObject.CompareTag("Ground"))
                    {
                        if (jumpFlag == true)
                        {
                            //�������[�V���������n���[�V������
                            anime.SetBool("doJump", false);
                            anime.SetBool("doIdle", false);
                            //Debug.Log("haitta");
                            jumpFlag = false;
                            //���n���[�V��������ҋ@���[�V������
                            if (jumpFlag == false)
                            {
                                anime.SetBool("doIdle", true);
                                /*Debug.Log("Landing" + anime.GetBool("doLanding"));
                                Debug.Log("doIdle" + anime.GetBool("doIdle"));
                                Debug.Log("doFall"+anime.GetBool("doFall"));*/
                            }
                        }

                        if (rollingJumpDidFlag == true)
                        {
                            Debug.Log("�n�� " + rollingJumpDidFlag);
                            anime.SetBool("RollingAriIdle", false);
                            anime.SetBool("doIdle", true);
                                /*Debug.Log("Landing" + anime.GetBool("doLanding"));
                                Debug.Log("doIdle" + anime.GetBool("doIdle"));
                                Debug.Log("doFall"+anime.GetBool("doFall"));*/
                            rollingJumpDidFlag = false;
                        }

                        /*if(wallClingJumpDidFlag == true)
                        {
                            anime.SetBool("doIdle", true);
                            rollingJumpDidFlag = false;
                        }*/
                    }
                    //��Ɠ���
                    if (hit.transform.gameObject.CompareTag("RollingJumpPoint"))
                    {
                        if (jumpFlag == true)
                        {
                            //�������[�V���������n���[�V������
                            anime.SetBool("doJump", false);
                            anime.SetBool("doIdle", false);
                            jumpFlag = false;
                            //���n���[�V��������ҋ@���[�V������
                            if (jumpFlag == false)
                            {
                                anime.SetBool("doIdle", true);
                                rollingJumpFlag = true;
                                /*Debug.Log("Landing" + anime.GetBool("doLanding"));
                                Debug.Log("doIdle" + anime.GetBool("doIdle"));
                                Debug.Log("doFall"+anime.GetBool("doFall"));*/
                            }
                        }

                        if (rollingJumpDidFlag == true)
                        {
                            Debug.Log("���[�����O�W�����v�|�C���g " + rollingJumpDidFlag);
                            anime.SetBool("RollingAriIdle", false);
                            //���[�����O�W�����v�A�j���[�V����������
                            anime.SetBool("doIdle", true);
                            rollingJumpFlag = true;
                            rollingJumpDidFlag = false;
                        }

                        /*if(wallClingJumpDidFlag == true)
                      {
                          anime.SetBool("doIdle", true);
                          rollingJumpDidFlag = false;
                          rollingJumpFlag = true;
                      }*/

                    }
                }
                //�����Ńt���O���聕���n�̌��ʉ������Ă���
               fallFlag = false;
               PlaySE(randingSE);
               Debug.Log("niya");
            }
        }
        else//�n�ʂɂ��鎞
        {
            //fallFlag��false��Ԃł��v���C���[���n�ʂ��痣�ꂽ����fallFlag��true�ɂ���
            //�n�ʂɃ��C���͂��Ă��Ȃ���Η����n�_��ݒ�
            if (!Physics.Linecast(rayPosition.position, rayPosition.position + Vector3.down * rayRange))
            {
                //�n�ʂ�����ł�LineCast�̐������ꂽ�Ƃ� = ������ԂƂ���
                //���̎��ɗ�����Ԃ𔻕ʂ��邽��fallFlag��true�ɂ���
                //�ŏ��̗����n�_��ݒ�
                fallenPosition = transform.position.y;
                fallenDistance = 0;
                //�t���O�𗧂Ă�
                fallFlag = true;
            }
        }
        #endregion

        //�A�j���[�V�������������
        if (speedAccelerationFlag == true)
        {
            //�������Ԍv�Z�Ƒ��x�ϊ�
            speedCTime++;
            runSpeed = accelSpeed;
            Debug.Log("���������ɂ͂�����");
            if(speedTime < speedCTime)
            {
                Debug.Log("���������I��");
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
         if (Input.GetKey(KeyCode.A))
         {
            moveFlag = true;

            //�������ł͂Ȃ���Ε������[�V���������Ă�
            if(fallFlag == false)
            {
                anime.SetBool("doIdle", false);
                anime.SetBool("doWalk", true);
            }

            //���ʂ̕����X�s�[�h
            if (jumpFlag == false && rollingJumpDidFlag == false)
            {
                _parent.transform.position -= mainCameraRightDer * runSpeed * Time.deltaTime;
                if(speedAccelerationFlag == true)
                {
                    PlaySE(accelSE);
                }
                if(speedAccelerationFlag == false)
                {
                    ActiveWalkSE();
                }
            }

            //�ʏ�W�����v�̃X�s�[�h
            if(jumpFlag == true)
            {
               _parent.transform.position -= mainCameraRightDer * jumpingRunSpeed * Time.deltaTime;
            }

            //���[�����O�W�����v�̃X�s�[�h
            if(rollingJumpDidFlag == true)
            {
                _parent.transform.position -= mainCameraRightDer * jumpRollingSpeed * Time.deltaTime;
            }
            transform.rotation = Quaternion.LookRotation(-mainCameraRightDer);
         }

         //�E�����Ɍ����Ĉړ�������
         if (Input.GetKey(KeyCode.D))
         {
            moveFlag = true;
            if (fallFlag == false)
            {
                anime.SetBool("doIdle", false);
                anime.SetBool("doWalk", true);
            }

            if (jumpFlag == false && rollingJumpDidFlag == false)
            {
                _parent.transform.position += mainCameraRightDer * runSpeed * Time.deltaTime;
                if (speedAccelerationFlag == true)
                {
                    PlaySE(accelSE);
                }
                if (speedAccelerationFlag == false)
                {
                    ActiveWalkSE();
                }

            }

            if(jumpFlag == true)
            {
                _parent.transform.position += mainCameraRightDer * jumpingRunSpeed * Time.deltaTime;
            }

            if (rollingJumpDidFlag == true)
            {
                _parent.transform.position += mainCameraRightDer * jumpRollingSpeed * Time.deltaTime;
            }

            transform.rotation = Quaternion.LookRotation(mainCameraRightDer);
         }

         //������Ɍ����Ĉړ�������
         if (Input.GetKey(KeyCode.W))
         {
            moveFlag = true;
            if (fallFlag == false)
            {
                anime.SetBool("doIdle", false);
                anime.SetBool("doWalk", true);
            }
            
            if (jumpFlag == false && rollingJumpDidFlag == false)
            {
                _parent.transform.position += cameraDreNoY * runSpeed * Time.deltaTime;
                if (speedAccelerationFlag == true)
                {
                    PlaySE(accelSE);
                }
                if (speedAccelerationFlag == false)
                {
                    ActiveWalkSE();
                }

            }

            if(jumpFlag == true)
            {
                 _parent.transform.position += cameraDreNoY * jumpingRunSpeed * Time.deltaTime;
            }

            if (rollingJumpDidFlag == true)
            {
                Debug.Log("oue");
                _parent.transform.position += cameraDreNoY * jumpRollingSpeed * Time.deltaTime;
            }

            transform.rotation = Quaternion.LookRotation(cameraDreNoY);
         }

         //�������Ɍ����Ĉړ�������
         if (Input.GetKey(KeyCode.S))
         {
            moveFlag = true;
            if (fallFlag == false)
            {
                anime.SetBool("doIdle", false);
                anime.SetBool("doWalk", true);
            }
            if (jumpFlag == false && rollingJumpDidFlag == false)
            {
                _parent.transform.position -= cameraDreNoY * runSpeed * Time.deltaTime;
                if (speedAccelerationFlag == true)
                {
                    PlaySE(accelSE);
                }
                if (speedAccelerationFlag == false)
                {
                    ActiveWalkSE();
                }
            }

            if(jumpFlag == true)
            {
               _parent.transform.position -= cameraDreNoY * jumpingRunSpeed * Time.deltaTime;
            }

            if (rollingJumpDidFlag == true)
            {
                _parent.transform.position -= cameraDreNoY * jumpRollingSpeed * Time.deltaTime;
            }

            transform.rotation = Quaternion.LookRotation(-cameraDreNoY);
         }

         //�����L�[��������Ă��Ȃ���΂����ɓ���
        if(!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) &&!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
        {
            moveFlag = false;
            //�������łȂ���Αҋ@���[�V�����ɓ���
            if (fallFlag == false)
            {
                //Debug.Log("��~��");
                anime.SetBool("doIdle", true);
                anime.SetBool("doWalk",false);
            }              
        }

        if (Input.GetKeyDown(KeyCode.Space)&& jumpCount == 0 && fallFlag == false)
        {
            //�W�����v�̌��ʉ��𗬂�
            PlaySE(jumpSE);

            //���[�����O�W�����v���ł��Ȃ���ԂȂ�
            if (rollingJumpFlag == false)
            {
                //�W�����v��
                anime.SetBool("doJump", true);
                this.rb.AddForce(new Vector3(0, jumpSpeed * 30, 0));
                jumpFlag = true;
                jumpCount++;
            }

            /*if (wallClingJumpFlag == true)
            {
                wallClingJumpDidFlag = true;
                jumpCount++;
                this.rb.AddForce(new Vector3(0, jumpSpeed * 30, 0));
            }*/


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
        }
        #endregion
    }

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
            //Debug.Log("���߂�");
            rollingJumpFlag = false;
            //wallClingJumpFlag = false;
            jumpCount = 0;
        }

        //���[�����O�W�����v�|�C���g�ɓ���������
        if (other.gameObject.CompareTag("RollingJumpPoint"))
        {
            rollingJumpFlag = true;
            Debug.Log(rollingJumpFlag);
            //wallClingJumpFlag = false;
            jumpCount = 0;
        }

        if (other.gameObject.CompareTag("ClingPoint"))
        {
            if (jumpFlag == true)
            {
                //�������[�V���������n���[�V������
                anime.SetBool("doJump", false);
                anime.SetBool("doLanding", true);
                anime.SetBool("doIdle", false);
                //���n���[�V��������ҋ@���[�V������
                jumpFlag = false;
                if (jumpFlag == false)
                {
                    anime.SetBool("doIdle", true);
                    /*Debug.Log("Landing" + anime.GetBool("doLanding"));
                    Debug.Log("doIdle" + anime.GetBool("doIdle"));
                    Debug.Log("doFall"+anime.GetBool("doFall"));*/
                }
            }
            //wallClingJumpFlag = true;
            //rollingJumpFlag = false;
            jumpCount = 0;
        }

        /*if (playerWallConC.ClingFlag)
        {
            if (jumpFlag == true)
            {
                //�������[�V���������n���[�V������
                jumpFlag = false;
                //anime.SetBool("doJump", false);
                anime.SetBool("doIdle", false);
                //���n���[�V��������ҋ@���[�V������
                if (jumpFlag == false)
                {
                    anime.SetBool("doIdle", true);
                    /*Debug.Log("Landing" + anime.GetBool("doLanding"));
                    Debug.Log("doIdle" + anime.GetBool("doIdle"));
                    Debug.Log("doFall"+anime.GetBool("doFall"));
                }
            }
            jumpCount = 0;
        }*/
            /*if (other.gameObject.CompareTag("Wall"))
            {
                if (jumpFlag == true) 
                {
                    jumpFlag = false;
                    anime.SetBool("doJump", false);
                    anime.SetBool("doIdle",true);
                }
                jumpCount = 0;
            }*/
    }

    private void OnTriggerEnter(Collider other)
    { 
        //�A�C�e���ɓ���������
        if (other.gameObject.CompareTag("Item"))
        {
            gm.PlayerIC++;
            other.gameObject.SetActive(false);
        }

        //�V�[���ړ�
        if(other.gameObject.name == "LoadFirstStagePoint")
        {
            SceneManager.LoadScene("LoadFirstStage");
        }

        if(other.gameObject.name == "goalPoint")
        {
            gm.LoadGameClear();
            SceneManager.LoadScene("GoalScene");
        }
    }

 
    //Hp���������̏���
    private void HpDisplay()
    {
        heartArray[gm.PlayerHp].gameObject.SetActive(false);
    }

    public void PlaySE(AudioClip clip)
    {
        if (audios != null)
        {
            audios.PlayOneShot(clip);//

        }
        else
        {
            Debug.Log("�I�[�f�B�I�\�[�X���ݒ肳��Ă��܂���");
        }
    }

    public void ActiveWalkSE()
    {
        PlaySE(runSE);
        Debug.Log("�Ƃ��Ƃ�");
    }

    #region//�R���[�`��
    //�X���[���[�V�����̌��R���[�`��
    private IEnumerator StartSlowmotion()
    {
        //slowmotion�{�̂��N��
        StartCoroutine("Slowmotion");

        //1.0�b�҂�
        yield return  new WaitForSecondsRealtime(1.0f);

        //slowmotion�{�̂��X�g�b�v
        StopCoroutine("StartSlowmotion");
        StopCoroutine("Slowmotion");
    }

    //�X���[���[�V��������R���[�`��
    private IEnumerator Slowmotion()
    {
        //�x������
        Time.timeScale = timeScale;

        //Debug.Log("slowmotion");

        //���Ԍv���{�L�[����
        while (elapsedTime < slowTime)
        {
            //1�b���Ȃ��Ȃ�X���[���[�V�����ɂ���
            Time.timeScale = timeScale;
            //�X���[���[�V�����̐������ԗp
            elapsedTime += Time.unscaledDeltaTime;
            //Debug.Log("elapsed"+elapsedTime);
            //�@�����ɂ��_���[�W���������鋗���𒴂���ꍇ����E�L�[��������Ă��Ȃ�������_���[�W��^����
            if (!Input.GetKey(KeyCode.E) && fallenDistance >= takeDamageDistance)
            {
                fallDamageHitFlag = true;
            }
            //�X���[���[�V��������
            if (elapsedTime > slowTime)
            {
                //Debug.Log("�Ƃ���");
                Time.timeScale = 1f;
                elapsedTime = 0.0f;
                StopCoroutine("Slowmotion");
                break;
            }
            yield return null;
        };
    }

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

    #endregion
}
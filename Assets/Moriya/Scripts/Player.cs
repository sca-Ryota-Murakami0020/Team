using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;


public class Player : MonoBehaviour
{
    #region//�v���C���[�X�e�[�^�X
    private int hp = 3;
    private int oldHp = 0;
    [SerializeField] private float originSpeed = 5.0f;
    [SerializeField] private float jumpSpeed =10.0f;
    private float speed;
    private float fallSpeed = -0.1f; 
    private int playerMaxhp = 3;
    private int itemPoint = 0;
    private int jumpCount = 0;
    //private bool pJumpFlag = false;
    #endregion

    //�v���C���[��Transform�̒�`
    [SerializeField]
    private Transform parentTran;

    //�v���C���[�A�j���[�V�����p�ϐ�
    [SerializeField]
    private Animator anime = null;

    #region//�󋵂ɉ����Ďg�p����t���O
    //�������ă_���[�W����ɂȂ������̃t���O
    private bool fallDamageFlag;
    //���������Ƃ��̃_���[�W���������Ƃ�
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
    //�Q�[���I�[�o�[
    private bool gameOverFlag = false;
    //�g��Ȃ����ǃR���[�`���p
    private bool sameTransFlag = false;
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
    private bool speedAccelerationFlag = false;
    private float speedCTime = 0;
    private float speedTime = 10.0f;
    private float accelSpeed;

    //�������p
    private float rSpeed = 10.0f;
    private int rMaxhp = 3;

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
    
    //�v���C���[�̃_���[�W���[�V��������
    private float damazeAnimeTime = 0;
 
    //�e�I�u�W�F�N�g
    private GameObject _parent;
    //�q�I�u�W�F�N�g
    private GameObject child;

    //�J����
    [SerializeField]
    private GameObject mainCamera;
    private Vector3 mainCameraForwardDer;
    private Vector3 mainCameraRightDer;

    // �摜�`��p�̃R���|�[�l���g
    [SerializeField]
    SkinnedMeshRenderer smr;
    STATE state;
    //�_�Ŋ��o
    [SerializeField]
    private float flashInterval;
    //�_�ł�����Ƃ��̃��[�v�J�E���g
    [SerializeField] 
    private int loopCount;
    //�����������ǂ����̃t���O
    private bool isHit;

    [SerializeField]
    private GameObject[] heartArray = new GameObject[3];

    private Vector3 playerTrans;
    Vector3 oldTrans;
    private Vector3 newTrans;


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

    public float PlayerSpeed
    {
        get { return this.speed; }
        set { this.speed = value; }
    }

    public int PlayerHp
    {
        get { return this.hp; }
        set { this.hp = value; }
    }

    public int PlayerMaxHp
    {
        get { return this.playerMaxhp; }
        set { this.playerMaxhp = value; }
    }

    public int JumpCount
    {
        get {return this.jumpCount; }
        set {this.jumpCount = value; }
    }

    public int ItemPoint
    {
        get { return this.itemPoint; }
        set { this.itemPoint = value; }
    }

    public GameObject[] HeartArray
    {
        get { return this.heartArray; }
        set { this.heartArray = value; }
    }

    //�V���O���g��
    /*private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }*/
    #endregion

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
        hp = gm.PlayerHp;
        oldHp = hp;
 
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

        //�X�s�[�h�̏�����
        speed = originSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        playerTrans.y = transform.position.y;

        //Debug.Log(rollingJumpDidFlag);
        Debug.Log("fallFlag:" + fallFlag);
        Debug.Log("HP:" + hp);
        Debug.Log("GMHP:" + gm.PlayerHp);
        //De//bug.Log("Idle" + anime.GetBool("doIdle"));
        //Debug.Log("walk" + anime.GetBool("doWalk"));
        
        gm.PlayerHp = hp;

        Debug.DrawLine(rayPosition.position, rayPosition.position + Vector3.down * rayRange, Color.red, 1.0f);

        //Debug.Log(hp);
        //Debug.Log(Time.timeScale);

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
        if (hp < oldHp && hp >= 1)
        {
            HpDisplay();
            oldHp = hp;
            state = STATE.DAMAGED;
            StartCoroutine(_hit());
        }

        //�Q�[���I�[�o�[�V�[���ɔ�Ԏ�
        if (hp == 0)
        {
            gameOverFlag = true;
            anime.SetTrigger("lose");
            if (gameOverFlag == true)
            {
                StartCoroutine(GameOver());
            }
        }

        #region//�������
        //�@�����Ă�����
        //�X�^�[�g�ł͗�����Ԃł͂Ȃ��̂�fallFlag��false�ƂȂ��Ă���

        if (fallFlag == true)
        {
            if(jumpFlag == false && moveFlag == true)
            {
                anime.SetBool("doFall", true);
                anime.SetBool("doWalk", true);
                anime.SetBool("doJump",false);
                anime.SetBool("doLandRolling", false);
            }
            //Debug.Log("if");        
            //���X�ɗ������x������������
            //transform.position -= transform.up * Time.deltaTime * fallSpeed;
            //Debug.Log("�����̈ʒu"+transform.position.y);
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
                    //�t���O���Ă�
                    fallDamageFlag = true;
                    StartCoroutine("StartSlowmotion");
                    if (fallDamageHitFlag == false)
                    {
                        anime.SetBool("doFall", false);
                        anime.SetBool("doLandRolling", true);
                        speedAccelerationFlag = true;
                    }
                    if (fallDamageHitFlag == true)
                    {
                        hp--;
                        fallDamageHitFlag = false;
                        if(jumpFlag == false || rollingJumpDidFlag == true)
                        {
                            anime.SetTrigger("domazeed");
                        }
                        anime.SetBool("doFall", false);
                    }
                }
                else
                {
                   anime.SetBool("doFall",false);
                   anime.SetBool("doLandRolling",false);
                   anime.SetBool("doLanding",true);
                   //���n�̃��[�V����������
                   //Debug.Log("���n���[�V�����̏o��");
                }
                //Debug.Log("fallFlag��false�ɕϊ�����");
                fallFlag = false;            
            }
        }
        else
        {
            //fallFlag��false��Ԃł��v���C���[���n�ʂ��痣�ꂽ����fallFlag��true�ɂ���
            //�n�ʂɃ��C���͂��Ă��Ȃ���Η����n�_��ݒ�
            if (!Physics.Linecast(rayPosition.position, rayPosition.position + Vector3.down * rayRange))
            {
              //StartCoroutine(PlayerTransform());
              //if (sameTransFlag == true)
              //{ 
                //�n�ʂ�����ł�LineCast�̐������ꂽ�Ƃ� = ������ԂƂ���
                //���̎��ɗ�����Ԃ𔻕ʂ��邽��fallFlag��true�ɂ���
                //�ŏ��̗����n�_��ݒ�
                //Debug.Log("else");
                fallenPosition = transform.position.y;
                //Debug.Log("fallenPosition" + fallenPosition);
                fallenDistance = 0;
                fallFlag = true;
                sameTransFlag = false;
              //}
            }
        }
        #endregion

        //�A�j���[�V�������������
        if (speedAccelerationFlag == true)
        {
            //StartCoroutine(StartAcceleration());
            speedCTime++;
            accelSpeed = originSpeed * 1.2f;
            speed = accelSpeed;
            Debug.Log("���������ɂ͂�����");
            if(speedTime < speedCTime)
            {
                Debug.Log("���������I��");
                speed = originSpeed;
                speedCTime = 0;
                speedAccelerationFlag = false;
            }

        }

        #region//�ړ����W�����v���@
        //�\���L�[����
        //�������Ɍ����Ĉړ�������
         if (Input.GetKey(KeyCode.A))
         {
            moveFlag = true;
            if(fallFlag == false)
            {
                anime.SetBool("doIdle", false);
                anime.SetBool("doWalk", true);
            }

            if (jumpFlag == false)
            {
              _parent.transform.position -= mainCameraRightDer * speed * Time.deltaTime;
            }

            if(jumpFlag == true)
            {
               _parent.transform.position -= mainCameraRightDer * (speed) * Time.deltaTime;
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

            if (jumpFlag == false)
            {
                _parent.transform.position += mainCameraRightDer * speed * Time.deltaTime;
            }

            if(jumpFlag == true)
            {
                _parent.transform.position += mainCameraRightDer * (speed) * Time.deltaTime;
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
            
            if (jumpFlag == false)
            {
                 _parent.transform.position += cameraDreNoY * speed * Time.deltaTime;
            }

            if(jumpFlag == true)
            {
                 _parent.transform.position += cameraDreNoY * (speed) * Time.deltaTime;
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
            if (jumpFlag == false)
            {
               _parent.transform.position -= cameraDreNoY * speed * Time.deltaTime;
            }

            if(jumpFlag == true)
            {
               _parent.transform.position -= cameraDreNoY * (speed) * Time.deltaTime;
            }
            transform.rotation = Quaternion.LookRotation(-cameraDreNoY);
         }

        if(!Input.anyKey)
        {
            moveFlag = false;
            //Debug.Log("��~��");
            //Debug.Log("fallFlag:" + fallFlag);
            if(fallFlag == false)
            {
                //Debug.Log("��~��");
                anime.SetBool("doIdle", true);
                anime.SetBool("doWalk",false);
            }              
        }


        if(Input.GetKeyDown(KeyCode.Space)&& jumpCount == 0 &&jumpFlag ==false)//&& anime.SetBool(doFall.true)&&anime.SetBool(doLanging.true)
        {
            if(wallClingJumpFlag == true)
            {

            }

            if(rollingJumpFlag == true)
            {
                //���[�����O�W�����v��
                //Debug.Log("mi");
                rollingJumpDidFlag = true;
                anime.SetTrigger("RollingJump");
                this.rb.AddForce(new Vector3(0, jumpSpeed * 30, 0));
                jumpFlag = true;
                jumpCount++;
                anime.SetBool("doLanding", false);              
            }

            if(rollingJumpFlag == false && wallClingJumpFlag == false)
            {
                //�W�����v��
                anime.SetBool("doJump", true);
                this.rb.AddForce(new Vector3(0, jumpSpeed * 30, 0));
                jumpFlag = true;
                jumpCount++;
                anime.SetBool("doLanding", false);
            }
        }
        #endregion
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            hp--;
            HpDisplay();
            oldHp = hp;
            StartCoroutine(DamageTime());
        }

        if (other.gameObject.CompareTag("Ground"))
        {
            //Debug.Log("���߂�");

            if (rollingJumpDidFlag == true && jumpFlag == true)
            {
                //Debug.Log("asuta");
                jumpFlag = false;
                rollingJumpDidFlag = false;
                //anime.SetBool("RollingAriIdle",false);
                //���[�����O�W�����v�A�j���[�V����������
                //;
                anime.SetBool("doLanding", true);
                anime.SetBool("doIdle", false);
                //���n���[�V��������ҋ@���[�V������
                if (jumpFlag == false)
                {
                    //Debug.Log("��");
                    anime.SetBool("doLanding", false);
                    anime.SetBool("doIdle", true);
                    /*Debug.Log("Landing" + anime.GetBool("doLanding"));
                    Debug.Log("doIdle" + anime.GetBool("doIdle"));
                    Debug.Log("doFall"+anime.GetBool("doFall"));*/
                }
            }

            if (jumpFlag == true)
            {
                    //�������[�V���������n���[�V������
                    jumpFlag = false;
                    anime.SetBool("doJump", false);
                    anime.SetBool("doLanding", true);
                    anime.SetBool("doIdle", false);
                    //���n���[�V��������ҋ@���[�V������
                    if (jumpFlag == false)
                    {
                        //Debug.Log("��");
                        anime.SetBool("doLanding", false);
                        anime.SetBool("doIdle", true);
                        /*Debug.Log("Landing" + anime.GetBool("doLanding"));
                        Debug.Log("doIdle" + anime.GetBool("doIdle"));
                        Debug.Log("doFall"+anime.GetBool("doFall"));*/
                    }
            }

            if(jumpFlag == false && fallFlag == true)
            {
                anime.SetBool("doLandRolling",false);
            }

            jumpCount = 0;

        }

        if (other.gameObject.CompareTag("RollingJumpPoint"))
        {
            //Debug.Log("��������");
            if (jumpFlag == true)
            {
                //�������[�V���������n���[�V������
                jumpFlag = false;
                anime.SetBool("doJump", false);
                //anime.SetBool("doFall", false);
                anime.SetBool("doLanding", true);
                anime.SetBool("doIdle", false);
                rollingJumpDidFlag = false;
                anime.SetBool("RollingAriIdle",false);
                //Debug.Log("haitta");
                //���n���[�V��������ҋ@���[�V������
                if (jumpFlag == false)
                {
                    anime.SetBool("doLanding", false);
                    anime.SetBool("doIdle", true);
                    rollingJumpFlag = true;
                    /*Debug.Log("Landing" + anime.GetBool("doLanding"));
                    Debug.Log("doIdle" + anime.GetBool("doIdle"));
                    Debug.Log("doFall"+anime.GetBool("doFall"));*/
                }
            }

            if(jumpFlag == false)
            {
                rollingJumpFlag = true;
            }
            jumpCount = 0;
        }

        if (other.gameObject.CompareTag("ClingJudgement"))
        {
            if (jumpFlag == true)
            {
                //�������[�V���������n���[�V������
                jumpFlag = false;
                anime.SetBool("doJump", false);
                //anime.SetBool("doFall", false);
                anime.SetBool("doLanding", true);
                anime.SetBool("doIdle", false);
                //���n���[�V��������ҋ@���[�V������
                if (jumpFlag == false)
                {
                    anime.SetBool("doLanding", false);
                    anime.SetBool("doIdle", true);
                    rollingJumpFlag = true;
                    /*Debug.Log("Landing" + anime.GetBool("doLanding"));
                    Debug.Log("doIdle" + anime.GetBool("doIdle"));
                    Debug.Log("doFall"+anime.GetBool("doFall"));*/
                }
            }

            if (jumpFlag == false)
            {
                wallClingJumpFlag = true;
            }
            jumpCount = 0;
        }

        /*if (playerWallConC.ClingFlag)
        {
            if (jumpFlag == true)
            {
                //�������[�V���������n���[�V������
                jumpFlag = false;
                //anime.SetBool("doJump", false);
                //anime.SetBool("doFall", false);
                anime.SetBool("doIdle", false);
                //���n���[�V��������ҋ@���[�V������
                if (jumpFlag == false)
                {
                    anime.SetBool("doLanding", false);
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

    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.CompareTag("RollingJumpPoint"))
        {
            rollingJumpFlag = false;
        }


        if (collision.gameObject.CompareTag("ClingJudgement"))
        {
            wallClingJumpFlag = false; 
        }
    }

    private void OnTriggerEnter(Collider other)
    { 
        //�A�C�e���ɓ���������
        if (other.gameObject.CompareTag("Item"))
        {
            gm.PlayerIC++;
            other.gameObject.SetActive(false);
        }
    }

    //���񂾂Ƃ��Ƀ��Z�b�g����l
    /*
    private void PlayerRisetController()
    {
        playerMaxhp = rMaxhp;
        hp = playerMaxhp;
        speed = rSpeed;
        jumpCount = 0;
        itemPoint = 0;
        for(int i = 0; i <hp; i++)
        {
            heartArray[i].gameObject.SetActive(true);
        }
    }*/

    //Hp���������̏���
    private void HpDisplay()
    {
        heartArray[hp].gameObject.SetActive(false);
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
            if (!Input.GetKey(KeyCode.E) && fallenDistance >= takeDamageDistance && fallDamageFlag == true)
            {
                fallDamageFlag = false;
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
            yield return new WaitForSeconds(1);
            //PlayerRisetController();
            //SceneManager.LoadScene("GameOverScene");
            break;
        }
    }

    /*private IEnumerator PlayerTransform()
    {
        while (true)
        {
            oldTrans = new Vector3(0, playerTrans.y, 0);
            yield return new WaitForSeconds(1.5F);
            newTrans = new Vector3(0, transform.position.y, 0);
            if(oldTrans != newTrans)
            {
                sameTransFlag = true;
            }
            if(oldTrans == newTrans)
            {
                sameTransFlag = false;
            }
            break;
        }
    }*/

    private IEnumerator DamageTime()
    {
        while (true)
        {
            anime.SetBool("doDamaze",true);
            yield return new WaitForSeconds(1.0f);
            anime.SetBool("doDamaze",false);
            anime.SetBool("doIdle",true);
            break;
        }      
    }

    /*private IEnumerator StartAcceleration()
    {

    }*/
    #endregion
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Player : MonoBehaviour
{
    //�v���C���[�X�e�[�^�X
    private int hp = 3;
    private float speed = 10.0f;
    private float jumpSpeed =10.0f;
    private float fallSpeed = -0.1f; 
    private int playerMaxhp = 3;
    private int itemPoint = 0;
    private int jumpCount = 0;
    //private bool pJumpFlag = false;

    //�v���C���[��Transform�̒�`
    [SerializeField]
    private Transform parentTran;

    //�v���C���[�A�j���[�V�����p�ϐ�
    [SerializeField]
    private Animator anime = null;

    //�_���[�W
    private bool damageFlag = false;
    //�ړ�
    private bool moveFlag = false;
    //�W�����v
    private bool jumpFlag = false;
    //������
    private bool fallFlag = false;
    //���n��
    private bool landFlag = false;

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

    //���C���[�֌W
    //[Header("���C���[")] [SerializeField] private GameObject wire;
    //private bool wireItemFlag = false;
    //[SerializeField]
    //���C���[�������ɕ\������UI�̃v���n�u
    //private GameObject wireUIPrefab;
    //���C���[UI�̃C���X�^���X
    //private GameObject wireUIInstance;

    //�������p
    private float rSpeed = 10.0f;
    private int rMaxhp = 3;

    //RigidBody�ƃ{�b�N�X�R���C�_�[�̒�`
    private Rigidbody rb;
    private BoxCollider bc;

    //GM�ƃ|�[�Y��ʊ֌W�̃X�v���N�g�̒�`
    // private GManager gm;
    private PasueDisplayC pasueDisplayC;

    //�v���C���[�p�x�v�Z
    private Quaternion left;
    private Quaternion up;
    private Quaternion right;
    private Quaternion down;


    //�@Time.timeScale�ɐݒ肷��l
    [SerializeField]
    private float timeScale = 0.1f;
    //�@���Ԃ�x�����Ă��鎞��
    private float slowTime = 1f;
    //�@�o�ߎ���
    private float elapsedTime = 0f;
    private bool fallDamegeFlag;
 
    //�e�I�u�W�F�N�g
    private GameObject _parent;

    //�J����
    [SerializeField]
    private GameObject mainCamera;
    private Quaternion mainCameraQuat;
    private Vector3 mainCameraForwardDer;
    private Vector3 mainCameraRightDer;

    //�Q�b�^�[&�Z�b�^�[
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

    /*public bool WireItemFlag
    {
        get { return this.wireItemFlag; }
        set { this.wireItemFlag = value; }
    }*/

    public int ItemPoint
    {
        get { return this.itemPoint; }
        set { this.itemPoint = value; }
    }


    //�V���O���g��
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }


    // Start is called before the first frame update
    void Start()
    {

        rb = GetComponent<Rigidbody>();
        bc = GetComponent<BoxCollider>();
        //gm = FindObjectOfType<GManager>();
        pasueDisplayC = FindObjectOfType<PasueDisplayC>();
        hp = playerMaxhp;
        this.anime= GetComponent<Animator>();

        //�e�I�u�W�F�N�g�擾
        _parent = transform.root.gameObject;

        anime.SetBool("doIdle",true);

        //���������Ɏg�����l���Z�b�g
        fallenDistance = 0f;
        fallenPosition = transform.position.y;
        fallFlag= false;

        mainCameraQuat = mainCamera.transform.rotation;
        
 
    }

    // Update is called once per frame
    void Update()
    {
        mainCameraForwardDer = mainCamera.transform.forward.normalized;
        mainCameraRightDer = mainCamera.transform.right.normalized;
        Vector3 cameraDreNoY = new Vector3(mainCameraForwardDer.x,0,mainCameraForwardDer.z);
        cameraDreNoY = cameraDreNoY.normalized;
        Vector3 cameraUpLeftDiaDer = new Vector3(mainCameraRightDer.x, 0, mainCameraForwardDer.z);



        Debug.Log(hp);
        Debug.Log(Time.timeScale);
        anime.SetBool("doIdle", true);

        //�@�����Ă�����
        if (fallFlag)
        {
            //���X�ɗ������x������������
            transform.position += transform.up * Time.deltaTime * fallSpeed;
            //�@�����n�_�ƌ��ݒn�̋������v�Z�i�W�����v���ŏ�ɔ��ŗ��������ꍇ���l������ׂ̏����j
            //�����n�_ = �����n�_���v���C���[�̗����n�_�̍ő�l
            fallenPosition = Mathf.Max(fallenPosition, transform.position.y);
            //Debug.Log(fallenPosition);

            //�@�n�ʂɃ��C���͂��Ă�����
            if (Physics.Linecast(rayPosition.position, rayPosition.position + Vector3.down * rayRange, LayerMask.GetMask("Ground")))
            {
                //�@�����������v�Z
                fallenDistance = fallenPosition - transform.position.y;
                if(fallenDistance >= takeDamageDistance)
                {
                    StartCoroutine("StartSlowmotion");
                    //�t���O���Ă�
                    fallDamegeFlag = true;
                    /*while (elapsedTime < slowTime)
                    {
                        //1�b���Ȃ��Ȃ�X���[���[�V�����ɂ���
                        Time.timeScale = timeScale;
                        //�X���[���[�V�����̐������ԗp
                        elapsedTime += Time.unscaledDeltaTime;
                        Debug.Log(elapsedTime);
                        //�@�����ɂ��_���[�W���������鋗���𒴂���ꍇ����E�L�[��������Ă��Ȃ�������_���[�W��^����
                        if (!Input.GetKey(KeyCode.E) && fallenDistance >= takeDamageDistance && fallDamegeFlag == true)
                        {
                            hp--;
                            fallDamegeFlag = false;
                        }
                        //�X���[���[�V��������
                        if (elapsedTime > slowTime)
                        {
                            Debug.Log("�Ƃ���");
                            Time.timeScale = 1f;
                            elapsedTime = 0.0f;
                            break;
                        }
                    }*/
                }
                if(damageFlag == true)
                {
                    hp--;
                    damageFlag = false;
                }
                fallFlag = false;
            }
        }
        else
        {
            //�@�n�ʂɃ��C���͂��Ă��Ȃ���Η����n�_��ݒ�
            if (!Physics.Linecast(rayPosition.position, rayPosition.position + Vector3.down * rayRange, LayerMask.GetMask("Ground")))
            {
                //�@�ŏ��̗����n�_��ݒ�
                fallenPosition = transform.position.y;
                fallenDistance = 0;
                fallFlag = true;
            }
        }

        if (speedAccelerationFlag == true)
        {
            speedCTime++;
            speed +=5.0f;
            if(speedTime < speedCTime)
            {
                speed-=5.0f;
                speedCTime = 0;
                speedAccelerationFlag = false;
            }

        }

        /*if(wireItemFlag == true)
        {
                GameObject Gun_obj = (GameObject)Instantiate(wire, transform.position, Quaternion.Euler(0.0f, 0.0f, 0.0f));
                Gun_obj.transform.SetParent(parentTran);
                WireGun Gun_cs = Gun_obj.GetComponent<WireGun>();
        }*/

        if (hp <= 0)
        {
            //SceneManager.LoadScene("OneStage");
            PlayerRisetController();
        }

        #region//�ړ����@

        //�\���L�[����
        //�������Ɍ����Ĉړ�������
         if (Input.GetKey(KeyCode.A))
         {
                //�������̉摜�ɕύX����
                //playerDirection = PlayerDirection.LEFT;
                //sr.sprite = leftImage;
                moveFlag = true;
                anime.SetBool("doWalk", true);

                if (jumpFlag == false)
                {
                    _parent.transform.position -= mainCameraRightDer * speed * Time.deltaTime;
                }
                if(jumpFlag == true)
                {
                    _parent.transform.position -= mainCameraRightDer * (speed/10) * Time.deltaTime;
                }

                //transform.rotation = left;
         }

         //�E�����Ɍ����Ĉړ�������
         else if (Input.GetKey(KeyCode.D))
         {
                //�E�����̉摜�ɕύX����
                //playerDirection = PlayerDirection.RIGHT;
                //sr.sprite = rightImage;
                moveFlag = true;
                anime.SetBool("doWalk", true);
                if (jumpFlag == false)
                {
                    _parent.transform.position += mainCameraRightDer * speed * Time.deltaTime;
                }
                if(jumpFlag == true)
                {
                    _parent.transform.position += mainCameraRightDer * (speed / 10) * Time.deltaTime;
                }
         }

         //������Ɍ����Ĉړ�������
         else if (Input.GetKey(KeyCode.W))
         //anime.SetBool(doLanging.true)
         {
                 //������̉摜�ɕύX����
                 //playerDirection = PlayerDirection.DOWN;
                 //sr.sprite = defaultImage;
                 moveFlag = true;
                 anime.SetBool("doWalk", true);
                 if(jumpFlag == false)
                 {
                     _parent.transform.position += mainCameraForwardDer * speed * Time.deltaTime;
                 }

                 if(jumpFlag == true)
                 {
                     _parent.transform.position += mainCameraForwardDer * (speed/10) * Time.deltaTime;
                 }
 
         }
         //�������Ɍ����Ĉړ�������
         else if (Input.GetKey(KeyCode.S))
         {
                moveFlag = true;
                anime.SetBool("doWalk", true);
                if(jumpFlag == false)
                {
                    _parent.transform.position -= cameraDreNoY * speed * Time.deltaTime;
                }

                if(jumpFlag == true)
                {
                    _parent.transform.position -= cameraDreNoY * (speed/10) * Time.deltaTime;
                }

         }


        else if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.W))
        {

            moveFlag = true;
            anime.SetBool("doWalk", true);
            if (jumpFlag == false)
            {
                _parent.transform.position -= cameraUpLeftDiaDer * speed * Time.deltaTime;
            }

            if (jumpFlag == true)
            {
                _parent.transform.position -= cameraUpLeftDiaDer * (speed / 10) * Time.deltaTime;
            }
            
        }

        else if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.S))
        {

        }

        else if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.W))
        {

        }


        else if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.S))
        {

        }


        else
        {
                moveFlag = false;
                anime.SetBool("doIdle", true);
                anime.SetBool("doWalk",false);
        }


        if(Input.GetKeyDown(KeyCode.Space)&& jumpCount == 0 &&jumpFlag ==false)//&& anime.SetBool(doFall.true)&&anime.SetBool(doLanging.true)
        {
            //�W�����v��
            anime.SetBool("doJump", true);
            this.rb.AddForce(new Vector3(0, jumpSpeed * 30, 0));
            jumpFlag = true;
            jumpCount++;

            //�W�����v���痎�����[�V������
            //if(anime.GetCurrentAnimatorStateInfo().normalizedTime)
            //fallFlag = true;
            anime.SetBool("doLanding",false);
            //anime.SetBool("doJump",false);
            anime.SetBool("doFall",true);
           /* Debug.Log("Landing" + anime.GetBool("doLanding"));
            Debug.Log("doJump" + anime.GetBool("doJump"));
            Debug.Log("doIdle" + anime.GetBool("doIdle"));
            Debug.Log("doFall" + anime.GetBool("doFall"));*/
        }
           
        #endregion


    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            
            var con = other.GetContact(0);

            if (con.normal.x < 0.0f)
            {
                landFlag = true;
                //��p�̒��n���[�V����
                jumpFlag = false;
                jumpCount = 0;

            }

            if (con.normal.z < 0.0f)
            {
                landFlag = true;
                jumpFlag = false;
                jumpCount = 0;
            }


            if (con.normal.y > 0.0f)
            {
                if (jumpFlag == true)
                {
                    //�������[�V���������n���[�V������
                    jumpFlag = false;
                    anime.SetBool("doJump", false);
                    anime.SetBool("doFall", false);
                    anime.SetBool("doLanding", true);
                    anime.SetBool("doIdle", false);
                    landFlag = true;
                    //���n���[�V��������ҋ@���[�V������
                    if (jumpFlag == false)
                    {
                        anime.SetBool("doLanding", false);
                        anime.SetBool("doIdle", true);
                        /*Debug.Log("Landing" + anime.GetBool("doLanding"));
                        Debug.Log("doIdle" + anime.GetBool("doIdle"));
                        Debug.Log("doFall"+anime.GetBool("doFall"));*/
                    }
                }
                jumpCount = 0;
            }
          
        }
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
        /*if (other.gameObject.CompareTag("Item"))
        {
            ItemPoint++;
            other.gameObject.SetActive(false);
        }*/

        /*if (other.gameObject.CompareTag(""))
        {
            //SceneManager.LoadScene("IndoorScene");
        }*/

        /*if (other.gameObject.CompareTag("WireItem"))
        {
            wireItemFlag = true;
        }*/

        //�v���C���[���񕜃A�C�e���ɐG�ꂽ��
      /*  if (other.gameObject.CompareTag("HpItem"))
        {
            int itemHp = 10;
            //hp+�A�C�e����������̉񕜗ʂ�Maxhp��葽��������񕜗ʂ����炷
            if (hp + itemHp >= playerMaxhp)
            {
                itemHp = playerMaxhp - hp;
                hp += itemHp;
            }
            else
            {
                hp += itemHp;
            }
           // gm.HpRecoveryFlag = true;
            other.gameObject.SetActive(false);
        }*/
    }

    private void PlayerRisetController()
    {
        playerMaxhp = rMaxhp;
        hp = playerMaxhp;
        speed = rSpeed;
        jumpCount = 0;
    }

    /*private void Fallsituation()
    {
        Debug.Log("a");
        jumpFlag = false;
        fallFlag = true;
        anime.SetBool("doLanding", false);
        anime.SetBool("doJump", false);
        anime.SetBool("doFall", true);
        Debug.Log("Landing" + anime.GetBool("doLanding"));
        Debug.Log("doIdle" + anime.GetBool("doIdle"));
        Debug.Log("doFall" + anime.GetBool("doFall"));
    }*/

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
            if (!Input.GetKey(KeyCode.E) && fallenDistance >= takeDamageDistance && fallDamegeFlag == true)
            {
                fallDamegeFlag = false;
                damageFlag = true;
            }
            //�X���[���[�V��������
            if (elapsedTime > slowTime)
            {
                Debug.Log("�Ƃ���");
                Time.timeScale = 1f;
                elapsedTime = 0.0f;
                StopCoroutine("Slowmotion");
                break;
            }

            yield return null;
        };
    }
}

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
    //���[�����O��
    private bool RollingJumpFlag = false;

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

    //�������p
    private float rSpeed = 10.0f;
    private int rMaxhp = 3;

    //RigidBody�ƃ{�b�N�X�R���C�_�[�̒�`
    private Rigidbody rb;
    private BoxCollider bc;

    //GM�ƃ|�[�Y��ʊ֌W�̃X�v���N�g�̒�`
    // private GManager gm;
    private PasueDisplayC pasueDisplayC;
    private PlayerWallCon playerWallConC;


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
    //�q�I�u�W�F�N�g
    private GameObject child;



 
    //�J����
    [SerializeField]
    private GameObject mainCamera;
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
    /*private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }*/


    // Start is called before the first frame update
    void Start()
    {

        //�R���|�[�l���g
        rb = GetComponent<Rigidbody>();
        bc = GetComponent<BoxCollider>();
        //gm = FindObjectOfType<GManager>();
        pasueDisplayC = FindObjectOfType<PasueDisplayC>();
        playerWallConC = FindObjectOfType<PlayerWallCon>();
        this.anime = GetComponent<Animator>();

        //hp������
        hp = playerMaxhp;
 
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
    }

    // Update is called once per frame
    void Update()
    {

        //Debug.Log(hp);
        //Debug.Log(Time.timeScale);

        //�J�����̊p�x�擾�ƒP�ʃx�N�g����
        mainCameraForwardDer = mainCamera.transform.forward.normalized;
        mainCameraRightDer = mainCamera.transform.right.normalized;
        Vector3 cameraDreNoY = new Vector3(mainCameraForwardDer.x,0,mainCameraForwardDer.z);
        cameraDreNoY = cameraDreNoY.normalized;

        //�����Ȃ�������ҋ@���[�V����
        //anime.SetBool("doIdle", true);

        #region//�������
        //�@�����Ă�����
        if (fallFlag)
        {
            //���X�ɗ������x������������
            transform.position += transform.up * Time.deltaTime * fallSpeed;
            //�@�����n�_�ƌ��ݒn�̋������v�Z�i�W�����v���ŏ�ɔ��ŗ��������ꍇ���l������ׂ̏����j
            //�����n�_ = �����n�_���v���C���[�̗����n�_�̍ő�l
            fallenPosition = Mathf.Max(fallenPosition, transform.position.y);

            //�@�n�ʂɃ��C���͂��Ă�����
            if (Physics.Linecast(rayPosition.position, rayPosition.position + Vector3.down * rayRange, LayerMask.GetMask("Ground")))
            {
                //�@�����������v�Z
                fallenDistance = fallenPosition - transform.position.y;
                if(fallenDistance >= takeDamageDistance)
                {
                    //�t���O���Ă�
                    fallDamegeFlag = true;
                    //
                    StartCoroutine("StartSlowmotion");
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
        #endregion

        //�A�j���[�V�������������
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

        //�Q�[���I�[�o�[�V�[���ɔ�Ԏ�
        if (hp <= 0)
        {
            //SceneManager.LoadScene("GameOverScene");
            PlayerRisetController();
        }

        #region//�ړ����W�����v���@
        //�\���L�[����
        //�������Ɍ����Ĉړ�������
         if (Input.GetKey(KeyCode.A))
         {
            moveFlag = true;
            anime.SetBool("doIdle", false);
            anime.SetBool("doWalk", true);
                if (jumpFlag == false)
                {
                    _parent.transform.position -= mainCameraRightDer * speed * Time.deltaTime;
                }
                if(jumpFlag == true)
                {
                    _parent.transform.position -= mainCameraRightDer * (speed/10) * Time.deltaTime;
                }

            transform.rotation = Quaternion.LookRotation(-mainCameraRightDer);
         }

         //�E�����Ɍ����Ĉړ�������
         if (Input.GetKey(KeyCode.D))
         {
            moveFlag = true;
            anime.SetBool("doIdle", false);
            anime.SetBool("doWalk", true);
                if (jumpFlag == false)
                {
                    _parent.transform.position += mainCameraRightDer * speed * Time.deltaTime;
                }
                if(jumpFlag == true)
                {
                    _parent.transform.position += mainCameraRightDer * (speed / 10) * Time.deltaTime;
                }
            transform.rotation = Quaternion.LookRotation(mainCameraRightDer);
         }

         //������Ɍ����Ĉړ�������
         if (Input.GetKey(KeyCode.W))
         {
                 moveFlag = true;
                 anime.SetBool("doWalk", true);
                 anime.SetBool("doIdle", false);
                if (jumpFlag == false)
                {
                     _parent.transform.position += mainCameraForwardDer * speed * Time.deltaTime;
                }

                 if(jumpFlag == true)
                 {
                     _parent.transform.position += mainCameraForwardDer * (speed/10) * Time.deltaTime;
                 }
            transform.rotation = Quaternion.LookRotation(cameraDreNoY);
         }

         //�������Ɍ����Ĉړ�������
         if (Input.GetKey(KeyCode.S))
         {
                moveFlag = true;
                anime.SetBool("doWalk", true);
                anime.SetBool("doIdle", false);
                if (jumpFlag == false)
                {
                    _parent.transform.position -= cameraDreNoY * speed * Time.deltaTime;
                }

                if(jumpFlag == true)
                {
                    _parent.transform.position -= cameraDreNoY * (speed/10) * Time.deltaTime;
                }
            transform.rotation = Quaternion.LookRotation(-cameraDreNoY);
         }

        if(!Input.anyKey)
        {
                moveFlag = false;
                anime.SetBool("doIdle", true);
                anime.SetBool("doWalk",false);
        }


        if(Input.GetKeyDown(KeyCode.Space)&& jumpCount == 0 &&jumpFlag ==false)//&& anime.SetBool(doFall.true)&&anime.SetBool(doLanging.true)
        {
            if(RollingJumpFlag == true)
            {
                //���[�����O�W�����v��
                //anime.SetBool("doJump", true);
                this.rb.AddForce(new Vector3(0, jumpSpeed * 30, 0));
                jumpFlag = true;
                jumpCount++;
                anime.SetBool("doLanding", false);
                anime.SetBool("doFall", true);
                RollingJumpFlag = false;
            }
            //�W�����v��
            anime.SetBool("doJump", true);
            this.rb.AddForce(new Vector3(0, jumpSpeed * 30, 0));
            jumpFlag = true;
            jumpCount++;
            anime.SetBool("doLanding",false);
            anime.SetBool("doFall",true);
        }
        #endregion

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
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

        if (other.gameObject.CompareTag("RollingPoint"))
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
                    RollingJumpFlag = true;
                    /*Debug.Log("Landing" + anime.GetBool("doLanding"));
                    Debug.Log("doIdle" + anime.GetBool("doIdle"));
                    Debug.Log("doFall"+anime.GetBool("doFall"));*/
                }
            }
            jumpCount = 0;
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
        //�A�C�e���ɓ���������
        /*if (other.gameObject.CompareTag("Item"))
        {
            itemPoint++;
            other.gameObject.SetActive(false);
        }*/

        //
        /*if (other.gameObject.CompareTag(""))
        {
            //SceneManager.LoadScene("IndoorScene");
        }*/

        //�v���C���[���񕜃A�C�e���ɐG�ꂽ��
      /*  if (other.gameObject.CompareTag("HpItem"))
        {
            int itemHp = 3;
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

    //���񂾂Ƃ��Ƀ��Z�b�g����l
    private void PlayerRisetController()
    {
        playerMaxhp = rMaxhp;
        hp = playerMaxhp;
        speed = rSpeed;
        jumpCount = 0;
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
    #endregion
}

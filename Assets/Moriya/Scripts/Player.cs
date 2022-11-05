using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Player : MonoBehaviour
{
    //�v���C���[�X�e�[�^�X
    private int hp = 0;
    private float speed = 10.0f;
    private int playerMaxhp = 3;
    private int itemPoint = 0;
    private int jumpCount = 0;
    //private bool pJumpFlag = false;

    //�v���C���[��Transform�̒�`
    [SerializeField]
    private Transform parentTran;


    //�����֌W
    private bool speedAccelerationFlag = false;
    private float speedCTime = 0;
    private float speedTime = 10.0f;

    //���C���[�֌W
    [Header("���C���[")] [SerializeField] private GameObject wire;
    private bool wireItemFlag = false;
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

    public bool WireItemFlag
    {
        get { return this.wireItemFlag; }
        set { this.wireItemFlag = value; }
    }

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
    }

    // Update is called once per frame
    void Update()
    {

        if(speedAccelerationFlag == true)
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


     
        //�������Ɍ����Ĉړ�������
        if (Input.GetKey(KeyCode.A))
        {
           
            //�������̉摜�ɕύX����
            /*playerDirection = PlayerDirection.LEFT;
            sr.sprite = leftImage;*/
            transform.Translate(-1 * speed * Time.deltaTime, 0, 0);
        }
        //�E�����Ɍ����Ĉړ�������
        else if (Input.GetKey(KeyCode.D))
        {
            //�E�����̉摜�ɕύX����
            /*playerDirection = PlayerDirection.RIGHT;
            sr.sprite = rightImage;*/
            transform.Translate(+1 * speed * Time.deltaTime, 0, 0);
        }
        //������Ɍ����Ĉړ�������
        else if (Input.GetKey(KeyCode.W))
        {
            //������̉摜�ɕύX����
            /*playerDirection = PlayerDirection.UP;
            sr.sprite = upImage;*/
            transform.Translate(0, 0, +1 * speed * Time.deltaTime);
        }
        //�������Ɍ����Ĉړ�������
        else if (Input.GetKey(KeyCode.S))
        {
            //�������̉摜�ɕύX����
            /*playerDirection = PlayerDirection.DOWN;
            sr.sprite = defaultImage;*/
            transform.Translate(0, 0, -1 * speed * Time.deltaTime);
        }

        if(Input.GetKeyDown(KeyCode.Space)&& this.jumpCount < 1)
        {
            this.rb.AddForce(new Vector3(0,speed*20, 0));
            jumpCount++;
        }
        #endregion
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            jumpCount = 0;

        }
        /*if (other.gameObject.CompareTag("Wall"))
        {
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
}

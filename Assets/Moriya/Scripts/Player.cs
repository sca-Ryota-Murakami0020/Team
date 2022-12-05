using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Player : MonoBehaviour
{
    //プレイヤーステータス
    private int hp = 0;
    private float speed = 10.0f;
    private int playerMaxhp = 3;
    private int itemPoint = 0;
    private int jumpCount = 0;
    //private bool pJumpFlag = false;

    //プレイヤーのTransformの定義
    [SerializeField]
    private Transform parentTran;

    //プレイヤーアニメーション用変数
    [SerializeField]
    private Animator anime = null;

    //移動
    private bool moveFlag = false;
    //ジャンプ
    private bool jumpFlag = false;

    //加速関係
    private bool speedAccelerationFlag = false;
    private float speedCTime = 0;
    private float speedTime = 10.0f;

    //ワイヤー関係
    //[Header("ワイヤー")] [SerializeField] private GameObject wire;
    //private bool wireItemFlag = false;
    //[SerializeField]
    //ワイヤーした時に表示するUIのプレハブ
    //private GameObject wireUIPrefab;
    //ワイヤーUIのインスタンス
    //private GameObject wireUIInstance;

    //初期化用
    private float rSpeed = 10.0f;
    private int rMaxhp = 3;

    //RigidBodyとボックスコライダーの定義
    private Rigidbody rb;
    private BoxCollider bc;

   //GMとポーズ画面関係のスプリクトの定義
   // private GManager gm;
   private PasueDisplayC pasueDisplayC;

    //ゲッター&セッター
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


    //シングルトン
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

        anime.SetBool("doIdle",true);
    }

    // Update is called once per frame
    void Update()
    {
        //if(anime.SetBool("doWalk",false) && anime.SetBool("doJump",false) && anime.SetBool("doLanging",false))
        //{
            //anime.SetBool("doldle",true);
        //}

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

        #region//移動方法


     
        //左方向に向いて移動したら
        if (Input.GetKey(KeyCode.A))
        {
            //左向きの画像に変更する
            /*playerDirection = PlayerDirection.LEFT;
            sr.sprite = leftImage;*/
            moveFlag = true;
            anime.SetBool("doWalk",true);
            transform.Translate(-1 * speed*Time.deltaTime , 0, 0);
        }
        //右方向に向いて移動したら
        else if (Input.GetKey(KeyCode.D))
        { 
            //右向きの画像に変更する
            /*playerDirection = PlayerDirection.RIGHT;
            sr.sprite = rightImage;*/
            moveFlag = true;
            anime.SetBool("doWalk", true);
            transform.Translate(+1 * speed * Time.deltaTime, 0, 0);
        }
        //上方向に向いて移動したら
        else if (Input.GetKey(KeyCode.W))
        {
            //上向きの画像に変更する
            /*playerDirection = PlayerDirection.UP;
            sr.sprite = upImage;*/
            moveFlag = true;
            anime.SetBool("doWalk", true);
            transform.Translate(0, 0, +1 * speed * Time.deltaTime);
        }
        //下方向に向いて移動したら
        else if (Input.GetKey(KeyCode.S))//anime.SetBool(doLanging.true)
        {
            //下向きの画像に変更する
            /*playerDirection = PlayerDirection.DOWN;
            sr.sprite = defaultImage;*/
            moveFlag = true;
            anime.SetBool("doWalk", true);
            transform.Translate(0, 0, -1 * speed * Time.deltaTime);
        }

        moveFlag = false;



        if(Input.GetKeyDown(KeyCode.Space)&& this.jumpCount < 1 && jumpFlag ==false)//&& anime.SetBool(doFall.true)&&anime.SetBool(doLanging.true)
        {
            jumpFlag = true;
            this.rb.AddForce(new Vector3(0,speed*20, 0));
            //anime.SetBool(doJump.true);
            jumpCount++;
        }
        #endregion
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground")&&jumpFlag==true)
        {
            jumpFlag = false;
            //anime.SetBool(doLanging.true)
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

        //プレイヤーが回復アイテムに触れたら
      /*  if (other.gameObject.CompareTag("HpItem"))
        {
            int itemHp = 10;
            //hp+アイテム取った時の回復量がMaxhpより多かったら回復量を減らす
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

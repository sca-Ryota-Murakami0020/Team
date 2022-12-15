using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Player : MonoBehaviour
{
    //プレイヤーステータス
    private int hp = 3;
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
    //落下中
    private bool fallFlag = false;
    //着地中
    private bool landFlag = false;

    //　レイを飛ばす場所
    [SerializeField]
    private Transform rayPosition;
    //　レイを飛ばす距離
    [SerializeField]
    private float rayRange = 0.85f;
    //　落ちた場所
    private float fallenPosition;
    //　落下してから地面に落ちるまでの距離
    private float fallenDistance;
    //　どのぐらいの高さからダメージを与えるか
    [SerializeField]
    private float takeDamageDistance = 2f;
    //落下し着地したときのフラグ
    private bool fallGroundFalg = false;

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

    //プレイヤー角度計算
    private Quaternion left;
    private Quaternion Right;
    private Quaternion up;
    private Quaternion down;


    //　Time.timeScaleに設定する値
    [SerializeField]
    private float timeScale = 0.1f;
    //　時間を遅くしている時間
    [SerializeField]
    private float slowTime = 1f;
    //　経過時間
    private float elapsedTime = 0f;
 

    private float time =0.0f;

    //親オブジェクト
    private GameObject _parent;

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

        //親オブジェクト取得
        _parent = transform.root.gameObject;

        anime.SetBool("doIdle",true);

        //落ちた時に使う数値リセット
        fallenDistance = 0f;
        fallenPosition = transform.position.y;
        fallFlag= false;

        left = Quaternion.Euler(0,-90,0);
        Right = Quaternion.Euler(0,90,0);
        up = Quaternion.Euler(0,180,0);
        down= Quaternion.Euler(0,0,0);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(hp);
        //　落ちている状態
        if (fallFlag)
        {

            //　落下地点と現在地の距離を計算（ジャンプ等で上に飛んで落下した場合を考慮する為の処理）
            //落下地点 = 落下地点かプレイヤーの落下地点の最大値
            fallenPosition = Mathf.Max(fallenPosition, transform.position.y);
            Debug.Log(fallenPosition);

            //　地面にレイが届いていたら
            if (Physics.Linecast(rayPosition.position, rayPosition.position + Vector3.down * rayRange, LayerMask.GetMask("Ground")))
            {
                //落ちて地面についた時のフラグ
                fallGroundFalg = true;

                //　落下距離を計算
                fallenDistance = fallenPosition - transform.position.y;
                //呼び出し
                if (fallGroundFalg)
                {
                    Falldamage();
                }
                //リセット
                fallFlag = false;
            }
        }
        else
        {
            //　地面にレイが届いていなければ落下地点を設定
            if (!Physics.Linecast(rayPosition.position, rayPosition.position + Vector3.down * rayRange, LayerMask.GetMask("Ground")))
            {
                //　最初の落下地点を設定
                fallenPosition = transform.position.y;
                fallenDistance = 0;
                fallFlag = true;
            }
        }


        //if(anime.SetBool("doWalk",false) && anime.SetBool("doJump",false) && anime.SetBool("doLanging",false))
        //{
        anime.SetBool("doIdle",true);
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
            _parent.transform.position -= Vector3.right * speed *Time.deltaTime;
            anime.SetBool("doWalk",true);
            transform.rotation = left;
        }
        //右方向に向いて移動したら
        else if (Input.GetKey(KeyCode.D))
        {
            //右向きの画像に変更する
            /*playerDirection = PlayerDirection.RIGHT;
            sr.sprite = rightImage;*/
            moveFlag = true;
            _parent.transform.position += Vector3.right * speed * Time.deltaTime;
            anime.SetBool("doWalk", true);
            transform.rotation = Right;
        }
        //上方向に向いて移動したら
        else if (Input.GetKey(KeyCode.W))
        {
            //上向きの画像に変更する
            /*playerDirection = PlayerDirection.UP;
            sr.sprite = upImage;*/
            moveFlag = true;
            anime.SetBool("doWalk", true);
            _parent.transform.position -= Vector3.forward * speed * Time.deltaTime;
            transform.rotation = up;
        }
        //下方向に向いて移動したら
        else if (Input.GetKey(KeyCode.S))//anime.SetBool(doLanging.true)
        {
            //下向きの画像に変更する
            /*playerDirection = PlayerDirection.DOWN;
            sr.sprite = defaultImage;*/
            moveFlag = true;
            anime.SetBool("doWalk", true);
            _parent.transform.position += Vector3.forward * speed * Time.deltaTime;
            transform.rotation =down;
        }
        else
        {
            moveFlag = false;
            anime.SetBool("doIdle", true);
            anime.SetBool("doWalk",false);
           
        }


        if(Input.GetKeyDown(KeyCode.Space)&& jumpCount == 0 &&jumpFlag ==false)//&& anime.SetBool(doFall.true)&&anime.SetBool(doLanging.true)
        {
            //ジャンプ時
            time +=Time.deltaTime;
            anime.SetBool("doJump", true);
            jumpFlag = true;
            this.rb.AddForce(new Vector3(0,speed*50, 0));
            jumpCount++;

            Debug.Log(time);

            //ジャンプから落下モーションへ
            //if(anime.GetCurrentAnimatorStateInfo().normalizedTime)
            fallFlag = true;
            anime.SetBool("doLanding",false);
            //anime.SetBool("doJump",false);
            anime.SetBool("doFall",true);
            Debug.Log("Landing" + anime.GetBool("doLanding"));
            Debug.Log("doJump" + anime.GetBool("doJump"));
            Debug.Log("doIdle" + anime.GetBool("doIdle"));
            Debug.Log("doFall" + anime.GetBool("doFall"));
            time = 0.0f;
        }

        #endregion


    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            if (jumpFlag == true) 
            {
                //落下モーションか着地モーションへ
                time += Time.deltaTime;
                jumpFlag = false;
                anime.SetBool("doJump", false);
                anime.SetBool("doFall",false);
                anime.SetBool("doLanding",true);
                anime.SetBool("doIdle",false);
                landFlag = true;

                Debug.Log(time);


                //着地モーションから待機モーションへ
                if (jumpFlag == false)
                {
                    anime.SetBool("doLanding", false);
                    anime.SetBool("doIdle", true);
                    /*Debug.Log("Landing" + anime.GetBool("doLanding"));
                    Debug.Log("doIdle" + anime.GetBool("doIdle"));
                    Debug.Log("doFall"+anime.GetBool("doFall"));*/
                    time =0.0f;
                   
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

    private void Falldamage()
    {
        //スローモーションの制限時間用
        elapsedTime += Time.unscaledDeltaTime;
        //1秒いないならスローモーションにする
        if (elapsedTime < slowTime)
        {
            Time.timeScale = timeScale;

            //　落下によるダメージが発生する距離を超える場合かつEキーが押されていなかったらダメージを与える
            if (!Input.GetKey(KeyCode.E) && fallenDistance >= takeDamageDistance)
            {
                hp--;
            }
        }

        //スローモーション解除
        if (elapsedTime > slowTime)
        {
            Debug.Log("とけた");
            Time.timeScale = 1f;
            elapsedTime = 0.0f;
            fallGroundFalg = false;
        }
    }

    private void PlayerRisetController()
    {
        playerMaxhp = rMaxhp;
        hp = playerMaxhp;
        speed = rSpeed;
        jumpCount = 0;
    }

    private void Fallsituation()
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
        time = 0.0f;
    }
}

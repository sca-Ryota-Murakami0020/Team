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
    private float jumpSpeed =10.0f;
    private float fallSpeed = -0.1f; 
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

    //ダメージ
    private bool damageFlag = false;
    //移動
    private bool moveFlag = false;
    //ジャンプ
    private bool jumpFlag = false;
    //落下中
    private bool fallFlag = false;
    //着地中
    private bool landFlag = false;
    //ローリング中
    private bool RollingJumpFlag = false;

    //　レイを飛ばす場所
    [SerializeField]
    private Transform rayPosition;
    //　レイを飛ばす距離
    [SerializeField]
    private float rayRange;
    //　落ちた場所
    private float fallenPosition;
    //　落下してから地面に落ちるまでの距離
    private float fallenDistance;
    //　どのぐらいの高さからダメージを与えるか
    [SerializeField]
    private float takeDamageDistance = 3f;

    //加速関係
    private bool speedAccelerationFlag = false;
    private float speedCTime = 0;
    private float speedTime = 10.0f;

    //初期化用
    private float rSpeed = 10.0f;
    private int rMaxhp = 3;

    //RigidBodyとボックスコライダーの定義
    private Rigidbody rb;
    private BoxCollider bc;

    //GMとポーズ画面関係のスプリクトの定義
    // private GManager gm;
    private PasueDisplayC pasueDisplayC;
    private PlayerWallCon playerWallConC;


    //　Time.timeScaleに設定する値
    [SerializeField]
    private float timeScale = 0.1f;
    //　時間を遅くしている時間
    private float slowTime = 1f;
    //　経過時間
    private float elapsedTime = 0f;
    private bool fallDamegeFlag;
 
    //親オブジェクト
    private GameObject _parent;
    //子オブジェクト
    private GameObject child;



 
    //カメラ
    [SerializeField]
    private GameObject mainCamera;
    private Vector3 mainCameraForwardDer;
    private Vector3 mainCameraRightDer;

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
    /*private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }*/


    // Start is called before the first frame update
    void Start()
    {

        //コンポーネント
        rb = GetComponent<Rigidbody>();
        bc = GetComponent<BoxCollider>();
        //gm = FindObjectOfType<GManager>();
        pasueDisplayC = FindObjectOfType<PasueDisplayC>();
        playerWallConC = FindObjectOfType<PlayerWallCon>();
        this.anime = GetComponent<Animator>();

        //hp初期化
        hp = playerMaxhp;
 
        //親オブジェクト取得
        _parent = transform.root.gameObject;
        //子オブジェクト取得
        child =transform.GetChild(2).gameObject;

        //アニメーション初期化
        anime.SetBool("doIdle",true);

        //落ちた時に使う数値リセット
        fallenDistance = 0f;
        fallenPosition = transform.position.y;
        fallFlag= false;
    }

    // Update is called once per frame
    void Update()
    {

        //Debug.Log(hp);
        //Debug.Log(Time.timeScale);

        //カメラの角度取得と単位ベクトル化
        mainCameraForwardDer = mainCamera.transform.forward.normalized;
        mainCameraRightDer = mainCamera.transform.right.normalized;
        Vector3 cameraDreNoY = new Vector3(mainCameraForwardDer.x,0,mainCameraForwardDer.z);
        cameraDreNoY = cameraDreNoY.normalized;

        //何もなかったら待機モーション
        //anime.SetBool("doIdle", true);

        #region//落下状態
        //　落ちている状態
        if (fallFlag)
        {
            //徐々に落下速度を加速させる
            transform.position += transform.up * Time.deltaTime * fallSpeed;
            //　落下地点と現在地の距離を計算（ジャンプ等で上に飛んで落下した場合を考慮する為の処理）
            //落下地点 = 落下地点かプレイヤーの落下地点の最大値
            fallenPosition = Mathf.Max(fallenPosition, transform.position.y);

            //　地面にレイが届いていたら
            if (Physics.Linecast(rayPosition.position, rayPosition.position + Vector3.down * rayRange, LayerMask.GetMask("Ground")))
            {
                //　落下距離を計算
                fallenDistance = fallenPosition - transform.position.y;
                if(fallenDistance >= takeDamageDistance)
                {
                    //フラグたてる
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
            //　地面にレイが届いていなければ落下地点を設定
            if (!Physics.Linecast(rayPosition.position, rayPosition.position + Vector3.down * rayRange, LayerMask.GetMask("Ground")))
            {
                //　最初の落下地点を設定
                fallenPosition = transform.position.y;
                fallenDistance = 0;
                fallFlag = true;
            }
        }
        #endregion

        //アニメーションしたら加速
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

        //ゲームオーバーシーンに飛ぶ式
        if (hp <= 0)
        {
            //SceneManager.LoadScene("GameOverScene");
            PlayerRisetController();
        }

        #region//移動＆ジャンプ方法
        //十字キー操作
        //左方向に向いて移動したら
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

         //右方向に向いて移動したら
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

         //上方向に向いて移動したら
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

         //下方向に向いて移動したら
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
                //ローリングジャンプ時
                //anime.SetBool("doJump", true);
                this.rb.AddForce(new Vector3(0, jumpSpeed * 30, 0));
                jumpFlag = true;
                jumpCount++;
                anime.SetBool("doLanding", false);
                anime.SetBool("doFall", true);
                RollingJumpFlag = false;
            }
            //ジャンプ時
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
                    //落下モーションか着地モーションへ
                    jumpFlag = false;
                    anime.SetBool("doJump", false);
                    anime.SetBool("doFall", false);
                    anime.SetBool("doLanding", true);
                    anime.SetBool("doIdle", false);
                    landFlag = true;
                    //着地モーションから待機モーションへ
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
                //落下モーションか着地モーションへ
                jumpFlag = false;
                anime.SetBool("doJump", false);
                anime.SetBool("doFall", false);
                anime.SetBool("doLanding", true);
                anime.SetBool("doIdle", false);
                landFlag = true;
                //着地モーションから待機モーションへ
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
        //アイテムに当たったら
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

        //プレイヤーが回復アイテムに触れたら
      /*  if (other.gameObject.CompareTag("HpItem"))
        {
            int itemHp = 3;
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

    //死んだときにリセットする値
    private void PlayerRisetController()
    {
        playerMaxhp = rMaxhp;
        hp = playerMaxhp;
        speed = rSpeed;
        jumpCount = 0;
    }

    #region//コルーチン
    //スローモーションの元コルーチン
    private IEnumerator StartSlowmotion()
    {
        //slowmotion本体を起動
        StartCoroutine("Slowmotion");

        //1.0秒待つ
        yield return  new WaitForSecondsRealtime(1.0f);

        //slowmotion本体をストップ
        StopCoroutine("StartSlowmotion");
        StopCoroutine("Slowmotion");
    }
    //スローモーションするコルーチン
    private IEnumerator Slowmotion()
    {
        //遅くする
        Time.timeScale = timeScale;

        //Debug.Log("slowmotion");

        //時間計測＋キー判定
        while (elapsedTime < slowTime)
        {
            //1秒いないならスローモーションにする
            Time.timeScale = timeScale;
            //スローモーションの制限時間用
            elapsedTime += Time.unscaledDeltaTime;
            //Debug.Log("elapsed"+elapsedTime);
            //　落下によるダメージが発生する距離を超える場合かつEキーが押されていなかったらダメージを与える
            if (!Input.GetKey(KeyCode.E) && fallenDistance >= takeDamageDistance && fallDamegeFlag == true)
            {
                fallDamegeFlag = false;
                damageFlag = true;
            }
            //スローモーション解除
            if (elapsedTime > slowTime)
            {
                Debug.Log("とけた");
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

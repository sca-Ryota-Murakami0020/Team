using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;


public class Player : MonoBehaviour
{
    #region//プレイヤーステータス
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

    //プレイヤーのTransformの定義
    [SerializeField]
    private Transform parentTran;

    //プレイヤーアニメーション用変数
    [SerializeField]
    private Animator anime = null;

    #region//状況に応じて使用するフラグ
    //落下してダメージ判定になった時のフラグ
    private bool fallDamageFlag;
    //落下したときのダメージが入ったとき
    private bool fallDamageHitFlag = false;
    //移動
    private bool moveFlag = false;
    //ジャンプ
    private bool jumpFlag = false;
    //落下中
    private bool fallFlag = false;
    //着地中
    private bool landFlag = false;
    //ローリングジャンプ地点にふれたフラグ
    private bool rollingJumpFlag = false;
    //ローリングジャンプをしたフラグ
    private bool rollingJumpDidFlag = false;
    //壁はりつきジャンプができる地点にふれたフラグ
    private bool wallClingJumpFlag = false;
    //ゲームオーバー
    private bool gameOverFlag = false;
    //使わないけどコルーチン用
    private bool sameTransFlag = false;
    #endregion

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
    private float accelSpeed;

    //初期化用
    private float rSpeed = 10.0f;
    private int rMaxhp = 3;

    //RigidBodyとボックスコライダーの定義
    private Rigidbody rb;
    private BoxCollider bc;

    //GMとポーズ画面関係のスプリクトの定義
    private totalGameManager gm;
    private PasueDisplayC pasueDisplayC;
    private PlayerWallCon playerWallConC;

    //　Time.timeScaleに設定する値
    [SerializeField]
    private float timeScale = 0.1f;
    //　時間を遅くしている時間
    private float slowTime = 1f;
    //　経過時間
    private float elapsedTime = 0f;
    
    //プレイヤーのダメージモーション時間
    private float damazeAnimeTime = 0;
 
    //親オブジェクト
    private GameObject _parent;
    //子オブジェクト
    private GameObject child;

    //カメラ
    [SerializeField]
    private GameObject mainCamera;
    private Vector3 mainCameraForwardDer;
    private Vector3 mainCameraRightDer;

    // 画像描画用のコンポーネント
    [SerializeField]
    SkinnedMeshRenderer smr;
    STATE state;
    //点滅感覚
    [SerializeField]
    private float flashInterval;
    //点滅させるときのループカウント
    [SerializeField] 
    private int loopCount;
    //当たったかどうかのフラグ
    private bool isHit;

    [SerializeField]
    private GameObject[] heartArray = new GameObject[3];

    private Vector3 playerTrans;
    Vector3 oldTrans;
    private Vector3 newTrans;


    //プレイヤーの状態用列挙型（ノーマル、ダメージ、２種類）
    enum STATE
    {
        NOMAL,
        DAMAGED,
    }

    #region//ゲッター&セッター
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

    //シングルトン
    /*private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }*/
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //コンポーネント
        rb = GetComponent<Rigidbody>();
        bc = GetComponent<BoxCollider>();
        gm = FindObjectOfType<totalGameManager>();
        pasueDisplayC = FindObjectOfType<PasueDisplayC>();
        playerWallConC = FindObjectOfType<PlayerWallCon>();
        this.anime = GetComponent<Animator>();

        //hp初期化
        hp = gm.PlayerHp;
        oldHp = hp;
 
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

        //スピードの初期化
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

        // ステートがダメージならリターン
        if (state == STATE.DAMAGED)
        {
            return;
        }

        //カメラの角度取得と単位ベクトル化
        mainCameraForwardDer = mainCamera.transform.forward.normalized;
        mainCameraRightDer = mainCamera.transform.right.normalized;
        Vector3 cameraDreNoY = new Vector3(mainCameraForwardDer.x,0,mainCameraForwardDer.z);
        cameraDreNoY = cameraDreNoY.normalized;

        //何もなかったら待機モーション
        //anime.SetBool("doIdle", true);

        //hpが減った時の処理
        if (hp < oldHp && hp >= 1)
        {
            HpDisplay();
            oldHp = hp;
            state = STATE.DAMAGED;
            StartCoroutine(_hit());
        }

        //ゲームオーバーシーンに飛ぶ式
        if (hp == 0)
        {
            gameOverFlag = true;
            anime.SetTrigger("lose");
            if (gameOverFlag == true)
            {
                StartCoroutine(GameOver());
            }
        }

        #region//落下状態
        //　落ちている状態
        //スタートでは落下状態ではないのでfallFlagはfalseとなっている

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
            //徐々に落下速度を加速させる
            //transform.position -= transform.up * Time.deltaTime * fallSpeed;
            //Debug.Log("自分の位置"+transform.position.y);
            //落下地点と現在地の距離を計算（ジャンプ等で上に飛んで落下した場合を考慮する為の処理）
            //落下地点 = 落下地点かプレイヤーの落下地点の最大値
            fallenPosition = Mathf.Max(fallenPosition, transform.position.y);
            //Debug.Log("fallenPosition" + fallenPosition);

            RaycastHit hit;
            //　地面にレイが届いていたら
            if (Physics.Linecast(rayPosition.position, rayPosition.position + Vector3.down * rayRange, out hit))
            {
                //　落下距離を計算
                fallenDistance = fallenPosition - transform.position.y;
                if (fallenDistance >= takeDamageDistance)
                {
                    //フラグたてる
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
                   //着地のモーションを入れる
                   //Debug.Log("着地モーションの出力");
                }
                //Debug.Log("fallFlagをfalseに変換する");
                fallFlag = false;            
            }
        }
        else
        {
            //fallFlagがfalse状態でかつプレイヤーが地面から離れた時にfallFlagをtrueにする
            //地面にレイが届いていなければ落下地点を設定
            if (!Physics.Linecast(rayPosition.position, rayPosition.position + Vector3.down * rayRange))
            {
              //StartCoroutine(PlayerTransform());
              //if (sameTransFlag == true)
              //{ 
                //地面から一回でもLineCastの線が離れたとき = 落下状態とする
                //その時に落下状態を判別するためfallFlagをtrueにする
                //最初の落下地点を設定
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

        //アニメーションしたら加速
        if (speedAccelerationFlag == true)
        {
            //StartCoroutine(StartAcceleration());
            speedCTime++;
            accelSpeed = originSpeed * 1.2f;
            speed = accelSpeed;
            Debug.Log("加速処理にはいった");
            if(speedTime < speedCTime)
            {
                Debug.Log("加速処理終了");
                speed = originSpeed;
                speedCTime = 0;
                speedAccelerationFlag = false;
            }

        }

        #region//移動＆ジャンプ方法
        //十字キー操作
        //左方向に向いて移動したら
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

         //右方向に向いて移動したら
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

         //上方向に向いて移動したら
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

         //下方向に向いて移動したら
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
            //Debug.Log("停止中");
            //Debug.Log("fallFlag:" + fallFlag);
            if(fallFlag == false)
            {
                //Debug.Log("停止中");
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
                //ローリングジャンプ時
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
                //ジャンプ時
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
            //Debug.Log("じめん");

            if (rollingJumpDidFlag == true && jumpFlag == true)
            {
                //Debug.Log("asuta");
                jumpFlag = false;
                rollingJumpDidFlag = false;
                //anime.SetBool("RollingAriIdle",false);
                //ローリングジャンプアニメーションをきる
                //;
                anime.SetBool("doLanding", true);
                anime.SetBool("doIdle", false);
                //着地モーションから待機モーションへ
                if (jumpFlag == false)
                {
                    //Debug.Log("上");
                    anime.SetBool("doLanding", false);
                    anime.SetBool("doIdle", true);
                    /*Debug.Log("Landing" + anime.GetBool("doLanding"));
                    Debug.Log("doIdle" + anime.GetBool("doIdle"));
                    Debug.Log("doFall"+anime.GetBool("doFall"));*/
                }
            }

            if (jumpFlag == true)
            {
                    //落下モーションか着地モーションへ
                    jumpFlag = false;
                    anime.SetBool("doJump", false);
                    anime.SetBool("doLanding", true);
                    anime.SetBool("doIdle", false);
                    //着地モーションから待機モーションへ
                    if (jumpFlag == false)
                    {
                        //Debug.Log("下");
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
            //Debug.Log("反応した");
            if (jumpFlag == true)
            {
                //落下モーションか着地モーションへ
                jumpFlag = false;
                anime.SetBool("doJump", false);
                //anime.SetBool("doFall", false);
                anime.SetBool("doLanding", true);
                anime.SetBool("doIdle", false);
                rollingJumpDidFlag = false;
                anime.SetBool("RollingAriIdle",false);
                //Debug.Log("haitta");
                //着地モーションから待機モーションへ
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
                //落下モーションか着地モーションへ
                jumpFlag = false;
                anime.SetBool("doJump", false);
                //anime.SetBool("doFall", false);
                anime.SetBool("doLanding", true);
                anime.SetBool("doIdle", false);
                //着地モーションから待機モーションへ
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
                //落下モーションか着地モーションへ
                jumpFlag = false;
                //anime.SetBool("doJump", false);
                //anime.SetBool("doFall", false);
                anime.SetBool("doIdle", false);
                //着地モーションから待機モーションへ
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
        //アイテムに当たったら
        if (other.gameObject.CompareTag("Item"))
        {
            gm.PlayerIC++;
            other.gameObject.SetActive(false);
        }
    }

    //死んだときにリセットする値
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

    //Hp減った時の処理
    private void HpDisplay()
    {
        heartArray[hp].gameObject.SetActive(false);
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
            if (!Input.GetKey(KeyCode.E) && fallenDistance >= takeDamageDistance && fallDamageFlag == true)
            {
                fallDamageFlag = false;
                fallDamageHitFlag = true;
            }
            //スローモーション解除
            if (elapsedTime > slowTime)
            {
                //Debug.Log("とけた");
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
        //点滅ループ開始
        for (int i = 0; i < loopCount; i++)
        {
            if (isHit == false)
            {
                continue;
            }
            //flashInterval待ってから
            yield return new WaitForSeconds(flashInterval);
            //spriteRendererをオフ
            smr.enabled = false;

            //flashInterval待ってから
            yield return new WaitForSeconds(flashInterval);
            //spriteRendererをオン
            smr.enabled = true;
        }
        //デフォルト状態にする
        state = STATE.NOMAL;
        //点滅ループが抜けたら当たりフラグをfalse(当たってない状態)
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
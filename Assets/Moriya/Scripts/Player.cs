using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;


public class Player : MonoBehaviour
{
    #region//プレイヤーステータス
    private int oldHp;
    //移動速度
    //ローリングジャンプした時のx方向スピード
    private float jumpRollingSpeed = 5.0f;
    //ジャンプした時のx方向スピード
    private float jumpingRunSpeed = 2.5f;
    //ジャンプした時のy方向スピード
    private float jumpSpeed =10.0f;
    //方向速度
    private float runSpeed = 5.0f;
    //落下中のy方向スピード(使っていない)
    private float fallSpeed = -0.1f; 
    //ジャンプする回数
    private int jumpCount = 0;
    #endregion

    //プレイヤーアニメーション用変数
    [SerializeField]
    private Animator anime = null;

    #region//状況に応じて使用するフラグ
    //落下したときのダメージが入るかどうかのフラグ
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
    //壁はりつきジャンプをしたフラグ
    private bool wallClingJumpDidFlag = false;
    //ゲームオーバー
    private bool gameOverFlag = false;
    //使わないけどコルーチン用
    //private bool sameTransFlag = false;
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
    //加速したかどうかのフラグ
    private bool speedAccelerationFlag = false;
    //カウント用
    private float speedCTime = 0;
    //加速制限時間
    private float speedTime = 1000.0f;
    //加速する値を入れる変数
    private float accelSpeed;
    //加速リセットする際に使う変数
    private float defaultSpeed = 5.0f;

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
    
    //親オブジェクト
    private GameObject _parent;
    //子オブジェクト
    private GameObject child;

    //カメラ
    [SerializeField]
    private GameObject mainCamera;
    //カメラの方向二種類
    private Vector3 mainCameraForwardDer;
    private Vector3 mainCameraRightDer;

    // 画像描画用のコンポーネント
    [SerializeField]
    SkinnedMeshRenderer smr;
    //プレイヤーの状態を認識する
    STATE state;
    //点滅感覚
    [SerializeField]
    private float flashInterval;
    //点滅させるときのループカウント
    [SerializeField] 
    private int loopCount;
    //当たったかどうかのフラグ
    private bool isHit;

    //プレイヤーのhpを表示するためのUIプレハブ
    [SerializeField]
    private GameObject[] heartArray = new GameObject[3];

    //効果音関係
    [SerializeField]
    private AudioClip jumpSE;
    [SerializeField]
    private AudioClip randingSE;
    [SerializeField]
    private AudioClip damegeSE;

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
        oldHp = gm.PlayerHp;
 
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

        //加速に使う時の速度
        accelSpeed = runSpeed * 1.5f;
    }

    // Update is called once per frame
    void Update()
    {
        //プレイヤーhp表示をするためのfor文
        for (int i = 0; i < gm.PlayerHp; i++)
        {
            heartArray[i].gameObject.SetActive(true);
        }

        Debug.Log(rollingJumpFlag);

        Debug.DrawLine(rayPosition.position, rayPosition.position + Vector3.down * rayRange, Color.red, 1.0f);

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
        //hpが0まではここ
        if (gm.PlayerHp < oldHp && gm.PlayerHp >= 1)
        {
            //アニメーション＆効果音流し
            Debug.Log("a");
            anime.SetTrigger("domazeed");
            gm.PlaySE(damegeSE);

            //Hp表示と点滅表示
            HpDisplay();
            oldHp = gm.PlayerHp;
            state = STATE.DAMAGED;
            StartCoroutine(_hit());
        }

        //ゲームオーバーシーンに飛ぶ式
        //hpが0になったらここに入る
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

        #region//落下状態
        //　落ちている状態
        //スタートでは落下状態ではないのでfallFlagはfalseとなっている

        if (fallFlag == true)
        {
            //ローリング着地に関わる値をリセット
            anime.SetBool("doFall", true);
            anime.SetBool("doLandRolling",false);

            //ジャンプやローリングジャンプしなかった時にここに入る
            if (rollingJumpDidFlag == false && jumpFlag == false && moveFlag == true)
            {
                anime.SetBool("doWalk", true);
                anime.SetBool("doJump",false);
            }

            //ローリングジャンプした時にここに入る
            if(rollingJumpDidFlag == true)
            {
                anime.SetBool("doWalk",false);
                anime.SetBool("doJump",false);
                anime.SetBool("RollingAriIdle", true);
            }

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
                    //スローモーション移行
                    StartCoroutine("StartSlowmotion");
                    //ダメージが入るフラグが経ったら
                    if (fallDamageHitFlag == false)
                    {
                        //ローロング着地のフラグを立てる
                        anime.SetBool("doFall", false);
                        anime.SetBool("doLandRolling", true);
                        //地面に着地したら
                        if (hit.transform.gameObject.CompareTag("Ground"))
                        {
                            //普通のジャンプをしていたら
                            if(jumpFlag == true)
                            {
                                jumpFlag = false;
                                anime.SetBool("doJump", false);
                                //着地モーションから待機モーションへ
                                if (jumpFlag == false)
                                {
                                    anime.SetBool("doIdle", true);
                                    /*Debug.Log("Landing" + anime.GetBool("doLanding"));
                                    Debug.Log("doIdle" + anime.GetBool("doIdle"));
                                    Debug.Log("doFall"+anime.GetBool("doFall"));*/
                                }
                            }
                            //ローリングジャンプをしていたら
                            if (rollingJumpDidFlag == true)
                            {
                                //ローロング空中待機モーションから待機モーションへ
                                anime.SetBool("doIdle", true);
                                anime.SetBool("RollingAriIdel", false);
                                rollingJumpDidFlag = false;
                            }
                            /*if(wallClingJumpDidFlag == true)
                            {
                                wallClingJumpDidFlag == false;
                            }*/
                        }
                        //ローロングジャンプポイントに着地したら
                        if (hit.transform.gameObject.CompareTag("RollingJumpPoint"))
                        {
                            //普通のジャンプをしていたら
                            if (jumpFlag == true)
                            {
                                jumpFlag = false;
                                //着地モーションから待機モーションへ
                                if (jumpFlag == false)
                                {
                                    anime.SetBool("doIdle", true);
                                    rollingJumpFlag = true;
                                    /*Debug.Log("Landing" + anime.GetBool("doLanding"));
                                    Debug.Log("doIdle" + anime.GetBool("doIdle"));
                                    Debug.Log("doFall"+anime.GetBool("doFall"));*/
                                }
                            }
                            //ローリングジャンプをしていたら
                            if (rollingJumpDidFlag == true)
                            {
                                // //ローロング空中待機モーションから待機モーションへ
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
                        //加速するフラグをたてる
                        speedAccelerationFlag = true;
                    }
                    //ダメージが入るフラグが経ったら
                    if (fallDamageHitFlag == true)
                    {
                        gm.PlayerHp--;
                        //gm.PDFlag = true;
                        anime.SetBool("doFall", false);
                        fallDamageHitFlag = false;
                    }
                    Debug.Log("sameki");
                }
                else//ダメージなしの着地
                {
                    //通常着地モーションをする
                    anime.SetBool("doFall", false);
                    anime.SetBool("doLanding", true);
                   
                    //上と同じくしている
                    if (hit.transform.gameObject.CompareTag("Ground"))
                    {
                        if (jumpFlag == true)
                        {
                            //落下モーションか着地モーションへ
                            anime.SetBool("doJump", false);
                            anime.SetBool("doIdle", false);
                            //Debug.Log("haitta");
                            jumpFlag = false;
                            //着地モーションから待機モーションへ
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
                            Debug.Log("地面 " + rollingJumpDidFlag);
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
                    //上と同じ
                    if (hit.transform.gameObject.CompareTag("RollingJumpPoint"))
                    {
                        if (jumpFlag == true)
                        {
                            //落下モーションか着地モーションへ
                            anime.SetBool("doJump", false);
                            anime.SetBool("doIdle", false);
                            jumpFlag = false;
                            //着地モーションから待機モーションへ
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
                            Debug.Log("ローリングジャンプポイント " + rollingJumpDidFlag);
                            anime.SetBool("RollingAriIdle", false);
                            //ローリングジャンプアニメーションをきる
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
                //ここでフラグおり＆着地の効果音を入れている
               fallFlag = false;
               gm.PlaySE(randingSE);
               Debug.Log("niya");
            }
        }
        else//地面にいる時
        {
            //fallFlagがfalse状態でかつプレイヤーが地面から離れた時にfallFlagをtrueにする
            //地面にレイが届いていなければ落下地点を設定
            if (!Physics.Linecast(rayPosition.position, rayPosition.position + Vector3.down * rayRange))
            {
                //地面から一回でもLineCastの線が離れたとき = 落下状態とする
                //その時に落下状態を判別するためfallFlagをtrueにする
                //最初の落下地点を設定
                fallenPosition = transform.position.y;
                fallenDistance = 0;
                //フラグを立てる
                fallFlag = true;
            }
        }
        #endregion

        //アニメーションしたら加速
        if (speedAccelerationFlag == true)
        {
            //制限時間計算と速度変換
            speedCTime++;
            runSpeed = accelSpeed;
            Debug.Log("加速処理にはいった");
            if(speedTime < speedCTime)
            {
                Debug.Log("加速処理終了");
                //制限時間＆速度リセット
                runSpeed = defaultSpeed;
                speedCTime = 0;
                speedAccelerationFlag = false;
            }

        }

        #region//移動＆ジャンプ方法
        //十字キー操作
        //中の処理はWASDどれも同じ
        //左方向に向いて移動したら
         if (Input.GetKey(KeyCode.A))
         {
            moveFlag = true;

            //落下中ではなければ歩きモーションをたてる
            if(fallFlag == false)
            {
                anime.SetBool("doIdle", false);
                anime.SetBool("doWalk", true);
            }

            //普通の歩くスピード
            if (jumpFlag == false && rollingJumpDidFlag == false)
            {
                _parent.transform.position -= mainCameraRightDer * runSpeed * Time.deltaTime;
            }

            //通常ジャンプのスピード
            if(jumpFlag == true)
            {
               _parent.transform.position -= mainCameraRightDer * jumpingRunSpeed * Time.deltaTime;
            }

            //ローロングジャンプのスピード
            if(rollingJumpDidFlag == true)
            {
                _parent.transform.position -= mainCameraRightDer * jumpRollingSpeed * Time.deltaTime;
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

            if (jumpFlag == false && rollingJumpDidFlag == false)
            {
                _parent.transform.position += mainCameraRightDer * runSpeed * Time.deltaTime;

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

         //上方向に向いて移動したら
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

         //下方向に向いて移動したら
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

         //方向キーが押されていなければここに入る
        if(!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) &&!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
        {
            moveFlag = false;
            //落下中でなければ待機モーションに入る
            if (fallFlag == false)
            {
                //Debug.Log("停止中");
                anime.SetBool("doIdle", true);
                anime.SetBool("doWalk",false);
            }              
        }

        if (Input.GetKeyDown(KeyCode.Space)&& jumpCount == 0 && fallFlag == false)
        {
            //ジャンプの効果音を流す
            gm.PlaySE(jumpSE);

            //ローロングジャンプができない状態なら
            if (rollingJumpFlag == false)
            {
                //ジャンプ時
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


            //ローロングジャンプができる状態なら
            if (rollingJumpFlag == true)
            {
                //ローリングジャンプ時
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
        //敵に当たったら
        if (other.gameObject.CompareTag("Enemy"))
        {
            gm.PlayerHp--;
        }

        //地面に当たったら
        if (other.gameObject.CompareTag("Ground"))
        {
            //Debug.Log("じめん");
            rollingJumpFlag = false;
            //wallClingJumpFlag = false;
            jumpCount = 0;
        }

        //ローリングジャンプポイントに当たったら
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
                //落下モーションか着地モーションへ
                anime.SetBool("doJump", false);
                anime.SetBool("doLanding", true);
                anime.SetBool("doIdle", false);
                //着地モーションから待機モーションへ
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
                //落下モーションか着地モーションへ
                jumpFlag = false;
                //anime.SetBool("doJump", false);
                anime.SetBool("doIdle", false);
                //着地モーションから待機モーションへ
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
        //アイテムに当たったら
        if (other.gameObject.CompareTag("Item"))
        {
            gm.PlayerIC++;
            other.gameObject.SetActive(false);
        }

        //シーン移動
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

 
    //Hp減った時の処理
    private void HpDisplay()
    {
        heartArray[gm.PlayerHp].gameObject.SetActive(false);
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
            if (!Input.GetKey(KeyCode.E) && fallenDistance >= takeDamageDistance)
            {
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
            //ゲームオーバーシーンにいく
            yield return new WaitForSeconds(1);
            //PlayerRisetController();
            SceneManager.LoadScene("GameOverScene");
            break;
        }
    }

    #endregion
}
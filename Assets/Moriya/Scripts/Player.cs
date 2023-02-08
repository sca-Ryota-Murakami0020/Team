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
    private float jumpRollingSpeed = 3.0f;
    //ジャンプした時のx方向スピード
    private float jumpingRunSpeed = 2.5f;
    //壁ジャンプした時のプレイヤーのスピード
    private float wallJumpRunSpeed = 3.0f;
    //ジャンプした時のy方向スピード
    private float jumpSpeed = 10.0f;
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
    //落下ダメージが入ったフラグ
    private bool fallDamageFlag = true;
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
    //落下中に壁ジャンプ地点に触れたら
    private bool wallJumpHitFlag = false;
    //壁ジャン用フラグ
    private bool wallJumpFlag = false;
    //壁ジャンプをしたフラグ
    private bool wallJumpDidFlag = false;
    //ゲームオーバー
    private bool gameOverFlag = false;
    //カメラに移動・向き反転かつ親オブジェクトに壁張り付き
    private bool doInputButtonFlag = false;
    //壁にいる判定
    private bool doStayWall = false;

    #endregion

    //　レイを飛ばす場所
    [SerializeField]
    private Transform rayPosition;
    //　レイを飛ばす距離
    [SerializeField]
    private float rayRange;
    //　落ちたy座標
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
    private float accelSpeed = 7.5f;
    //加速リセットする際に使う変数
    private float defaultSpeed = 5.0f;

    //RigidBodyとボックスコライダーの定義
    private Rigidbody rb;
    private BoxCollider bc;

    //GMとポーズ画面関係のスプリクトの定義
    private totalGameManager gm;
    private PasueDisplayC pasueDisplayC;
    private PlayerWallCon pWallC;

    //　Time.timeScaleに設定する値
    [SerializeField]
    private float timeScale = 0.1f;
    //　時間を遅くしている時間
    private float slowTime = 2f;
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
    private AudioSource audios = null;
    [SerializeField]
    private AudioClip jumpSE;
    [SerializeField]
    private AudioClip randingSE;
    [SerializeField]
    private AudioClip damegeSE;
    [SerializeField]
    private AudioClip runSE;//移動用
    [SerializeField]
    private AudioClip accelSE;//加速用
    //効果音がなったら
    private bool soundFlag = true;

    //コルーチン戻り値用
    private Coroutine lineCast;


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
        get { return this.jumpCount; }
        set { this.jumpCount = value; }
    }

    public bool RollingJumpFlag
    {
        get { return this.RollingJumpFlag; }
        set { this.RollingJumpFlag = value; }
    }

    public GameObject[] HeartArray
    {
        get { return this.heartArray; }
        set { this.heartArray = value; }
    }

    public bool DoInputButtonFlag
    {
        get { return this.doInputButtonFlag; }
        set { this.doInputButtonFlag = value; }
    }

    #endregion

    private void Awake()
    {
        audios = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //コンポーネント
        rb = GetComponent<Rigidbody>();
        bc = GetComponent<BoxCollider>();
        gm = FindObjectOfType<totalGameManager>();
        pasueDisplayC = FindObjectOfType<PasueDisplayC>();
        pWallC = FindObjectOfType<PlayerWallCon>();
        this.anime = GetComponent<Animator>();

        //hp初期化
        oldHp = gm.PlayerHp;

        //親オブジェクト取得
        _parent = transform.root.gameObject;

        //子オブジェクト取得
        child = transform.GetChild(2).gameObject;

        //アニメーション初期化
        anime.SetBool("doIdle", true);

        //落ちた時に使う数値リセット
        fallenDistance = 0f;
        fallenPosition = transform.position.y;
        fallFlag = false;

        //ray投射開始
        lineCast = StartCoroutine("StartLineCast");
    }

    // Update is called once per frame
    void Update()
    {
        //プレイヤーhp表示をするためのfor文
        for (int i = 0; i < gm.PlayerHp; i++)
        {
            heartArray[i].gameObject.SetActive(true);
        }

        Debug.DrawLine(rayPosition.position, rayPosition.position + Vector3.down * rayRange, Color.red, 1.0f);

        // ステートがダメージならリターン
        if (state == STATE.DAMAGED)
        {
            return;
        }

        //カメラの角度取得と単位ベクトル化
        mainCameraForwardDer = mainCamera.transform.forward.normalized;
        mainCameraRightDer = mainCamera.transform.right.normalized;
        Vector3 cameraDreNoY = new Vector3(mainCameraForwardDer.x, 0, mainCameraForwardDer.z);
        cameraDreNoY = cameraDreNoY.normalized;

        //何もなかったら待機モーション
        //anime.SetBool("doIdle", true);

        //hpが減った時の処理
        //hpが0まではここ
        if (gm.PlayerHp < oldHp && gm.PlayerHp >= 1)
        {
            //アニメーション＆効果音流し
            PlaySE(damegeSE);
            //Hp表示と点滅表示
            HpDisplay();
            oldHp = gm.PlayerHp;
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

        //　落ちている状態
        //落下中の処理(ほぼアニメーション)
        FallAnime();

        //落下中に壁ジャン地点に当たったら
        if (doStayWall == true)
        {
            //Debug.Log("ここに入ったよ");
            jumpCount = 0;
            rollingJumpFlag = false;
            //左クリックおされたら
            if (Input.GetMouseButton(1))
            {
                anime.SetTrigger("LiftWall");
                //重力を作用させる
                rb.useGravity = true;
                //壁ジャンプできるフラグをおる
                wallJumpFlag = false;
                //カメラに渡すフラグをおる
                doInputButtonFlag = false;
                //壁はりつき中のフラグをおる
                doStayWall = false;
                //壁から離れたのでフラグをおる
                pWallC.WallJumpHitFlag = false;
            }
        }

        //アニメーションしたら加速
        if (speedAccelerationFlag == true)
        {
            //Debug.Log("加速したよ");
            //制限時間計算と速度変換
            speedCTime++;
            runSpeed = accelSpeed;
            if (speedTime < speedCTime)
            {
                //Debug.Log("加速終わったよ");
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
        if (Input.GetKey(KeyCode.A) && doInputButtonFlag == false)
        {
            moveFlag = true;

            //落下中ではなければ歩きモーションをたてる
            if (fallFlag == false)
            {
                anime.SetBool("doIdle", false);
                anime.SetBool("doWalk", true);
            }

            //普通の歩くスピード
            if (jumpFlag == false && rollingJumpDidFlag == false && wallJumpDidFlag == false)
            {
                _parent.transform.position -= mainCameraRightDer * runSpeed * Time.deltaTime;
            }

            //通常ジャンプと壁ジャンプのスピード
            if (jumpFlag == true)
            {
                _parent.transform.position -= mainCameraRightDer * jumpingRunSpeed * Time.deltaTime;
            }

            //ローロングジャンプのスピード
            if (rollingJumpDidFlag == true)
            {
                _parent.transform.position -= mainCameraRightDer * jumpRollingSpeed * Time.deltaTime;
            }

            if (wallJumpDidFlag == true)
            {
                _parent.transform.position -= mainCameraRightDer * wallJumpRunSpeed * Time.deltaTime;
            }

            transform.rotation = Quaternion.LookRotation(-mainCameraRightDer);
        }

        //右方向に向いて移動したら
        if (Input.GetKey(KeyCode.D) && doInputButtonFlag == false)
        {
            moveFlag = true;
            if (fallFlag == false)
            {
                anime.SetBool("doIdle", false);
                anime.SetBool("doWalk", true);
            }
            //普通の歩くスピード
            if (jumpFlag == false && rollingJumpDidFlag == false && wallJumpDidFlag == false)
            {
                _parent.transform.position += mainCameraRightDer * runSpeed * Time.deltaTime;
            }

            //通常ジャンプと壁ジャンプのスピード
            if (jumpFlag == true)
            {
                _parent.transform.position += mainCameraRightDer * jumpingRunSpeed * Time.deltaTime;
            }

            if (rollingJumpDidFlag == true)
            {
                _parent.transform.position += mainCameraRightDer * jumpRollingSpeed * Time.deltaTime;
            }

            if (wallJumpDidFlag == true)
            {
                _parent.transform.position += mainCameraRightDer * wallJumpRunSpeed * Time.deltaTime;
            }

            transform.rotation = Quaternion.LookRotation(mainCameraRightDer);
        }

        //上方向に向いて移動したら
        if (Input.GetKey(KeyCode.W) && doInputButtonFlag == false)
        {
            moveFlag = true;
            if (fallFlag == false)
            {
                anime.SetBool("doIdle", false);
                anime.SetBool("doWalk", true);
            }

            //普通の歩くスピード
            if (jumpFlag == false && rollingJumpDidFlag == false && wallJumpDidFlag == false)
            {
                _parent.transform.position += cameraDreNoY * runSpeed * Time.deltaTime;

            }

            //通常ジャンプと壁ジャンプのスピード
            if (jumpFlag == true)
            {
                _parent.transform.position += cameraDreNoY * jumpingRunSpeed * Time.deltaTime;
            }

            if (rollingJumpDidFlag == true)
            {
                _parent.transform.position += cameraDreNoY * jumpRollingSpeed * Time.deltaTime;
            }

            if (wallJumpDidFlag == true)
            {
                _parent.transform.position += cameraDreNoY * wallJumpRunSpeed * Time.deltaTime;
            }

            transform.rotation = Quaternion.LookRotation(cameraDreNoY);
        }

        //下方向に向いて移動したら
        if (Input.GetKey(KeyCode.S) && doInputButtonFlag == false)
        {
            moveFlag = true;

            if (fallFlag == false)
            {
                anime.SetBool("doIdle", false);
                anime.SetBool("doWalk", true);
            }
            //普通の歩くスピード
            if (jumpFlag == false && rollingJumpDidFlag == false && wallJumpDidFlag == false)
            {
                _parent.transform.position -= cameraDreNoY * runSpeed * Time.deltaTime;
            }

            //通常ジャンプと壁ジャンプのスピード
            if (jumpFlag == true)
            {
                _parent.transform.position -= cameraDreNoY * jumpingRunSpeed * Time.deltaTime;
            }

            if (rollingJumpDidFlag == true)
            {
                _parent.transform.position -= cameraDreNoY * jumpRollingSpeed * Time.deltaTime;
            }

            if (wallJumpDidFlag == true)
            {
                _parent.transform.position -= cameraDreNoY * wallJumpRunSpeed * Time.deltaTime;
            }

            transform.rotation = Quaternion.LookRotation(-cameraDreNoY);
        }

        //方向キーが押されていなければここに入る
        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
        {
            moveFlag = false;
            rb.angularVelocity = Vector3.zero;
            if (doStayWall == false)
            {
                if (fallFlag == false)
                {
                    //落下中でなければ待機モーションに入る
                    anime.SetBool("doIdle", true);
                    anime.SetBool("doWalk", false);
                }
            }
        }

        //ジャンプの動き
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount == 0 && fallFlag == false)
        {
            //ジャンプの効果音を流す
            PlaySE(jumpSE);

            //ローロングジャンプができない状態なら
            if (rollingJumpFlag == false && wallJumpFlag == false)
            {
                //ジャンプ時
                anime.SetBool("doJump", true);
                this.rb.AddForce(new Vector3(0, jumpSpeed * 30, 0));
                jumpFlag = true;
                jumpCount++;
            }

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

            //壁ジャンプができるなら
            if (wallJumpFlag == true)
            {
                //壁ジャンプしたフラグとアニメーション関係
                wallJumpDidFlag = true;
                anime.SetTrigger("DoWallJump");
                this.rb.AddForce(new Vector3(0, jumpSpeed * 40, 0));

                jumpCount++;

                //重力を作用させる
                rb.useGravity = true;
                //壁ジャンプできるフラグをおる
                wallJumpFlag = false;
                //カメラに渡すフラグをおる
                doInputButtonFlag = false;
                //加速フラグをたてる
                speedAccelerationFlag = true;
                //壁はりつき中のフラグをおる
                doStayWall = false;
                //壁から離れたのでフラグをおる......
                pWallC.WallJumpHitFlag = false;
            }

        }

        #endregion
    }

    //isTriggerがついてない時の判定
    private void OnCollisionEnter(Collision other)
    {
        //敵に当たったら
        if (other.gameObject.CompareTag("Enemy"))
        {
            gm.PlayerHp--;
            anime.SetTrigger("domazeed");
        }

        //地面に当たったら
        if (other.gameObject.CompareTag("Ground"))
        {
            rollingJumpFlag = false;
            pWallC.WallJumpHitFlag = false;
            jumpCount = 0;
        }

        //ローリングジャンプポイントに当たったら
        if (other.gameObject.CompareTag("RollingJumpPoint"))
        {
            rollingJumpFlag = true;
            pWallC.WallJumpHitFlag = false;
            jumpCount = 0;
        }
    }

    //isTriggerがついている時の処理
    private void OnTriggerEnter(Collider other)
    {
        //アイテムに当たったら
        if (other.gameObject.CompareTag("Item"))
        {
            gm.PlayerIC++;
            other.gameObject.SetActive(false);
        }

        //シーン移動
        if (other.gameObject.name == "goalPoint")
        {
            gm.LoadGameClear();
            SceneManager.LoadScene("GoalScene");
        }

        if (other.gameObject.name == "LoadFirstStagePoint" && gm.PlayerIC >= 1)
        {
            SceneManager.LoadScene("LoadFirstStage");
        }

        if (other.gameObject.name == "LoadSecondPoint" && gm.PlayerIC >= 2)
        {
            //二個目のロードシーンにもちこむをかく
            SceneManager.LoadScene("LoadSecondStage");
        }

        if (other.gameObject.name == "LoadTherdPoint" && gm.PlayerIC >= 3)
        {
            //三個目のロードシーンにもちこむやつをかく
            SceneManager.LoadScene("LoadTherdStage");//ロードシーンの名前を書く;
        }

        if (other.gameObject.name == "LoadLastPoint")
        {
            SceneManager.LoadScene("LoadLastStage");
        }
    }


    //Hp減った時の処理
    private void HpDisplay()
    {
        heartArray[gm.PlayerHp].gameObject.SetActive(false);
    }

    //効果音を流す処理
    public void PlaySE(AudioClip clip)
    {
        if (audios != null)
        {
            audios.PlayOneShot(clip);
        }
        else
        {
        }
    }

    //走るときの効果音の処理
    public void ActiveWalkSE()
    {
        if (speedAccelerationFlag == true)
        {
            PlaySE(accelSE);
        }
        if (speedAccelerationFlag == false)
        {
            PlaySE(runSE);
        }

    }

    //地面にふれた時のアニメーション関係
    public void FallAnime()
    {
        if (fallFlag)
        {
            //アニメーション関係の初期化
            ResetAnime();

            //壁ジャンプできる壁に触れたらここに入る
            if (pWallC.WallJumpHitFlag)
            {
                ClingToWall();
            }

            //落下地点と現在地の距離を計算（ジャンプ等で上に飛んで落下した場合を考慮する為の処理）
            //落下地点 = 落下地点かプレイヤーの落下地点の最大値
            fallenPosition = Mathf.Max(fallenPosition, transform.position.y);
            RaycastHit hit;
            //レイが非null = レイを飛ばす状況なら
            if (lineCast != null)
            {
                //　地面にレイが届いていたら
                if (Physics.Linecast(rayPosition.position, rayPosition.position + Vector3.down * rayRange, out hit, LayerMask.GetMask("Ground")))
                {
                    //　落下距離を計算
                    fallenDistance = fallenPosition - transform.position.y;

                    if (fallenDistance >= takeDamageDistance)
                    {

                        //アニメーションフラグをおる
                        anime.SetBool("doFall", false);
                        //スローモーション移行
                        StartCoroutine("StartSlowmotion");

                        //ダメージが入るフラグが立っていない時
                        //Eキーが押されたらこの中に入る
                        if (fallDamageHitFlag == false)
                        {
                            //ローロング着地のフラグを立てる
                            anime.SetBool("doLandRolling", true);
                            //地面に着地したら
                            if (hit.transform.gameObject.CompareTag("Ground"))
                            {
                                GroundAnime();
                            }

                            //ローロングジャンプポイントに着地したら
                            if (hit.transform.gameObject.CompareTag("RollingJumpPoint"))
                            {
                                RollingPointAnime();
                            }

                            //加速するフラグをたてる
                            speedAccelerationFlag = true;
                        }
                        //ダメージが入るフラグがたった時
                        //Eキーが押されなかった時に入る
                        if (fallDamageHitFlag == true)
                        {
                            //プレイヤーのHp減少・プレイヤー点滅処理・フラグを折る
                            gm.PlayerHp--;
                            fallDamageHitFlag = false;
                            state = STATE.DAMAGED;
                            StartCoroutine(_hit());
                            anime.SetBool("doLanding", true);

                            //地面に着地したら
                            if (hit.transform.gameObject.CompareTag("Ground"))
                            {
                                GroundAnime();
                            }

                            //ローロングジャンプポイントに着地したら
                            if (hit.transform.gameObject.CompareTag("RollingJumpPoint"))
                            {
                                RollingPointAnime();
                            }
                        }
                    }
                    //ダメージなしの着地
                    if (fallenDistance <= takeDamageDistance)
                    {
                        //通常着地モーションをする
                        anime.SetBool("doFall", false);
                        anime.SetBool("doLanding", true);

                        //上と同じくしている
                        if (hit.transform.gameObject.CompareTag("Ground"))
                        {
                            GroundAnime();
                        }

                        //上と同じ
                        if (hit.transform.gameObject.CompareTag("RollingJumpPoint"))
                        {
                            RollingPointAnime();
                        }
                    }
                    //ここでフラグおり＆着地の効果音を入れている
                    fallFlag = false;
                    PlaySE(randingSE);
                }
            }
        }
        else
        {
            //レイを飛ばせる状況にあるとき
            //fallFlagがfalse状態でかつプレイヤーが地面から離れた時にfallFlagをtrueにする
            //壁にはりついていない時かつ地面にレイが届いていなければ落下地点を設定
            if (lineCast != null)
            {
                //レイが届かないなら
                if (!Physics.Linecast(rayPosition.position, rayPosition.position + Vector3.down * rayRange, LayerMask.GetMask("Ground")) && doStayWall == false)
                {
                    //地面から一回でもLineCastの線が離れたとき = 落下状態とする
                    //その時に落下状態を判別するためfallFlagをtrueにする
                    //最初の落下地点を設定
                    fallenPosition = transform.position.y;
                    fallenDistance = 0;
                    //フラグを立てる
                    fallFlag = true;
                    //Debug.Log("地面から離れたよ");
                }
            }
        }
    }

    //アニメーションのリセット
    private void ResetAnime()
    {
        //ローリング着地に関わる値をリセット
        anime.SetBool("doFall", true);
        anime.SetBool("doLandRolling", false);
        anime.SetBool("doLanding", false);

        //ジャンプやローリングジャンプしなかった時にここに入る
        if (rollingJumpDidFlag == false && jumpFlag == false && moveFlag == true)
        {
            anime.SetBool("doWalk", true);
            anime.SetBool("doJump", false);
        }

        //ジャンプしたらここに入る
        if (jumpFlag == true)
        {
            anime.SetBool("doWalk", true);
            anime.SetBool("RollingAriIdle", false);
        }

        //ローリングジャンプした時にここに入る
        if (rollingJumpDidFlag == true)
        {
            anime.SetBool("doWalk", false);
            anime.SetBool("doJump", false);
            anime.SetBool("RollingAriIdle", true);
        }

        //壁ジャンプ中ならここに入る
        if (wallJumpDidFlag == true)
        {
            anime.SetBool("doWalk", false);
            anime.SetBool("doJump", false);
            anime.SetBool("RollingAriIdle", true);
        }

    }

    //壁はりつきの関数
    private void ClingToWall()
    {
        //左マウスボタンが押されていたら
        if (Input.GetMouseButton(0))
        {
            wallJumpFlag = true;
            //プレイヤーの座標固定＆向き反転
            doInputButtonFlag = true;
            rb.useGravity = false;
            rb.velocity = new Vector3(0, 0f, 0);
            //壁に張り付いてるフラグを立てる
            doStayWall = true;
            //アニメーション関係
            anime.SetTrigger("WallJumpHit");
            StartCoroutine("StartRotate");
            //普通のジャンプをしていたら
            if (jumpFlag == true)
            {
                jumpFlag = false;
                anime.SetBool("doJump", false);
            }

            //ローリングジャンプをしていたら
            if (rollingJumpDidFlag == true)
            {
                //ローロング空中待機モーションから待機モーションへ
                anime.SetBool("RollingAriIdle", false);
                rollingJumpDidFlag = false;
            }

            //壁ジャンをしていたら
            if (wallJumpDidFlag == true)
            {
                wallJumpDidFlag = false;
            }

            //フラグ関係
            PlaySE(randingSE);
            fallFlag = false;
        }
    }

    //地面判定に触れた時のアニメ処理
    private void GroundAnime()
    {
        //普通のジャンプをしていたら
        if (jumpFlag == true)
        {
            jumpFlag = false;
            anime.SetBool("doJump", false);
            //着地モーションから待機モーションへ
            if (jumpFlag == false)
            {
                anime.SetBool("doIdle", true);
            }
        }

        //ローリングジャンプをしていたら
        if (rollingJumpDidFlag == true)
        {
            //ローロング空中待機モーションから待機モーションへ
            anime.SetBool("RollingAriIdle", false);
            anime.SetBool("doIdle", true);
            rollingJumpDidFlag = false;
        }

        //壁ジャンしていたら
        if (wallJumpDidFlag == true)
        {
            anime.SetBool("RollingAriIdle", false);
            anime.SetBool("doIdle", true);
            wallJumpDidFlag = false;
        }
    }

    //ローリングジャンプ地点に触れた時のアニメ処理
    private void RollingPointAnime()
    {
        //普通のジャンプをしていたら
        if (jumpFlag == true)
        {
            jumpFlag = false;
            anime.SetBool("doJump", false);
            //着地モーションから待機モーションへ
            if (jumpFlag == false)
            {
                anime.SetBool("doIdle", true);
                rollingJumpFlag = true;
            }
        }
        //ローリングジャンプをしていたら
        if (rollingJumpDidFlag == true)
        {
            // //ローロング空中待機モーションから待機モーションへ
            anime.SetBool("RollingAriIdle", false);
            anime.SetBool("doIdle", true);
            rollingJumpFlag = true;
            rollingJumpDidFlag = false;
        }

        //壁ジャンしていたら
        if (wallJumpDidFlag == true)
        {
            anime.SetBool("RollingAriIdle", false);
            anime.SetBool("doIdle", true);
            wallJumpDidFlag = false;
        }
    }

    #region//コルーチン
    //レイを投射するコルーチン
    private IEnumerator StartLineCast()
    {
        while (true)
        {
            if (fallFlag == true)
            {
                break;
            }
            if (fallFlag == false)
            {
                yield return null;
            }
        }
    }

    //スローモーションの元コルーチン
    private IEnumerator StartSlowmotion()
    {
        //レイは飛ばさない
        lineCast = null;

        //遅くする
        Time.timeScale = timeScale;

        //スローモーション中に一回でも右クリックが押されたかどうかの判定
        bool isClicked = false;

        //時間計測＋キー判定
        while (elapsedTime < slowTime)
        {
            //1秒いないならスローモーションにする
            Time.timeScale = timeScale;
            //スローモーションの制限時間用
            elapsedTime += Time.unscaledDeltaTime;
            //　落下によるダメージが発生する距離を超える場合に右クリックが押されているかどうか
            if (Input.GetMouseButton(1) == true)
            {
                isClicked = true;
            }
            //スローモーション解除
            if (elapsedTime > slowTime)
            {
                //右クリックされていなければ，ダメージ判定をさせる
                if (!isClicked) { fallDamageHitFlag = true; }
                //Debug.Log("とけた");
                Time.timeScale = 1f;
                elapsedTime = 0.0f;
                lineCast = StartCoroutine(StartLineCast());//レイ復活
                break;
            }
            yield return null;
        };

        //slowmotion本体をストップ
        StopCoroutine("StartSlowmotion");
    }

    //ダメージが入った時の点滅処理
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

    //ゲームオーバー時の処理
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

    //回転処理
    private IEnumerator StartRotate()
    {
        while (true)
        {
            this.transform.Rotate(0, 180.0f, 0);
            yield return null;
            break;
        }
    }

    #endregion
}
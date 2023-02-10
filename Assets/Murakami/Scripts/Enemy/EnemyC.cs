using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EnemyC : MonoBehaviour
{
    //RigidBody
    private Rigidbody rb;

    #region//ステータス
    //移動速度
    private float enemySpeed = 0.7f;
    //移動距離
    [SerializeField] private float limitDistance;
    //追跡中使うスピード
    private float addSpeed = 3.0f;
    //回転回数
    private int rotateCounter = 0;
    //透明化時間（サイズを0にする時間）
    private int invisibleTime = 0;
    //消去した後に元のサイズに戻すために使う変数
    private Vector3 defSize;
    #endregion

    #region//フラグ
    //追跡フラグ
    private bool doEncount = false;
    //回転中かの判定
    private bool doTurn = false;
    //最初に振り向く処理を向こうにする
    private bool noCountFlag = true;
    //消滅処理中
    private bool isInvisible = false;
    #endregion

    #region//プレイ中に変化するEnemyの位置・旋回情報
    //ray関係
    private float rayDistance = 2.0f;
    //rayを飛ばすオブジェクト
    [SerializeField] private GameObject shotRayPosition;
    //回転の軸を設定するために必要
    private Quaternion defaultRotation;
    //リセット時のポジション
    private Vector3 defaultPosition;
    //消滅処理の際にEnemyを戻す位置
    private Vector3 trueDefaultPosition;
    //消去処理後のEnemyの角度
    private Quaternion tDQ;
    #endregion

    //敵の回転する方向
    enum RotationPar
    {
        NULL,
        RIGHT,
        LEFT,
        RESET,
        TURN,
    };
    
    //パラメータ
    RotationPar rotationState;

    #region//プロパティ
    public bool DoEn
    {
        get { return this.doEncount;}
        set { this.doEncount = value;}
    }
    #endregion

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        //Enemyのステータス関係の初期化
        rotationState = RotationPar.NULL;
        defaultPosition = this.transform.position;
        trueDefaultPosition = this.transform.position;
        tDQ = this.transform.rotation;
        defSize = this.transform.localScale;
    }

    void Update()
    {
        //プレイヤーの索敵
        if(isInvisible == false)
        {
            SearchPlayer();
        }

        //速度関係
        //Playerを発見したら
        if (this.doEncount)
        {
            this.transform.position += transform.forward * addSpeed * Time.deltaTime;
        }

        //巡回行動
        //旋回位置に着くまでの処理
        if (doTurn == false && isInvisible == false && this.doEncount == false)
        {
            MoveEnemy();
        }
    }

    //接触判定
    private void OnCollisionEnter(Collision collision)
    {
        //壁にぶつかったら
        if ((collision.gameObject.CompareTag("Wall") ||
            collision.gameObject.CompareTag("WallJumpPoint") || 
            collision.gameObject.CompareTag("LimitWall") || 
            collision.gameObject.CompareTag("OutSidePoint") ||
            collision.gameObject.CompareTag("Ground")) && 
            doEncount == true)
        {

            //敵がオブジェクトに当たった時に10秒後に元の場所、元の向きに
            //戻してあげる
            ReSetEnemy();
        }
    }

    #region//関数関係

    //Rayを用いたプレイヤーの索敵
    public void SearchPlayer()
    {
        //ここで進行先のPlayerを感知する
        Vector3 rayPosition = shotRayPosition.transform.position;
        RaycastHit hit;
        Ray ray = new Ray(rayPosition, this.gameObject.transform.forward);
        Debug.DrawRay(shotRayPosition.transform.position, shotRayPosition.transform.forward * this.rayDistance, Color.red, 1.0f);
        if (Physics.Raycast(ray, out hit, 100.0f))
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                this.doEncount = true;
            }
        }
    }

    //巡回行動
    public void MoveEnemy()
    {
        //ここでEnemyを巡回行動をさせる
        this.transform.position += transform.forward * enemySpeed * Time.deltaTime;
        Vector3 enamyPasoion = this.transform.position;
        //往復点についたら一旦停止する
        //条件が4つあるのは敵の動く方向が様々だから
        if (enamyPasoion.z >= defaultPosition.z + limitDistance || 
            enamyPasoion.x >= defaultPosition.x + limitDistance ||
            enamyPasoion.z <= defaultPosition.z - limitDistance ||
            enamyPasoion.x <= defaultPosition.x - limitDistance)
        {
            //旋回処理
            returnLookPosition();
        }
    }

    //旋回処理
    public void returnLookPosition()
    {
        //現在地（旋回位置）を次の出発地点にする
        defaultPosition = this.transform.position;
        //次の終点位置の方向に向けるようにするために値を変更する
        this.defaultRotation = this.transform.rotation;
        //旋回中にする
        this.doTurn = true;
        //旋回する方向をステータスで管理しているのでここで回転方向を決める
        this.rotationState = RotationPar.RIGHT;
        //各方向に旋回するコルーチン
        StartCoroutine("TurnLookPosition");
    }

    //Enemyを消去する演出を行う
    private void ReSetEnemy()
    {
        //位置の初期化
        this.transform.position = trueDefaultPosition;
        //向きの初期化
        this.transform.rotation = tDQ;
        //プレイヤーを見つけた判定をリセットする
        doEncount = false;
        //透明化
        isInvisible = true;
        //最初にブロックに触れても消えない様にする
        noCountFlag = true;
        //コルーチン開始
        StartCoroutine("ResetEnemy");
    }
    #endregion

    #region//コルーチン
    //リセット機能
    private IEnumerator ResetEnemy()
    {
        //１０秒間Enemyを縮小する処理
        while(invisibleTime <= 10)
        {
            //Enemyのサイズを０にする
            this.transform.localScale = Vector3.zero;
            //１秒間待機する
            yield return new WaitForSeconds(1.0f);
            //コルーチン内でのTime関数は精確では無かったのでint型の変数を用いて秒数を計測した
            invisibleTime++;
        }
        //１０秒後にEnemyのサイズを元に戻す
        this.transform.localScale = defSize;
        //透明化の処理が終了したのでフラグをfalseにする
        isInvisible = false;
        //次のリセット処理のためにここで計測に用いた変数を初期化する
        invisibleTime = 0;
        yield break;
    }

    //旋回処理
    private IEnumerator TurnLookPosition()
    {
        //旋回前に少し待つ
        yield return new WaitForSeconds(1.5f);

        while (rotationState == RotationPar.RIGHT)
        {
             
            //一度ずつ右方向に回転
            this.transform.Rotate(0, 1.0f, 0);
            //傾きを計算する
            rotateCounter++;

            //2度までは少し緩やかに回転
            if (rotateCounter < 3) yield return new WaitForSeconds(0.1f);
            //3度から39度までは一定の回転速度で回転
            if (rotateCounter >= 3 && rotateCounter < 40) yield return new WaitForSeconds(0.01f);
            //40度以降は少し緩やかに回転する
            if (rotateCounter >= 40) yield return new WaitForSeconds(0.075f);
            //回転した度数が45度を超えたら
            if (rotateCounter >= 45)
            {
                //回転状態のステータスを「左方向」に変更
                rotationState = RotationPar.LEFT;
                //計算した角度数をリセット
                rotateCounter = 0;
                //1.3秒待機
                yield return new WaitForSeconds(1.3f);
                break;
            }
        }

        //左方向に回転
        while (rotationState == RotationPar.LEFT)
        {
            //1度ずつ左方向に回転する
            this.transform.Rotate(0, -1.0f, 0);
            //傾いた角度を計算する
            rotateCounter++;

            //2度までは少し緩やかに回転
            if (rotateCounter < 3) yield return new WaitForSeconds(0.1f);
            //3度から85度までは一定の回転速度で回転する
            if (rotateCounter >= 3 && rotateCounter < 85) yield return new WaitForSeconds(0.01f);
            //85度以降は少し緩やかに回転する
            if (rotateCounter >= 85) yield return new WaitForSeconds(0.075f);
            //回転した角度が90度を超えたら
            if (rotateCounter >= 90)
            {
                //回転状態のステータスを「リセット」に変更する
                rotationState = RotationPar.RESET;
                //計算した角度数をリセットする
                rotateCounter = 0;
                //1.3秒待機
                yield return new WaitForSeconds(1.3f);
                break;
            }
        }

        //正位置に戻る
        while (rotationState == RotationPar.RESET)
        {
            //1度ずつ回転する
            this.transform.Rotate(0, 1.0f, 0);
            //傾いた角度を計算する
            rotateCounter++;

            //2度までは少し緩やかに回転する
            if (rotateCounter < 3) yield return new WaitForSeconds(0.1f);
            //3度から39度までは一定の回転速度で回転する
            if (rotateCounter >= 3 && rotateCounter < 40) yield return new WaitForSeconds(0.01f);
            //40度以降は少し緩やかに回転する
            if (rotateCounter >= 40) yield return new WaitForSeconds(0.075f);
            //回転した角度が45度を超えたら
            if (rotateCounter >= 45)
            {
                //回転状態のステータスを「逆方向に向く」にする
                rotationState = RotationPar.TURN;
                //計算した角度数をリセットする
                rotateCounter = 0;
                //1.3秒待機
                yield return new WaitForSeconds(1.3f);
                break;
            }
        }

        //次の終点の方向に旋回
        while (rotationState == RotationPar.TURN)
        {
            //1度ずつ回転する
            this.transform.Rotate(0, 1.0f, 0);
            //傾いた角度を計算する
            rotateCounter++;

            //2度までは少し緩やかに回転する
            if (rotateCounter < 3) yield return new WaitForSeconds(0.01f);
            //3度から175度までは一定の速度で回転する
            if (rotateCounter >= 3 && rotateCounter <= 175) yield return new WaitForSeconds(0.00075f);
            //175度以降は少し緩やかに回転する
            if (rotateCounter >= 175) yield return new WaitForSeconds(0.075f);
            //回転した角度が180度を超えたら
            if (rotateCounter >= 180)
            {
                //計算した角度数をリセットする
                rotateCounter = 0;
                //1.3秒待機
                yield return new WaitForSeconds(1.3f);
                break;
            }
        }

        //ここで旋回処理が終了するのでfalseにする
        doTurn = false;
        //回転状態のステータスを初期化
        rotationState = RotationPar.NULL;
        //リセット処理をした際にブロックに触れても消えないフラグがtrueならfalseにする
        if (noCountFlag == true)
        {
            noCountFlag = false;
        }
        yield break;
    }
    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EnemyC : MonoBehaviour
{
    //RigidBody
    private Rigidbody rb;

    //移動速度
    [SerializeField] private float enemySpeed = 0.7f;
    //巡回距離
    [SerializeField] private float limitDistance;
    //ノックバック速度
    private float speed = 1.5f;
    //追跡中使うスピード
    private float addSpeed = 3.0f;
    //回転回数
    private int rotateCounter = 0;
    //回転時間
    private float rotateTime = 0.0f;
    //走行距離
    private float distance = 0.0f;
    //回転した距離
    private float rotationDistance = 0.0f;
    //透明化時間
    private int invisibleTime = 0;
    //
    private Vector3 defSize;

    //追跡フラグ
    private bool doEncount = false;
    //回転中かの判定
    private bool doTurn = false;
    //最初に振り向く処理を向こうにする
    private bool noCountFlag = true;
    //消滅処理中
    private bool isInvisible = false;

    //位置
    private Vector3 pos;
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

    //敵の回転する方向
    enum RotationPar
    {
        NULL,
        RIGHT,
        LEFT,
        RESET,
        TURN,
    };
    
    RotationPar rotationState;

    #region//プロパティ
    public bool DoEn
    {
        get { return this.doEncount;}
        set { this.doEncount = value;}
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        rotationState = RotationPar.NULL;
        defaultPosition = this.transform.position;
        trueDefaultPosition = this.transform.position;
        tDQ = this.transform.rotation;
        defSize = this.transform.localScale;
    }

    // Update is called once per frameshotRayPosition.transform.position, 
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

            //生成処理開始
            //rSE.SponeEnemy();
            //this.gameObject.SetActive(false);
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
        //distance += 0.001f;
        //this.transform.position = new Vector3(0, 0, Mathf.Sin(distance) * limitDistance * enemySpeed);
        //往復点についたら一旦停止するdistance >= 0.25f
        if (enamyPasoion.z >= defaultPosition.z + limitDistance || 
            enamyPasoion.x >= defaultPosition.x + limitDistance ||
            enamyPasoion.z <= defaultPosition.z - limitDistance ||
            enamyPasoion.x <= defaultPosition.x - limitDistance)
        {
            //distance = 0.0f;
            //Debug.Log("一旦停止");
            returnLookPosition();
        }
    }

    //旋回処理
    public void returnLookPosition()
    {
        //旋回位置を次の出発地点にする
        defaultPosition = this.transform.position;
        //次の終点位置の方向に向けるようにするために値を変更する
        this.defaultRotation = this.transform.rotation;
        //旋回中にする
        this.doTurn = true;
        //旋回する方向をステータスで管理しているのでここで回転方向を決める
        this.rotationState = RotationPar.RIGHT;
        //Debug.Log("旋回開始");
        //各方向に旋回するコルーチン
        StartCoroutine("TurnLookPosition");
    }

    //
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
        Debug.Log("透明化処理開始");
        StartCoroutine("ResetEnemy");
    }
    #endregion

    #region//コルーチン
    //リセット機能
    private IEnumerator ResetEnemy()
    {
        Debug.Log("透明化処理中");
        while(invisibleTime <= 10)
        {
            this.transform.localScale = Vector3.zero;
            yield return new WaitForSeconds(1.0f);
            invisibleTime++;
        }
        Debug.Log("透明化処理終了");
        this.transform.localScale = defSize;
        isInvisible = false;
        invisibleTime = 0;
        yield break;
    }

    private IEnumerator TurnLookPosition()
    {
        //旋回前に少し待つ
        yield return new WaitForSeconds(1.5f);

        while (rotationState == RotationPar.RIGHT)
        {
             
            //一度ずつ回転
            this.transform.Rotate(0, 1.0f, 0);
            rotateCounter++;

            //始めは少し緩やかに回転
            if (rotateCounter < 3) yield return new WaitForSeconds(0.1f);
            //f痛の回転速度で回転
            if (rotateCounter >= 3 && rotateCounter < 40) yield return new WaitForSeconds(0.01f);
            //最後は少し緩やかに回転する
            if (rotateCounter >= 40) yield return new WaitForSeconds(0.075f);
                        //回転した度数が45°を超えたら
            if (rotateCounter >= 45)
            {
                rotationState = RotationPar.LEFT;
                rotateCounter = 0;
                yield return new WaitForSeconds(1);
                break;
            }
            /*
            Debug.Log("右旋回中");

            rotateTime += 0.001f;
            this.transform.Rotate(new Vector3(0, Mathf.Sin(rotateTime), 0));

            if (rotateTime >= 0.125f)
            {
                rotationState = RotationPar.LEFT;
                rotateTime = 0.0f;
                yield return new WaitForSeconds(1);
                break;
            }*/
        }

        //左方向に回転
        while (rotationState == RotationPar.LEFT)
        {
            
            this.transform.Rotate(0, -1.0f, 0);
            rotateCounter++;

            if (rotateCounter < 3) yield return new WaitForSeconds(0.1f);

            if (rotateCounter >= 3 && rotateCounter < 85) yield return new WaitForSeconds(0.01f);

            if (rotateCounter >= 85) yield return new WaitForSeconds(0.075f);

            if (rotateCounter >= 90)
            {
                rotationState = RotationPar.RESET;
                rotateCounter = 0;
                yield return new WaitForSeconds(1);
                break;
            }
            //Debug.Log("左旋回中");
            /*
            rotateTime += 0.001f;
            this.transform.Rotate(new Vector3(0, Mathf.Sin(rotateTime) * -1, 0));

            if (rotateTime >= 0.25)
            {
                rotationState = RotationPar.RESET;
                rotateTime = 0.0f;
                yield return new WaitForSeconds(1);
                break;
            }*/
        }

        //正位置に戻る
        while (rotationState == RotationPar.RESET)
        {
            
            this.transform.Rotate(0, 1.0f, 0);
            rotateCounter++;

            if (rotateCounter < 3) yield return new WaitForSeconds(0.1f);

            if (rotateCounter >= 3 && rotateCounter < 40) yield return new WaitForSeconds(0.01f);

            if (rotateCounter >= 40) yield return new WaitForSeconds(0.075f);

            if (rotateCounter >= 45)
            {
                rotationState = RotationPar.TURN;
                rotateCounter = 0;
                yield return new WaitForSeconds(1);
                break;
            }
            
            /*
             * Debug.Log("正面を向く");
            rotateTime += 0.001f;
            this.transform.Rotate(new Vector3(0, Mathf.Sin(rotateTime), 0));

            if (rotateTime >= 0.125f)
            {
                rotationState = RotationPar.TURN;
                rotateTime = 0.0f;
                yield return new WaitForSeconds(1);
                break;
            }*/
        }

        //次の終点の方向に旋回
        while (rotationState == RotationPar.TURN)
        {
            
            this.transform.Rotate(0, 1.0f, 0);
            rotateCounter++;

            if (rotateCounter < 3) yield return new WaitForSeconds(0.01f);

            if (rotateCounter >= 3 && rotateCounter <= 175) yield return new WaitForSeconds(0.00075f);

            if (rotateCounter >= 175) yield return new WaitForSeconds(0.075f);

            if (rotateCounter >= 180)
            {
                rotateCounter = 0;
                yield return new WaitForSeconds(1);
                break;
            }
            /*
            Debug.Log("向きを反転");
            rotateTime += 0.001f;
            this.transform.Rotate(new Vector3(0, Mathf.Sin(rotateTime), 0));

            if (rotateTime >= 0.5f)
            {
                rotationState = RotationPar.TURN;
                rotateTime = 0.0f;
                yield return new WaitForSeconds(1);
                break;
            }*/
        }

        doTurn = false;
        rotationState = RotationPar.NULL;

        if (noCountFlag == true)
        {
            noCountFlag = false;
        }
        yield break;
    }
    #endregion
}

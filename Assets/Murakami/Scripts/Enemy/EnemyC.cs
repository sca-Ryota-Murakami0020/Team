using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EnemyC : MonoBehaviour
{
    //表示の管理を行う
    private ResetEnemyPosition rEP;
    //Enemyの蘇生処理
    private ReSponeEnemy rSE;
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
    private int rotateCounter;
    //回転時間
    private float rotateTime = 0.0f;
    //走行距離
    private float distance = 0.0f;
    //回転した距離
    private float rotationDistance;

    //追跡フラグ
    private bool doEncount;
    //巡回開始のフラグ
    private bool startFlag;
    //回転中かの判定
    private bool doTurn;
    //最初に振り向く処理を向こうにする
    private bool noCountFlag;

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
        rEP = GameObject.Find("startPos").GetComponent<ResetEnemyPosition>();
        rSE = FindObjectOfType<ReSponeEnemy>();

        doEncount = false;
        startFlag = true;
        doTurn = false;
        noCountFlag = true;
        rotateCounter = 0;
        rotationState = RotationPar.NULL;
        rotationDistance = 0.0f;
        //defaultPosition = this.transform.position;

    }

    // Update is called once per frameshotRayPosition.transform.position, 
    void Update()
    {
        //ここで進行先のPlayerを感知する
        Vector3 rayPosition = shotRayPosition.transform.position;
        RaycastHit hit;
        Ray ray = new Ray(rayPosition, this.gameObject.transform.forward);
        Debug.DrawRay(shotRayPosition.transform.position, shotRayPosition.transform.forward * this.rayDistance, Color.red ,1.0f);
        if (Physics.Raycast(ray, out hit, 100.0f))
        {
            if(hit.collider.gameObject.CompareTag("Player"))
            {
                this.doEncount = true;
            }        
        }

        //速度関係
        if (this.doEncount) this.transform.position += transform.forward * addSpeed * Time.deltaTime;
        else
        {
            if (!doTurn)
            {
                //this.transform.position += transform.forward * enemySpeed * Time.deltaTime;
                distance += 0.01f;
                this.transform.position = new Vector3(pos.x, pos.y, pos.z + Mathf.Sin(distance) * limitDistance * enemySpeed);
                if(rotateTime >= 1.0f)
                {
                    doTurn = true;
                    distance = 0.0f;
                    returnLookPosition();
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        if ((collision.gameObject.CompareTag("Wall") ||
            collision.gameObject.CompareTag("WallJumpPoint") || 
            collision.gameObject.CompareTag("LimitWall") || 
            collision.gameObject.CompareTag("Ground")) && 
            doEncount == true)
        {
            Debug.Log("消えた");
            //StartCoroutine("ResetEnemy");
            rSE.SponeEnemy();
            Destroy(this);
        }
    }

    public void returnLookPosition()
    {
        //次の終点位置の方向に向けるようにするために値を変更する
        this.defaultRotation = this.transform.rotation;
        this.doTurn = true;
        this.doEncount = false;
        this.rotationState = RotationPar.RIGHT;
        StartCoroutine("TurnLookPosition");
    }

    private IEnumerator TurnLookPosition()
    {
        yield return new WaitForSeconds(1);

        while (rotationState == RotationPar.RIGHT)
        {
            /* 
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
            */

            rotateTime += 0.01f;
            this.transform.Rotate(new Vector3(0, Mathf.Sin(rotateTime), 0));

            if (rotateTime >= 0.25f)
            {
                rotationState = RotationPar.LEFT;
                rotateTime = 0.0f;
                yield return new WaitForSeconds(1);
                break;
            }
        }

        //左方向に回転
        while (rotationState == RotationPar.LEFT)
        {
            /*
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
            }*/

            rotateTime += 0.01f;
            this.transform.Rotate(new Vector3(0, Mathf.Sin(rotateTime) * -1, 0));

            if (rotateTime >= 0.5)
            {
                rotationState = RotationPar.RESET;
                rotateTime = 0.0f;
                yield return new WaitForSeconds(1);
                break;
            }
        }

        //正位置に戻る
        while (rotationState == RotationPar.RESET)
        {
            /*
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
            */
            rotateTime += 0.01f;
            this.transform.Rotate(new Vector3(0, Mathf.Sin(rotateTime), 0));

            if (rotateTime >= 0.25f)
            {
                rotationState = RotationPar.TURN;
                rotateTime = 0.0f;
                yield return new WaitForSeconds(1);
                break;
            }
        }

        while (rotationState == RotationPar.TURN)
        {
            /*
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
            */

            rotateTime += 0.01f;
            this.transform.Rotate(new Vector3(0, Mathf.Sin(rotateTime), 0));

            if (rotateTime >= 1.0f)
            {
                rotationState = RotationPar.TURN;
                rotateTime = 0.0f;
                yield return new WaitForSeconds(1);
                break;
            }
        }

        doTurn = false;
        rotationState = RotationPar.NULL;

        if (noCountFlag == true)
        {
            noCountFlag = false;
        }
        yield break;
    }
}

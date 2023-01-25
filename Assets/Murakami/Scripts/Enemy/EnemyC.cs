using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO.MemoryMappedFiles;

public class EnemyC : MonoBehaviour
{
    //プレイヤーの情報を取得するために使用
    private PlayerC pl;
    //移動速度
    [SerializeField] private float enemySpeed = 0.7f;
    //位置
    private Vector3 pos;
    //RigidBody
    Rigidbody rb;
    //ノックバック速度
    private float speed = 1.5f;
    //追跡中使うスピード
    private float addSpeed = 3.0f;
    //追跡フラグ
    private bool doEncount;
    //巡回開始のフラグ
    private bool startFlag;
    //スタート地点
    [SerializeField] private GameObject startPoint;
    //終着点
    [SerializeField] private GameObject endPoint;
    //表示の管理を行う
    private ResetEnemyPosition rEP;
    //ray関係
    private float rayDistance = 2.0f;
    //回転中かの判定
    private bool doTurn;
    //rayを飛ばすオブジェクト
    [SerializeField] private GameObject shotRayPosition;
    //回転の軸を設定するために必要
    private Quaternion defaultRotation;
    //回転時間
    private float rotateTime = 0.0f;
    //最初に振り向く処理を向こうにする
    private bool noCountFlag;
    //回転回数
    private int rotateCounter;
    //回転した距離
    private float rotationDistance;

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
    public GameObject StartP
    {
        get { return this.startPoint;}
        set { this.startPoint = value;}
    }

    public GameObject EndP
    {
        get { return this.endPoint;}
        set { this.endPoint = value;}
    }

    public bool DoEn
    {
        get { return this.doEncount;}
        set { this.doEncount = value;}
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        var rb = GetComponent<Rigidbody>();
        pl = GameObject.Find("Player").GetComponent<PlayerC>();
        rEP = GameObject.Find("startPos").GetComponent<ResetEnemyPosition>();
        this.transform.position = this.startPoint.transform.position;
        doEncount = false;
        startFlag = true;
        doTurn = false;
        noCountFlag = true;
        rotateCounter = 0;
        rotationState = RotationPar.NULL;
        rotationDistance = 0.0f;

        this.transform.LookAt(endPoint.transform.position);

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
            //Debug.Log("rayHit");
            if(hit.collider.gameObject.CompareTag("Player"))
            {
                //Debug.Log("ray");
                this.doEncount = true;
            }        
        }

        //速度関係
        //Debug.Log("doTurn: " + doTurn);
        if (this.doEncount) this.transform.position += transform.forward * addSpeed * Time.deltaTime;
        else
        {
            if(!doTurn)
            {
                this.transform.position += transform.forward * enemySpeed * Time.deltaTime;
            }
            else
            {
                this.transform.position += new Vector3(0,0,0);
            }
        }
        //Debug.Log("エンカウント:" + rotationState);
    }

    private void OnCollisionEnter(Collision collision)
    {

        if ((collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("WallJumpPoint") || collision.gameObject.CompareTag("LimitWall")) && doEncount == true)
        {
            //Destroy(this.gameObject);
            //eSC.SponeEnemy();
            StartCoroutine("ResetEnemy");
            //Debug.Log("引き渡し");
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {      
        if (other.gameObject == this.startPoint && noCountFlag == false)
        {
            returnLookEndPosition();
        }

        if (other.gameObject == this.endPoint)
        {
            returnLookStartPosition();
        }
    }

    public void returnLookStartPosition()
    {
        //endPointの方向に向けるようにするために値を変更する
        defaultRotation = this.transform.rotation;
        doTurn = true;
        rotationState = RotationPar.RIGHT;
        StartCoroutine("TurnLookForStartPoint");
        //Debug.Log("始点から動き出す");
    }

    public void returnLookEndPosition()
    {
        //startPointの方向に向けるようにするために値を変更する
        defaultRotation = this.transform.rotation;
        doTurn = true;
        rotationState = RotationPar.RIGHT;
        StartCoroutine("TurnLookForEndPoint");
        //Debug.Log("終点から動き出す");     
    }

    private IEnumerator ResetEnemy()
    { 
        this.transform.position = this.startPoint.transform.position;
        this.doEncount = false;
        rEP.StartCountDistance();
        this.gameObject.SetActive(false);
        //Debug.Log("待機中");
        yield break;
    }

    private IEnumerator TurnLookForStartPoint()
    {
        yield return new WaitForSeconds(1);       
        while(rotationState == RotationPar.RIGHT)
        {
            //一度ずつ回転
            this.transform.Rotate(0,1.0f,0);
            rotateCounter++;

            //始めは少し緩やかに回転
            if (rotateCounter < 3) yield return new WaitForSeconds(0.1f);
            //f痛の回転速度で回転
            if (rotateCounter >= 3 && rotateCounter < 40) yield return new WaitForSeconds(0.01f);
            //最後は少し緩やかに回転する
            if (rotateCounter >= 40) yield return new WaitForSeconds(0.075f);

            //回転した度数が45°を超えたら
            if (rotateCounter >=45)
            {
                rotationState = RotationPar.LEFT;
                rotateCounter = 0;
                yield return new WaitForSeconds(1);
                Debug.Log("左回転に移行する");
                break;
            }         
        }

        //左方向に回転
        while(rotationState == RotationPar.LEFT)
        {
            //transform.rotation = Quaternion.AngleAxis(-2.0f, this.transform.up) * defaultRotation;]rotationDistance = defaultRotation.y + 75.0f;
            this.transform.Rotate(0, -1.0f, 0);
            rotateCounter++;

            if(rotateCounter < 3) yield return new WaitForSeconds(0.1f);

            if (rotateCounter >= 3 && rotateCounter < 85) yield return new WaitForSeconds(0.01f);

            if (rotateCounter >= 85) yield return new WaitForSeconds(0.075f);

            //Debug.Log("左回転中");
            if (rotateCounter >= 90)
            {
                rotationState = RotationPar.RESET;
                rotateCounter = 0;
                yield return new WaitForSeconds(1);
                Debug.Log("リセットに移行する");
                break;
            }
        }

        //正位置に戻る
        while(rotationState == RotationPar.RESET)
        {
            //transform.rotation = Quaternion.AngleAxis(-2.0f, this.transform.up) * defaultRotation;
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
                Debug.Log("OK,Go");
                break;
            }         
        }

        while(rotationState == RotationPar.TURN)
        {
            //transform.rotation = Quaternion.AngleAxis(2.0f, this.transform.up) * defaultRotation;
            this.transform.Rotate(0, 1.0f, 0);
            rotateCounter++;

            if (rotateCounter < 3) yield return new WaitForSeconds(0.01f);

            if (rotateCounter >=3 && rotateCounter <= 175) yield return new WaitForSeconds(0.00075f);

            if (rotateCounter >= 175) yield return new WaitForSeconds(0.075f);

            if (rotateCounter >= 180)
            {
                Debug.Log("TURN");
                rotateCounter = 0;
                yield return new WaitForSeconds(1);
                break;
            }           
        }
        
        doTurn = false;
        rotationState = RotationPar.NULL;
        if(noCountFlag == true)
        {
            noCountFlag = false;
        }
        yield break;
    }

    private IEnumerator TurnLookForEndPoint()
    {
        yield return new WaitForSeconds(1);
        while (rotationState == RotationPar.RIGHT)
        {
            //transform.rotation = Quaternion.AngleAxis(2.0f, this.transform.up) * defaultRotation;rotationDistance = defaultRotation.y - 75.0f;
            this.transform.Rotate(0, 1.0f, 0);
            rotateCounter++;

            if (rotateCounter < 3) yield return new WaitForSeconds(0.1f);

            if (rotateCounter >= 3 && rotateCounter < 40) yield return new WaitForSeconds(0.01f);

            if (rotateCounter >= 40) yield return new WaitForSeconds(0.075f);

            //Debug.Log("右回転中");
            if (rotateCounter >= 45)
            {
                rotationState = RotationPar.LEFT;
                rotateCounter = 0;
                yield return new WaitForSeconds(1);
                Debug.Log("左回転に移行する");
                break;
            }
        }

        //左方向に回転
        while (rotationState == RotationPar.LEFT)
        {
            //transform.rotation = Quaternion.AngleAxis(-2.0f, this.transform.up) * defaultRotation;]rotationDistance = defaultRotation.y + 75.0f;
            this.transform.Rotate(0, -1.0f, 0);
            rotateCounter++;

            if (rotateCounter < 3) yield return new WaitForSeconds(0.1f);

            if (rotateCounter>= 3 && rotateCounter < 85) yield return new WaitForSeconds(0.01f);

            if(rotateCounter >= 80) yield return new WaitForSeconds(0.075f);

            //Debug.Log("左回転中");
            if (rotateCounter >= 90)
            {
                rotationState = RotationPar.RESET;
                rotateCounter = 0;
                yield return new WaitForSeconds(1);
                Debug.Log("リセットに移行する");
                break;
            }
        }

        //正位置に戻る
        while (rotationState == RotationPar.RESET)
        {
            //transform.rotation = Quaternion.AngleAxis(-2.0f, this.transform.up) * defaultRotation;
            this.transform.Rotate(0, 1.0f, 0);
            rotateCounter++;

            if (rotateCounter < 3) yield return new WaitForSeconds(0.1f);

            if (rotateCounter >= 3 && rotateCounter < 35) yield return new WaitForSeconds(0.01f);

            if (rotateCounter >= 35) yield return new WaitForSeconds(0.075f);

            if (rotateCounter >= 45)
            {
                rotationState = RotationPar.TURN;
                rotateCounter = 0;
                yield return new WaitForSeconds(1);
                Debug.Log("OK,Go");
                break;
            }
        }

        while (rotationState == RotationPar.TURN)
        {
            //transform.rotation = Quaternion.AngleAxis(2.0f, this.transform.up) * defaultRotation;
            this.transform.Rotate(0, 1.0f, 0);
            rotateCounter++;

            if (rotateCounter < 3) yield return new WaitForSeconds(0.01f);

            if (rotateCounter >= 3 && rotateCounter <= 175) yield return new WaitForSeconds(0.00075f);

            if (rotateCounter >= 175) yield return new WaitForSeconds(0.075f);

            if (rotateCounter >= 180)
            {
                Debug.Log("TURN");
                rotateCounter = 0;
                yield return new WaitForSeconds(1);
                break;
            }
        }

        Debug.Log("終点に向けて動き出す");
        doTurn = false;
        rotationState = RotationPar.NULL;
        yield break;
    }
}

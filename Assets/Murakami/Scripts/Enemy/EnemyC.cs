using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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
        this.transform.LookAt(endPoint.transform.position);
    }

    // Update is called once per frameshotRayPosition.transform.position, 
    void Update()
    {
        //ここで進行先のPlayerを感知する
        Vector3 rayPosition = shotRayPosition.transform.position;
        RaycastHit hit;
        Ray ray = new Ray(rayPosition, Vector3.forward);
        Debug.DrawRay(shotRayPosition.transform.position, shotRayPosition.transform.forward * this.rayDistance, Color.red ,1.0f);
        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            //Debug.Log("rayHit");
            if(hit.collider.CompareTag("Player"))
            {
                Debug.Log("rayHit");
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
        //Debug.Log("エンカウント" + doEncount);
    }

    private void OnCollisionEnter(Collision collision)
    {
        /*
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector3 ver = (pl.transform.position - this.transform.position).normalized;
            ver.y = 0;
            ver = ver.normalized;
            collision.transform.Translate(ver * speed);
        }*/

        if (collision.gameObject.CompareTag("WallJumpPoint"))
        {
            //Destroy(this.gameObject);
            //eSC.SponeEnemy();
            StartCoroutine("ResetEnemy");
            //Debug.Log("引き渡し");
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {      
        if (other.gameObject == this.startPoint && doEncount == false && this.gameObject.CompareTag("Enemy"))
        {
            returnLookStartPosition();
        }

        if (other.gameObject == this.endPoint && doEncount == false && this.gameObject.CompareTag("Enemy"))
        {
            returnLookEndPosition();
        }
        
        /*if(other.gameObject.CompareTag("Player"))
        {
            this.doEncount = true;
        }*/
    }

    public void returnLookStartPosition()
    {
        //StartCoroutine("TurnLookForStartPoint");
        this.transform.LookAt(endPoint.transform.position);
        //Debug.Log("Estart開始");
    }

    public void returnLookEndPosition()
    {
        //StartCoroutine("TurnLookForEndPoint");
        this.transform.LookAt(startPoint.transform.position);
        //Debug.Log("Eend開始");
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

    /*private IEnumerator TurnLookForStartPoint()
    {
        yield return new WaitForSeconds(3);
        transform.Rotate(new Vector3(0, 2, 0));
        if()
    }

    private IEnumerator TurnLookForEndPoint()
    { 

    }*/
}

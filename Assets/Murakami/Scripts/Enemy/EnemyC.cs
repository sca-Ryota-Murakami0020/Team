using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Start is called before the first frame update
    void Start()
    {
        var rb = GetComponent<Rigidbody>();
        pl = GameObject.Find("Player").GetComponent<PlayerC>();
        this.transform.position = startPoint.transform.position;
        doEncount = false;
        startFlag = true;
        this.transform.LookAt(endPoint.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (this.doEncount) this.transform.position += transform.forward * addSpeed * Time.deltaTime;
        else this.transform.position += transform.forward * enemySpeed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector3 ver = (pl.transform.position - this.transform.position).normalized;
            ver.y = 0;
            ver = ver.normalized;
            collision.transform.Translate(ver * speed);
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            Destroy(this.gameObject);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == startPoint && !doEncount && this.gameObject.CompareTag("Enemy"))
        {
            returnLookStartPosition();
        }

        else if (other.gameObject == endPoint && !doEncount && this.gameObject.CompareTag("Enemy"))
        {
            returnLookEndPosition();
        }
    }

    public void returnLookStartPosition()
    {
        this.transform.LookAt(endPoint.transform.position);
        Debug.Log("Estart開始");
    }

    public void returnLookEndPosition()
    {
        this.transform.LookAt(startPoint.transform.position);
        Debug.Log("Eend開始");
    }
}

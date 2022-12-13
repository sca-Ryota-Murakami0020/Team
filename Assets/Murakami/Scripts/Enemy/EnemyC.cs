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
    //スタート地点
    [SerializeField] private GameObject startPoint;
    //終着点
    [SerializeField] private GameObject endPoint;

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
            pl.Hp -= 1;
            if(pl.Hp <= 0) 
            {
                pl.GameOver();
            }
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
        if (other.gameObject == startPoint && !this.doEncount)
        {
            this.transform.LookAt(endPoint.transform.position);
            //Debug.Log("start開始");
        }

        else if (other.gameObject == endPoint && !this.doEncount)
        {
            this.transform.LookAt(startPoint.transform.position);
            //Debug.Log("end開始");
        }
    }
}

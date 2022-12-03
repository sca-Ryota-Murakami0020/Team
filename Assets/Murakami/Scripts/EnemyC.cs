using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyC : MonoBehaviour
{
    //プレイヤーの情報を取得するために使用
    private PlayerC pl;
    //移動速度
    [SerializeField] private float enemySpeed = 0.3f;
    //位置
    private Vector3 pos;
    //移動幅
    private float wide = 8.0f;
    //RigidBody
    Rigidbody rb;
    //ノックバック速度
    private float speed = 1.5f;
    //周回周期
    private float theta;
    //スタート地点
    [SerializeField] private GameObject startPoint;
    //
    [SerializeField] private GameObject endPoint;

    // Start is called before the first frame update
    void Start()
    {
        var rb = GetComponent<Rigidbody>();
        pl = GameObject.Find("Player").GetComponent<PlayerC>();
        theta = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        //if()
        //this.pos = this.transform.position;
        var diff = pl.transform.position - transform.position;
        var axis = Vector3.Cross(transform.forward, diff);
        var angle = Vector3.Angle(transform.forward, diff) * (axis.y < 0 ? -1 : 1);
        if (angle <= 70.0f && angle >= -70.0f)
        {
            // ray cast
            RaycastHit hit;
            Vector3 temp = pl.transform.position - this.transform.position;
            Vector3 normal = temp.normalized;
            if (Physics.Raycast(this.transform.position, normal, out hit, 50))
            {
                if (hit.transform.gameObject == pl)
                {
                    // 対象のオブジェクトが視界に入った時の処理
                }
                else this.transform.position += transform.forward * 0.7f;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            pl.Hp -= 1;
            Vector3 ver = (pl.transform.position - this.transform.position).normalized;
            ver.y = 0;
            ver = ver.normalized;
            collision.transform.Translate(ver * speed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == startPoint)
        {
            this.transform.LookAt(endPoint.transform.position);
        }

        else if(other.gameObject == endPoint)
        {
            this.transform.LookAt(startPoint.transform.position);
        }
    }
}

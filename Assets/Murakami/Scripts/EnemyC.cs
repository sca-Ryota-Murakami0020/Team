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

    // Start is called before the first frame update
    void Start()
    {
        pos = this.transform.position;
        var rb = GetComponent<Rigidbody>();
        pl = GameObject.Find("Player").GetComponent<PlayerC>();
        theta = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = new Vector3(pos.x , pos.y, pos.z + Mathf.Sin(theta)* wide);
        theta += 0.01f * enemySpeed;
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySponerC : MonoBehaviour
{
    //スタート地点
    [SerializeField] private GameObject setStartPoint;
    //終着点
    [SerializeField] private GameObject setEndPoint;
    //呼び出す敵のオブジェクト
    [SerializeField] private GameObject isSponeEnemy;
    //出現間隔
    private float coolSponeEnemyTime = 3.0f;
    //Enemyを呼び出した時にtrueになり消去されるまでtrueになる
    //private bool isEnemyAlive;

    public GameObject StartPoint
    {
        get { return this.setStartPoint;}
        set { this.setStartPoint = value;}
    }

    public GameObject EndPoint
    {
        get { return this.setEndPoint;}
        set { this.setEndPoint = value;}
    }

    void Start()
    {
        this.transform.position = this.setStartPoint.transform.position;
    }

    public void SponeEnemy()
    {
        Debug.Log("コルーチン開始");
        StartCoroutine("StartSponeEnemy");
    }

    private IEnumerable StartSponeEnemy()
    {
        coolSponeEnemyTime -= Time.deltaTime;
        if(coolSponeEnemyTime <= 0.0f)
        {
            coolSponeEnemyTime = 3.0f;
            Instantiate(isSponeEnemy, this.gameObject.transform.position, Quaternion.identity);
            yield break;
        }
    }
}

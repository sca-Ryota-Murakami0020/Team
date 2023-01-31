using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReSponeEnemy : MonoBehaviour
{
    //呼び出すEnemy
    [SerializeField] private GameObject enemy;
    //1体のみ出力するためのフラグ
    private bool aliveFlag;
    //Enemyを呼び出すオブジェクト
    [SerializeField] private GameObject sponePosObj;
    //呼び出す位置
    private Vector3 sponePosition;

    /*
    public bool AliveFlag
    {
        get { return this.aliveFlag;}
        set { this.aliveFlag = value;}
    }*/

    void Start()
    {
        aliveFlag = false;
        sponePosition = sponePosObj.transform.position;
        this.transform.position = sponePosition;
        Debug.Log("最初の呼び出し完了");
    }

    void Update()
    {      
        //初期設置
        if(aliveFlag == false)
        {
            //最初、ステージには敵が配置していないのでここで一度だけ呼び出す
            Instantiate(enemy, new Vector3(sponePosition.x, sponePosition.y,sponePosition.z), Quaternion.identity);
            aliveFlag = true;
            Debug.Log("呼び出し位置" + sponePosition);
        }
        
    }

    public void SponeEnemy()
    {
        //生成処理するコルーチンの起動
        StartCoroutine("StartSponeEnemy");
    }

    private IEnumerator StartSponeEnemy()
    {
        Debug.Log("処理中");
        //10秒間待機する
        yield return new WaitForSeconds(10);
        //10秒後にEnemyCの方でSetSctiveをfalseにしたGameObjectのSetActiveをtrueにして再び活動できるようにしてあげる
        Instantiate(enemy, this.transform.position, Quaternion.identity);
        Debug.Log("処理完了");
        yield break;
    }
}

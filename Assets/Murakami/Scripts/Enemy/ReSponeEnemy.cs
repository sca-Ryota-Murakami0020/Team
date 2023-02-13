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

    void Start()
    {
        aliveFlag = false;
        sponePosition = sponePosObj.transform.position;
        this.transform.position = sponePosition;
    }

    void Update()
    {      
        //初期設置
        if(aliveFlag == false)
        {
            //最初、ステージには敵が配置していないのでここで一度だけ呼び出す
            Instantiate(enemy, new Vector3(sponePosition.x, sponePosition.y,sponePosition.z), Quaternion.identity);
            aliveFlag = true;
        }
        
    }

    public void SponeEnemy()
    {
        //生成処理するコルーチンの起動
        StartCoroutine("StartSponeEnemy");
    }

    private IEnumerator StartSponeEnemy()
    {
        //10秒間待機する
        yield return new WaitForSeconds(10);
        //10秒後にEnemyCの方でSetSctiveをfalseにしたGameObjectのSetActiveをtrueにして再び活動できるようにしてあげる
        Instantiate(enemy, this.transform.position, Quaternion.identity);
        yield break;
    }
}

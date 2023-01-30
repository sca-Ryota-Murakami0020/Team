using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SponeEnemy : MonoBehaviour
{
    //スタート位置
    private Vector3 startPos;
    //呼び出すEnemy
    [SerializeField] private GameObject enemy;
    //呼び出すときのEnemyの角度
    [SerializeField] 


    // Start is called before the first frame update
    void Start()
    {
        startPos = this.transform.position;
        Instantiate(enemy, startPos, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReSponeEnemy()
    {
        StartCoroutine("StartSponeEnemy");
    }

    private IEnumerator StartSponeEnemy()
    {
        Debug.Log("処理中");
        //10秒間待機する
        yield return new WaitForSeconds(10);
        //10秒後にEnemyCの方でSetSctiveをfalseにしたGameObjectのSetActiveをtrueにして再び活動できるようにしてあげる
        Instantiate(enemy,startPos,Quaternion.identity);
        Debug.Log("処理完了");
        yield break;
    }
}

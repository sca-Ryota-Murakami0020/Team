using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetEnemyPosition : MonoBehaviour
{

    [SerializeField] private GameObject enemy;
     
    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartCountDistance()
    {
        //EnemyCから受け取た時に指示するコルーチンを呼び出す
        Debug.Log("引き渡し成功");
        StartCoroutine("ActiveEnemy");
    }

    private IEnumerator ActiveEnemy()
    {
        Debug.Log("処理中");
        //10秒間待機する
        yield return new WaitForSeconds(10);
        //10秒後にEnemyCの方でSetSctiveをfalseにしたGameObjectのSetActiveをtrueにして再び活動できるようにしてあげる
        enemy.SetActive(true);
        Debug.Log("処理完了");
        yield break;
    }
}

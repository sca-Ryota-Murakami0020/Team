using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPlayerposition : MonoBehaviour
{
    //プレイヤーが戻る位置の基準点になるオブジェクト
    [SerializeField] private GameObject resetPosition;
    private Player pl;

    void Start()
    {
        pl = FindObjectOfType<Player>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            //プレイヤーがビルステージのコンクリートオブジェクトに触れたら
            //指定の位置に設置しているオブジェクトの位置に移動させる
            pl.transform.position = resetPosition.transform.position;
        }
    }
}

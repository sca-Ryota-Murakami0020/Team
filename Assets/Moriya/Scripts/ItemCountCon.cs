using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCountCon : MonoBehaviour
{

    //右上のアイテムカウント用スプリクト

    //テキストを呼び出すようにする
    private Text itemText = null;
    //敵を倒した記録用変数
    private float oldItemCount = 0;
    //GMとプレイヤーとカメラスプリクトの定義
    //private GManeger gmaneger;
    private totalGameManager totalGM;
    //アイテム記録用変数
    private int item = 0;

    // Start is called before the first frame update
    void Start()
    {
        //GMとプレイヤーとカメラスプリクトの呼び出し
        //this.gmaneger = FindObjectOfType<GManeger>();
        this.totalGM = FindObjectOfType<totalGameManager>();
        //テキストを使えるようにする
        itemText = GetComponent<Text>();
        //GMが定義されていたら
        if (totalGM != null)
        {
            //テキストを画面に出す
            itemText.text = "アイテムの数:" + item;
        }
    }
    // Update is called once per frame
    void Update()
    {
        // //倒した敵の数を表記変更する場合
        item = totalGM.PlayerIC;
        //記録用変数とGMから持ってきた値を保存する変数の値が違ったら
        if (oldItemCount != item)
        {
            //値を変えてテキストを画面に出す
            itemText.text = "取ったアイテムの数:" + item;
            oldItemCount = item;
        }


    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Itemcounter : MonoBehaviour
{
    //表示する画像の配列
    [SerializeField] private Sprite[] numberImage;
    //プレイヤーが取得したアイテムの個数。これを用いて呼び出す画像を識別する
    private int itemCon;
    //画像を表示するImage
    [SerializeField] private Image image;
    //TotalGameManager
    private totalGameManager gm;
    //SpriteRenderer sr;

    private void Awake()
    {
        gm = FindObjectOfType<totalGameManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //TotalGameManagerのアイテムカウントを取得する
        itemCon = gm.PlayerIC;
        //TotalGameManagerが持つアイテムカウントの数値に応じた数字の画像を呼び出す
        image.sprite =  numberImage[itemCon];
    }
}

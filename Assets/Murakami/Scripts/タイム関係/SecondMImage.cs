using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SecondMImage : MonoBehaviour
{
    //TotalGameManager
    private totalGameManager gm;
    //表示する画像の配列
    [SerializeField] private Sprite[] numberImage;
    //画像を表示するImage
    [SerializeField] private Image image;
    //TotalGameManagerから取得したゲーム時間をint型として扱えるようにするための変数
    private int SecondMCount;

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<totalGameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //TotalGameManagerから取得したゲーム時間をint型に変換
        SecondMCount = Mathf.FloorToInt(gm.TotalTime / 600);
        //ゲーム時間の10秒単位の表示を行う(0〜5)
        image.sprite = numberImage[SecondMCount];
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class firstMImage : MonoBehaviour
{
    //TotalGameManager
    private totalGameManager gm;
    //表示する画像の配列
    [SerializeField] private Sprite[] numberImage;
    //画像を表示するImage
    [SerializeField] private Image image;
    //TotalGameManagerから取得したゲーム時間をint型にして取得する変数
    private int firstMCount;

    void Start()
    {
        gm = FindObjectOfType<totalGameManager>();
    }

    void Update()
    {
        //TotalGameManagerから取得したゲーム時間をint型に変換
        firstMCount = Mathf.FloorToInt(gm.TotalTime / 60);
        //ゲーム時間の1分単位の表示を行う(0～9)
        image.sprite = numberImage[firstMCount % 10];
    }
}

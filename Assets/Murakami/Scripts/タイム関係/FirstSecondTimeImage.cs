using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//0〜9までの一桁までのImageの表示
public class FirstSecondTimeImage : MonoBehaviour
{
    //TotalGameManger
    private totalGameManager gm;
    //表示する画像の配列
    [SerializeField] private Sprite[] numberImage;
    //画像を表示するImage
    [SerializeField] private Image image;
    //TotalGameManagerから取得したゲーム時間をint型として扱えるようにするための変数
    private int firstSCount;

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<totalGameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //TotalGameManagerから取得したゲーム時間をint型に変換
        firstSCount = Mathf.FloorToInt(gm.TotalTime % 10);
        //ゲーム時間の1秒単位の表示を行う(0〜9)
        image.sprite = numberImage[firstSCount];
    }
}

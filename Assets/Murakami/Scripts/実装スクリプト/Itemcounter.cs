using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Itemcounter : MonoBehaviour
{
    [SerializeField] private Sprite[] numberImage;
    private int itemCon;
    [SerializeField] private Image image;
    private totalGameManager gm;
    //SpriteRenderer sr;

    private void Awake()
    {
        gm = FindObjectOfType<totalGameManager>();
        //sr = gameObject.GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        itemCon = gm.PlayerIC;
        image.sprite =  numberImage[itemCon];
    }
}
/*
 public float[] BestTime//ハイスコアの数値格納配列
    {
        get { return this.bestTime; }
        set { this.bestTime = value; }
    }

    public float TotalTime//1プレイ時間
    {
        get { return this.totalTime; }

        set { this.totalTime = value; }
    }

    /*public bool CounterFlag//
    {
        get { return this.counterFlag; }
        set { this.counterFlag = value; }
    

public string[] TimeText//ハイスコアの文字列格納配列
{
    get { return this.timer; }
    set { this.timer = value; }
}

public string DispTime//プレイ中に表示するタイム表記
{
    get { return this.timeScore; }
    set { this.timeScore = value; }
}
*/

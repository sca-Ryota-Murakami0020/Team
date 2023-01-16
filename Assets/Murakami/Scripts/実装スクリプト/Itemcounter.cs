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
 public float[] BestTime//�n�C�X�R�A�̐��l�i�[�z��
    {
        get { return this.bestTime; }
        set { this.bestTime = value; }
    }

    public float TotalTime//1�v���C����
    {
        get { return this.totalTime; }

        set { this.totalTime = value; }
    }

    /*public bool CounterFlag//
    {
        get { return this.counterFlag; }
        set { this.counterFlag = value; }
    

public string[] TimeText//�n�C�X�R�A�̕�����i�[�z��
{
    get { return this.timer; }
    set { this.timer = value; }
}

public string DispTime//�v���C���ɕ\������^�C���\�L
{
    get { return this.timeScore; }
    set { this.timeScore = value; }
}
*/

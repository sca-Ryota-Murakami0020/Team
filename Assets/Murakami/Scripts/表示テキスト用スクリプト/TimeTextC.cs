using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeTextC : MonoBehaviour
{
    private totalGameManager totalGM;
    [SerializeField] private Sprite[] numberImage;
    [SerializeField] private Image oneSecImage;
    [SerializeField] private Image tenSecImage;
    [SerializeField] private Image oneMinImage;
    [SerializeField] private Image tenMinImage;
    [SerializeField] private RectTransform timer;
    private int counter;


    // Start is called before the first frame update
    void Start()
    {
        totalGM = FindObjectOfType<totalGameManager>();
        oneSecImage.sprite = numberImage[0];
        tenSecImage.sprite = numberImage[0];
        oneMinImage.sprite = numberImage[0];
        tenMinImage.sprite = numberImage[0];
        counter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(olt.OldSecondTime);
        //ハイスコアの表記
        //TimeText.text = (totalGM.TotalTime / 3600).ToString("00") + ":" + (totalGM.TotalTime / 120).ToString("00") + ":" + ((int)totalGM.TotalTime % 60).ToString("00");
        Debug.Log("リザルト処理完了");
        oneSecImage.sprite = numberImage[Mathf.FloorToInt(totalGM.TotalTime % 10)];
        tenSecImage.sprite = numberImage[Mathf.FloorToInt((totalGM.TotalTime % 60) / 10)];
        oneMinImage.sprite = numberImage[Mathf.FloorToInt(totalGM.TotalTime / 60)];
        tenMinImage.sprite = numberImage[Mathf.FloorToInt(totalGM.TotalTime / 600)];

        if(counter >= 100)
        {
            timer.position += new Vector3(1.0f * Time.deltaTime,0,0);
            counter++;
        }
    }
}

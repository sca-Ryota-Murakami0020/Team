using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SecondScore : MonoBehaviour
{
    private totalGameManager totalGM;
    //private Animator anim;
    [SerializeField] private Image oneSecImage;
    [SerializeField] private Image tenSecImage;
    [SerializeField] private Image oneMinImage;
    [SerializeField] private Image tenMinImage;
    [SerializeField] private Sprite[] numberImage;
    [SerializeField] private RectTransform timer;
    private int counter;

    // Start is called before the first frame update
    void Start()
    {
        totalGM = FindObjectOfType<totalGameManager>();
        for (int i = 0; i <= 2; i++)
        {
            oneSecImage.sprite = numberImage[0];
            tenSecImage.sprite = numberImage[0];
            oneMinImage.sprite = numberImage[0];
            tenMinImage.sprite = numberImage[0];
        }
        //anim = this.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(olt.OldSecondTime);
        //2位スコアの表記
        if (totalGM.TimeText[1] == "1")
        {
            //２位のデフォルト表示
            oneSecImage.sprite = numberImage[0];
            tenSecImage.sprite = numberImage[0];
            oneMinImage.sprite = numberImage[0];
            tenMinImage.sprite = numberImage[0];
        }
        else
        {
            //２位のランキング表示
            oneSecImage.sprite = numberImage[Mathf.FloorToInt(totalGM.BestTime[1] % 10)];
            tenSecImage.sprite = numberImage[Mathf.FloorToInt((totalGM.BestTime[1] % 60) / 10)];
            oneMinImage.sprite = numberImage[Mathf.FloorToInt(totalGM.BestTime[1] / 60)];
            tenMinImage.sprite = numberImage[Mathf.FloorToInt(totalGM.BestTime[1] / 600)];
            //if(olt.LoadCout > 3)
            //Debug.Log("text2 " + olt.TimeText[1]);
        }

        if (counter >= 100)
        {
            timer.position += new Vector3(1.0f * Time.deltaTime, 0, 0);
            counter++;
        }
        //anim.SetBool("setSecondScore", true);
    }
}

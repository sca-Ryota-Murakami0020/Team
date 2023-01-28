using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TherdScore : MonoBehaviour
{
    private totalGameManager totalGM;
    [SerializeField] private Sprite[] numberImage;
    [SerializeField] private Image oneSecImage;
    [SerializeField] private Image tenSecImage;
    [SerializeField] private Image oneMinImage;
    [SerializeField] private Image tenMinImage;
    [SerializeField] private RectTransform timer;
    private int counter;
    private SecondScore ss;
    //private Animator anim;

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
        counter = 0;
        ss = FindObjectOfType<SecondScore>();
        //anim = this.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(olt.OldSecondTime);
        //2位スコアの表記
        if (totalGM.BestTime[2] <= 0.0f)
        {
            //３位のデフォルト表示"
            oneSecImage.sprite = numberImage[0];
            tenSecImage.sprite = numberImage[0];
            oneMinImage.sprite = numberImage[0];
            tenMinImage.sprite = numberImage[0];
        }
        else
        {
            //３位のランキング表示"
            oneSecImage.sprite = numberImage[Mathf.FloorToInt(totalGM.BestTime[2] % 10)];
            tenSecImage.sprite = numberImage[Mathf.FloorToInt((totalGM.BestTime[2] % 60) / 10)];
            oneMinImage.sprite = numberImage[Mathf.FloorToInt(totalGM.BestTime[2] / 60)];
            tenMinImage.sprite = numberImage[Mathf.FloorToInt(totalGM.BestTime[2] / 600)];
            //if(olt.LoadCout > 3)
            //Debug.Log("text3 " + olt.TimeText[2]);
        }
    }

    private IEnumerator StartTherdScore()
    {
        if (counter <= 300 && counter >= 0)
        {
            timer.position -= new Vector3(2.0f, 0, 0);
            counter++;
        }
        if (counter <= 500 && counter >= 300)
        {
            timer.position -= new Vector3(1.0f, 0, 0);
            counter++;
        }
        yield return new WaitForSeconds(1);
        ss.StartCoroutine("StartSecondScore");
        yield break;
    }
}

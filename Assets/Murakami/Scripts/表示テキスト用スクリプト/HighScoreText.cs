using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreText : MonoBehaviour
{
    private totalGameManager totalGM;
    [SerializeField] private Sprite[] numberImage;
    [SerializeField] private Image oneSecImage;
    [SerializeField] private Image tenSecImage;
    [SerializeField] private Image oneMinImage;
    [SerializeField] private Image tenMinImage;
    [SerializeField] private RectTransform timer;
    private int counter;
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
        //anim = this.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(olt.OldSecondTime);
        //ハイスコアの表記

            if(totalGM.TimeText[0] == "0")
            {
                //１位のデフォルト表示"
                oneSecImage.sprite = numberImage[0];
                tenSecImage.sprite = numberImage[0];
                oneMinImage.sprite = numberImage[0];
                tenMinImage.sprite = numberImage[0];
            }
            else {
                //１位のランキング表示"
                oneSecImage.sprite = numberImage[Mathf.FloorToInt(totalGM.BestTime[0] % 10)];
                tenSecImage.sprite = numberImage[Mathf.FloorToInt((totalGM.BestTime[0] % 60) / 10)];
                oneMinImage.sprite = numberImage[Mathf.FloorToInt(totalGM.BestTime[0] / 60)];
                tenMinImage.sprite = numberImage[Mathf.FloorToInt(totalGM.BestTime[0] / 600)];
            }

        //anim.SetBool("setHighScore", true);
    }

    private IEnumerator StartHighScore()
    {
        if (counter <= 500)
        {
            timer.position -= new Vector3(2.0f, 0, 0);
            counter++;
        }
        yield return new WaitForSeconds(1);
        yield break;
    }
}

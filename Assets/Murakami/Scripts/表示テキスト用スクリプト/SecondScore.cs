using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SecondScore : MonoBehaviour
{
    //totalGameManager
    private totalGameManager totalGM;
    //HighScoreText
    private HighScoreText hst;

    //1�b�P�ʂ̉摜�\�芷�����s��Image(0�`9)
    [SerializeField] private Image oneSecImage;
    //10�b�P�ʂ̉摜�\�芷�����s��Image(0�`5)
    [SerializeField] private Image tenSecImage;
    //1���P�ʂ̉摜�\�芷�����s��Image(0�`9)
    [SerializeField] private Image oneMinImage;
    //10���P�ʂ̉摜�\�芷�����s��Image(0�`5)
    [SerializeField] private Image tenMinImage;
    //���ԕ\���ŗp���鐔���̉�m�z��
    [SerializeField] private Sprite[] numberImage;
    //�e�L�X�g�𓮂������߂ɕK�v�Ȑ錾
    [SerializeField] private RectTransform timer;
    //�R���[�`���Ńe�L�X�g�𓮂����������v�Z����ϐ�
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
        hst = FindObjectOfType<HighScoreText>();
        UpLoadSecondTime();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpLoadSecondTime()
    {
        //2�ʃX�R�A�̕\�L
        if (totalGM.BestTime[1] <= 0.0f)
        {
            //�Q�ʂ̃f�t�H���g�\��
            oneSecImage.sprite = numberImage[0];
            tenSecImage.sprite = numberImage[0];
            oneMinImage.sprite = numberImage[0];
            tenMinImage.sprite = numberImage[0];
        }
        else
        {
            //�Q�ʂ̃����L���O�\��
            oneSecImage.sprite = numberImage[Mathf.FloorToInt(totalGM.BestTime[1] % 10)];
            tenSecImage.sprite = numberImage[Mathf.FloorToInt((totalGM.BestTime[1] % 60) / 10)];
            oneMinImage.sprite = numberImage[Mathf.FloorToInt(totalGM.BestTime[1] / 60)];
            tenMinImage.sprite = numberImage[Mathf.FloorToInt(totalGM.BestTime[1] / 600)];
            //if(olt.LoadCout > 3)
            //Debug.Log("text2 " + olt.TimeText[1]);
        }
    }

    private IEnumerator StartSecondScore()
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
        hst.StartCoroutine("StartHighScore");
        yield break;
    }
}

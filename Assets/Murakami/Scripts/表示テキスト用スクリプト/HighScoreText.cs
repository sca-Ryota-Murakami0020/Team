using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreText : MonoBehaviour
{
    //totalGameManager
    private totalGameManager totalGM;
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
    private int counter = 0;�@

    void Start()
    {
        totalGM = FindObjectOfType<totalGameManager>();

        //1�b�P�ʂ�Image�摜�̏�����
        oneSecImage.sprite = numberImage[0];
        //10�b�P�ʂ�Image�摜�̏�����
        tenSecImage.sprite = numberImage[0];
        //1���P�ʂ�Image�摜�̏�����
        oneMinImage.sprite = numberImage[0];
        //10���P�ʂ�Image�摜�̏�����
        tenMinImage.sprite = numberImage[0];
    }

    public void UpdateHighScore()
    {
        //�n�C�X�R�A�̕\�L
        if (totalGM.BestTime[0] <= 0.0f)
        {
            //�P�ʂ̃f�t�H���g�\���[��00:00�\��
            oneSecImage.sprite = numberImage[0];
            tenSecImage.sprite = numberImage[0];
            oneMinImage.sprite = numberImage[0];
            tenMinImage.sprite = numberImage[0];
        }

        else
        {
            //�P�ʂ̃����L���O�\��
            //1�b�P�ʂ�Image�摜�̍X�V
            oneSecImage.sprite = numberImage[Mathf.FloorToInt(totalGM.BestTime[0] % 10)];
            //10�b�P�ʂ�Image�摜�̍X�V
            tenSecImage.sprite = numberImage[Mathf.FloorToInt((totalGM.BestTime[0] % 60) / 10)];
            //1���P�ʂ�Image�摜�̍X�V
            oneMinImage.sprite = numberImage[Mathf.FloorToInt(totalGM.BestTime[0] / 60)];
            //10���P�ʂ�Image�摜�̍X�V
            tenMinImage.sprite = numberImage[Mathf.FloorToInt(totalGM.BestTime[0] / 600)];
        }
        //�R���[�`���쓮
        StartCoroutine("StartHighScore");
    }

    //�����L���O�̃X���C�h�ړ�
    private IEnumerator StartHighScore()
    {
        //��ʊO����ړ����Ă���
        if (counter <= 120 && counter >= 0)
        {
            timer.position -= new Vector3(5.0f, 0, 0);
            counter++;
        }
        //������x�ړ������猸������
        if (counter <= 245 && counter >= 120)
        {
            timer.position -= new Vector3(3.0f, 0, 0);
            counter++;
        }
        //�I�����Ă���1�b�ԑҋ@����
        yield return new WaitForSeconds(1);
        yield break;
    }
}

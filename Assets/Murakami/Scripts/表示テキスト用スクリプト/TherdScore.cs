using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TherdScore : MonoBehaviour
{
    //totalGameManager
    private totalGameManager totalGM;
    //SecondScore
    private SecondScore ss;

    //�^�C���\���ŗp���鐔���̉摜
    [SerializeField] private Sprite[] numberImage;
    //1�b�P�ʂ̉摜�\�芷�����s��Image(0�`9)
    [SerializeField] private Image oneSecImage;
    //10�b�P�ʂ̉摜�\�芷�����s��Image(0�`5)
    [SerializeField] private Image tenSecImage;
    //1�b�P�ʂ̉摜�\�芷�����s��Image(0�`9)
    [SerializeField] private Image oneMinImage;
    //10�b�P�ʂ̉摜�\�芷�����s��Image(0�`5)
    [SerializeField] private Image tenMinImage;
    //�e�L�X�g�𓮂������߂ɕK�v�Ȑ錾
    [SerializeField] private RectTransform timer;
    //�e�L�X�g�̓������������v������ϐ�
    private int counter = 0;


    void Start()
    {
        totalGM = FindObjectOfType<totalGameManager>();
        ss = FindObjectOfType<SecondScore>();

        //1�b�P�ʂ�Image�摜�̏�����
        oneSecImage.sprite = numberImage[0];
        //10�b�P�ʂ�Image�摜�̏�����
        tenSecImage.sprite = numberImage[0];
        //1���P�ʂ�Image�摜�̏�����
        oneMinImage.sprite = numberImage[0];
        //10���P�ʂ�Image�摜�̏�����
        tenMinImage.sprite = numberImage[0];
    }

    //�X�R�A�̍X�V
    public void UpdateTherdScore()
    {
        //2�ʃX�R�A�̕\�L
        if (totalGM.BestTime[2] <= 0.0f)
        {
            //�R�ʂ̃f�t�H���g�\���[��00:00�\��
            oneSecImage.sprite = numberImage[0];
            tenSecImage.sprite = numberImage[0];
            oneMinImage.sprite = numberImage[0];
            tenMinImage.sprite = numberImage[0];
        }
        else
        {
            //�R�ʂ̃����L���O�\��"
            //1�b�P�ʂ�Image�摜�̍X�V
            oneSecImage.sprite = numberImage[Mathf.FloorToInt(totalGM.BestTime[2] % 10)];
            //10�b�P�ʂ�Image�摜�̍X�V
            tenSecImage.sprite = numberImage[Mathf.FloorToInt((totalGM.BestTime[2] % 60) / 10)];
            //1���P�ʂ�Image�摜�̍X�V
            oneMinImage.sprite = numberImage[Mathf.FloorToInt(totalGM.BestTime[2] / 60)];
            //10���P�ʂ�Image�摜�̍X�V
            tenMinImage.sprite = numberImage[Mathf.FloorToInt(totalGM.BestTime[2] / 600)];
        }
        StartCoroutine("StartTherdScore");
    }

    //�����L���O�̃X���C�h�ړ�
    private IEnumerator StartTherdScore()
    {
        //��ʊO����ړ����Ă���
        if (counter <= 255 && counter >= 0)
        {
            timer.position -= new Vector3(2.5f, 0, 0);
            counter++;
        }
        //������x�ړ������猸������
        if (counter <= 333 && counter >= 255)
        {
            timer.position -= new Vector3(1.5f, 0, 0);
            counter++;
        }
        //�I�����Ă���1.5�b�ԑҋ@����
        yield return new WaitForSeconds(1.5f);
        //�ҋ@�I����2�ʂ̃n�C�X�R�A�e�L�X�g���ړ�������R���[�`�����쓮������
        ss.UpLoadSecondTime();
        yield break;
    }
}

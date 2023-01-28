using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeTextC : MonoBehaviour
{
    //totalGameOManager
    private totalGameManager totalGM;
    //�\������摜�̔z��
    [SerializeField] private Sprite[] numberImage;
    //1�b�P�ʂɂ����ĕ\������摜
    [SerializeField] private Image oneSecImage;
    //10�b�P�ʂɂ����ĕ\������摜
    [SerializeField] private Image tenSecImage;
    //1���P�ʂɂ����ĕ\������摜
    [SerializeField] private Image oneMinImage;
    //10���P�ʂɂ����ĕ\������摜
    [SerializeField] private Image tenMinImage;
    //�ړ�����^�C���\��
    [SerializeField] private RectTransform timer;
    //�R���[�`���ŗp����ϐ�
    private int counter;
    //TherdScore
    private TherdScore th;

    // Start is called before the first frame update
    void Start()
    {
        totalGM = FindObjectOfType<totalGameManager>();
        //00:00�\���ɂ���@���@������
        oneSecImage.sprite = numberImage[0];
        tenSecImage.sprite = numberImage[0];
        oneMinImage.sprite = numberImage[0];
        tenMinImage.sprite = numberImage[0];

        counter = 0;
        th = FindObjectOfType<TherdScore>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetStartCowrotine()
    {
        //�n�C�X�R�A�̕\�L
        //�Q�[�����Ԃ�1�b�P�ʂ̕\�����s��
        oneSecImage.sprite = numberImage[Mathf.FloorToInt(totalGM.TotalTime % 10)];
        //�Q�[�����Ԃ�10�b�P�ʂ̕\�����s��
        tenSecImage.sprite = numberImage[Mathf.FloorToInt((totalGM.TotalTime % 60) / 10)];
        //�Q�[�����Ԃ�1���P�ʂ̕\�����s��
        oneMinImage.sprite = numberImage[Mathf.FloorToInt(totalGM.TotalTime / 60)];
        //�Q�[�����Ԃ�10���P�ʂ̕\�����s��
        tenMinImage.sprite = numberImage[Mathf.FloorToInt(totalGM.TotalTime / 600)];
        //�R���[�`���J�n
        StartCoroutine("StartText");
    }

    private IEnumerator StartText()
    {
        //1�Q�[���̃^�C����00:00�\���ŉ�ʊO����-x������500����������
        if (counter <= 500)
        {
            timer.position -= new Vector3(2.0f, 0, 0);
            counter++;
        }
        yield return new WaitForSeconds(1);
        //�ݒu����1�b���TherdScore�̃R���[�`�����J�n������
        th.StartCoroutine("StartTherdScore");
        yield break;
    }
}

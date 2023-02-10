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
    private int counter = 0;
    //TherdScore
    private TherdScore th;

    // Start is called before the first frame update
    void Start()
    {
        totalGM = FindObjectOfType<totalGameManager>();
        th = FindObjectOfType<TherdScore>();

        //������
        //1�b�P�ʂ̏������i0�`9�j
        oneSecImage.sprite = numberImage[0];
        //10�b�P�ʂ̏������i0�`5�j
        tenSecImage.sprite = numberImage[0];
        //1���P�ʂ̏������i0�`9�j
        oneMinImage.sprite = numberImage[0];
        //10���P�ʂ̏������i0�`5�j
        tenMinImage.sprite = numberImage[0];
    }

    void Update()
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

    //�����L���O�̃X���C�h�ړ�
    private IEnumerator StartText()
    {
        yield return new WaitForSeconds(1.5f);
        //1�Q�[���̃^�C����00:00�\���ŉ�ʊO����-x������500����������
        if (counter <= 480)
        {
            timer.position -= new Vector3(2.5f, 0, 0);
            counter++;
        }
        //1.5�b�ԑҋ@����
        yield return new WaitForSeconds(1.5f);
        //�ҋ@�I�����TherdScore�̃R���[�`�����J�n������
        th.UpdateTherdScore();
        yield break;
    }
}

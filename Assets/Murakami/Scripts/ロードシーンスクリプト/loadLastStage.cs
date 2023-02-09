using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class loadLastStage : MonoBehaviour
{
    //���[�h���Ԃ̃O���t
    [SerializeField] private Slider loadGazeSlider;
    //�t�F�[�h�C���E�t�F�[�h�A�E�g����p�l���̓����x
    [SerializeField] private GameObject fadePanel;
    //�t�F�[�h�C���E�t�F�[�h�A�E�g����p�l���̓����x
    [SerializeField] private Image fadeAlpha;

    //���݂̃��[�h����
    private float nowGaze = 0.0f;
    //���[�h���Ԃ̏��
    private float maxload = 1.0f;
    //�t�F�[�h�C�������t���O
    private bool didFadeIn = false;
    //���݂̓����x
    private Color pC;

    void Start()
    {
        //�X���C�_�[�̍ő�l�̐ݒ�
        loadGazeSlider.maxValue = maxload;

        //�X���C�_�[�̌��ݒl�̐ݒ�
        loadGazeSlider.value = 0.0f;

        //�p�l���̃C���[�W�̎擾
        fadeAlpha = fadePanel.GetComponent<Image>();

        //���݂̃p�l���̓����x���擾
        pC = fadeAlpha.color;
    }

    void Update()
    {
        //�t�F�[�h�C�����s���Ă��Ȃ��Ȃ�
        if (didFadeIn == false)
        {
            StartCoroutine("FadeIn");
        }
    }

    //�t�F�[�h�C���̏���
    private IEnumerator FadeIn()
    {
        //�����x��0�ȏ�Ȃ�
        while (pC.a >= 0)
        {
            //�w�莞�ԕ������҂�
            yield return new WaitForSeconds(0.7f);
            //�����������x���グ�Ă����i�p�l���𓧖��ɂ��Ă����j
            pC.a -= 0.001f;
            fadeAlpha.color = pC;
        }
        //�t�F�[�h�C�����s��ꂽ�t���O�𗧂Ă�
        didFadeIn = true;
        //�R���[�`���J�n
        StartCoroutine("StartLoadLastStage");
        yield break;
    }

    //����K�X�e�[�W�V�[���̓ǂݍ��݉��o����
    private IEnumerator StartLoadLastStage()
    {
        while (nowGaze <= maxload)
        {
            //�w�莞�ԕ������҂�
            yield return new WaitForSeconds(0.7f);
            //���[�h���̉��o���s�����߂ɃQ�[�W�𑝂₷
            nowGaze += 0.001f;
            //�����ŃQ�[�W�𓮂���
            loadGazeSlider.value = nowGaze;
        }
        //�R���[�`���J�n
        StartCoroutine("FadeOut");
        yield break;
    }

    //�t�F�[�h�A�E�g�̏���
    private IEnumerator FadeOut()
    {
        //�����x��1�ȉ��Ȃ�
        while (pC.a <= 1)
        {
            //�w�莞�ԕ������҂�
            yield return new WaitForSeconds(0.7f);
            //���X�ɓ����x��������i�p�l����F�Z������j
            pC.a += 0.001f;
            fadeAlpha.color = pC;
        }
        //����K�X�e�[�W�V�[�����Ăяo��
        SceneManager.LoadScene("LastScene");
        yield break;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class loadTherdStage : MonoBehaviour
{
    //���݂̃��[�h���� = %
    private float nowGaze = 0.0f;
    //�ő�ǂݍ��ݎ���
    private float maxload = 1.0f;
    //�������X���C�_�[ = �Q�[�W
    [SerializeField] private Slider loadGazeSlider;

    void Start()
    {
        //�X���C�_�[�̍ő�l�̐ݒ�
        loadGazeSlider.maxValue = maxload;

        //�X���C�_�[�̌��ݒl�̐ݒ�
        loadGazeSlider.value = 0f;
    }

    void Update()
    {
        nowGaze += Time.deltaTime;
        loadGazeSlider.value = nowGaze / 5.0f;
        if (nowGaze / 5.0f >= maxload)
        {
            SceneManager.LoadScene("3�K");
        }
    }
}

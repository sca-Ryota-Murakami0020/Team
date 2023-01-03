using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class loadTutorial : MonoBehaviour
{
    private float maxload = 100f;
    Slider loadGazeSlider;
    private float nowGaze = 0.0f;

    void Start()
    {

        loadGazeSlider = GetComponent<Slider>();

        //�X���C�_�[�̍ő�l�̐ݒ�
        loadGazeSlider.maxValue = maxload;

        //�X���C�_�[�̌��ݒl�̐ݒ�
        loadGazeSlider.value = 0f;
    }

    void Update()
    {
        nowGaze += Time.deltaTime;
        loadGazeSlider.value = nowGaze / 3.0f;
        if (loadGazeSlider.value == maxload)
        {
            SceneManager.LoadSceneAsync("�`���[�g���A���V�[��");
            Debug.Log("�`���[�g���A���V�[�����Ăяo����");
        }
    }
}

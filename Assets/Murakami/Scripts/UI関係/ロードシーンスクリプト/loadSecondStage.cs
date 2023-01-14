using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class loadSecondStage : MonoBehaviour
{
    private float nowGaze = 0.0f;
    private float maxload = 1.0f;
    [SerializeField] private Slider loadGazeSlider;
    //private GameObject LoadCanvas;

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
            SceneManager.LoadScene("2�K");
            Debug.Log("���߂ă^�C�g�����Ăяo����");
        }
    }
}

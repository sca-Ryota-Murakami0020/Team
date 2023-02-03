using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class loadTitle : MonoBehaviour
{
    float maxload = 1.0f;
    [SerializeField] private Slider loadGazeSlider = null;
    private float nowGaze = 0.0f;
    //private GameObject LoadCanvas;

    void Start()
    {

        //oadGazeSlider = LoadCanvas.transform.Find("LoadBar").GetComponent<Slider>();

        //�X���C�_�[�̍ő�l�̐ݒ�
        loadGazeSlider.maxValue = maxload;

        //�X���C�_�[�̌��ݒl�̐ݒ�
        loadGazeSlider.value = 0f;
    }

    void Update()
    {
        //���[�h����
        nowGaze += Time.deltaTime;
        loadGazeSlider.value = nowGaze / 5.0f;

        //���[�h�Q�[�W���ő�ɂȂ�����
        if (nowGaze / 5.0f >= maxload)
        {
            SceneManager.LoadScene("TitleScene");
        }
    }
}

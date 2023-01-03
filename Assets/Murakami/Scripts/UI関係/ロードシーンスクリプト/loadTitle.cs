using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class loadTitle : MonoBehaviour
{
    float maxload = 100f;
    public Slider loadGazeSlider = null;
    private float nowGaze = 0.0f;
    private GameObject LoadCanvas;

    void Start()
    {

        loadGazeSlider = LoadCanvas.transform.Find("LoadBar").GetComponent<Slider>();

        //�X���C�_�[�̍ő�l�̐ݒ�
        loadGazeSlider.maxValue = maxload;

        //�X���C�_�[�̌��ݒl�̐ݒ�
        loadGazeSlider.value = 0f;
    }

    void Update()
    {
        nowGaze += Time.deltaTime;
        loadGazeSlider.value = nowGaze / 3.0f;
        Debug.Log("Loading...");
        if (loadGazeSlider.value == maxload)
        {
            SceneManager.LoadSceneAsync("TitleScene");
            Debug.Log("�^�C�g�����Ăяo����");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class loadFrastStage : MonoBehaviour
{
    private float nowGaze = 0.0f;
    private float maxload = 1.0f;
    [SerializeField] private Slider loadGazeSlider;
    //private GameObject LoadCanvas;

    void Start()
    {

        //loadGazeSlider = LoadCanvas.transform.Find("LoadBar").GetComponent<Slider>();

        //スライダーの最大値の設定
        loadGazeSlider.maxValue = maxload;

        //スライダーの現在値の設定
        loadGazeSlider.value = 0f;
    }

    void Update()
    {
        nowGaze += Time.deltaTime;
        loadGazeSlider.value = nowGaze / 5.0f;
        if (nowGaze / 5.0f >= maxload)
        {
            SceneManager.LoadScene("1階");
            Debug.Log("初めてタイトルを呼び出した");
        }
    }
}

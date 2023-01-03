using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class loadFrastStage : MonoBehaviour
{
    private float nowGaze = 0.0f;
    private float maxload = 100f;
    Slider loadGazeSlider;

    void Start()
    {

        loadGazeSlider = GetComponent<Slider>();

        //スライダーの最大値の設定
        loadGazeSlider.maxValue = maxload;

        //スライダーの現在値の設定
        loadGazeSlider.value = 0f;
    }

    void Update()
    {
        nowGaze += Time.deltaTime;
        loadGazeSlider.value = nowGaze / 3.0f;
        if (loadGazeSlider.value == maxload)
        {
            SceneManager.LoadSceneAsync("TitleScene");
            Debug.Log("初めてタイトルを呼び出した");
        }
    }
}

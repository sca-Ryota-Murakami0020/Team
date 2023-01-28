using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class loadTherdStage : MonoBehaviour
{
    //現在のロード時間 = %
    private float nowGaze = 0.0f;
    //最大読み込み時間
    private float maxload = 1.0f;
    //動かすスライダー = ゲージ
    [SerializeField] private Slider loadGazeSlider;

    void Start()
    {
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
            SceneManager.LoadScene("3階");
        }
    }
}

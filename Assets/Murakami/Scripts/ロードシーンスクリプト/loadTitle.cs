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

        //スライダーの最大値の設定
        loadGazeSlider.maxValue = maxload;

        //スライダーの現在値の設定
        loadGazeSlider.value = 0f;
    }

    void Update()
    {
        //ロード処理
        nowGaze += Time.deltaTime;
        loadGazeSlider.value = nowGaze / 5.0f;

        //ロードゲージが最大になったら
        if (nowGaze / 5.0f >= maxload)
        {
            SceneManager.LoadScene("TitleScene");
        }
    }
}

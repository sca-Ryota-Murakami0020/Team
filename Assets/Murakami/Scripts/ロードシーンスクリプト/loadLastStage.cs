using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class loadLastStage : MonoBehaviour
{
    //ロード時間のグラフ
    [SerializeField] private Slider loadGazeSlider;
    //フェードイン・フェードアウトするパネルの透明度
    [SerializeField] private GameObject fadePanel;
    //フェードイン・フェードアウトするパネルの透明度
    [SerializeField] private Image fadeAlpha;

    //現在のロード時間
    private float nowGaze = 0.0f;
    //ロード時間の上限
    private float maxload = 1.0f;
    //フェードインしたフラグ
    private bool didFadeIn = false;
    //現在の透明度
    private Color pC;

    void Start()
    {
        //スライダーの最大値の設定
        loadGazeSlider.maxValue = maxload;

        //スライダーの現在値の設定
        loadGazeSlider.value = 0.0f;

        //パネルのイメージの取得
        fadeAlpha = fadePanel.GetComponent<Image>();

        //現在のパネルの透明度を取得
        pC = fadeAlpha.color;
    }

    void Update()
    {
        //フェードインが行われていないなら
        if (didFadeIn == false)
        {
            StartCoroutine("FadeIn");
        }
    }

    //フェードインの処理
    private IEnumerator FadeIn()
    {
        //透明度が0以上なら
        while (pC.a >= 0)
        {
            //指定時間分だけ待つ
            yield return new WaitForSeconds(0.7f);
            //少しずつ透明度を上げていく（パネルを透明にしていく）
            pC.a -= 0.001f;
            fadeAlpha.color = pC;
        }
        //フェードインが行われたフラグを立てる
        didFadeIn = true;
        //コルーチン開始
        StartCoroutine("StartLoadLastStage");
        yield break;
    }

    //屋上階ステージシーンの読み込み演出処理
    private IEnumerator StartLoadLastStage()
    {
        while (nowGaze <= maxload)
        {
            //指定時間分だけ待つ
            yield return new WaitForSeconds(0.7f);
            //ロード中の演出を行うためにゲージを増やす
            nowGaze += 0.001f;
            //ここでゲージを動かす
            loadGazeSlider.value = nowGaze;
        }
        //コルーチン開始
        StartCoroutine("FadeOut");
        yield break;
    }

    //フェードアウトの処理
    private IEnumerator FadeOut()
    {
        //透明度が1以下なら
        while (pC.a <= 1)
        {
            //指定時間分だけ待つ
            yield return new WaitForSeconds(0.7f);
            //徐々に透明度を下げる（パネルを色濃くする）
            pC.a += 0.001f;
            fadeAlpha.color = pC;
        }
        //屋上階ステージシーンを呼び出す
        SceneManager.LoadScene("LastScene");
        yield break;
    }
}

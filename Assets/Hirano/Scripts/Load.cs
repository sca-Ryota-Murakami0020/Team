using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Load : MonoBehaviour
{

    //private AsyncOperation async;
    //public GameObject LoadingUi;
    //public Slider Slider;
    public Image maxGaze;
    private float nowGaze = 0.0f;

    void Update()
    {
        nowGaze += Time.deltaTime;
        maxGaze.fillAmount = nowGaze / 3.0f;
        if((float)nowGaze % 3.0f <= 0)
        {
            SceneManager.LoadSceneAsync("FirstScene");
        }
    }

    /*public void LoadNextScene()
    {
        //LoadingUi.SetActive(true);
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        async = SceneManager.LoadSceneAsync("FirstScene");

        while (!async.isDone)
        {
            //Slider.value = async.progress;
            nowGaze = Time.deltaTime;
            maxGaze.fillAmount = (float) nowGaze/100.0f;
            yield return null;
        }
    }*/
}
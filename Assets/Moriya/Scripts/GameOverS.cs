using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverS : MonoBehaviour
{
    //ゲームオーバーシーンのスプリクト
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //ボタンごとにロードするシーンを使い分ける
    public void Retry()
    {
        SceneManager.LoadScene("LoadBill");
    }

    public void Titleback()
    {
        SceneManager.LoadScene("AnyLoadTitle");
    }
}

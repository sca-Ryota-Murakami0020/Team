using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryButtonC : MonoBehaviour
{
    //ゲームをリトライする
    public void RetryGame()
    {
        //ビルステージを呼び出すロードシーンを呼び出す
        SceneManager.LoadScene("LoadBill");
    }
}

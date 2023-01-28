using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleNextStege : MonoBehaviour
{
    //タイトル画面のスプリクト

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextStege()
    {
      //操作説明のシーンにいく
      SceneManager.LoadScene("操作説明"); 
    }
}

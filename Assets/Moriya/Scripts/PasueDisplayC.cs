using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PasueDisplayC : MonoBehaviour
{
    //ポーズ関係のスプリクト
    private Player playerC;

    //ポーズが開いたかのフラグ
    private bool menuFlag = false;

    //操作説明開いたときのフラグ
    private bool operationExpFlag = false;

    [SerializeField]
    //ポーズした時に表示するUIのプレハブ
    private GameObject pauseUIPrefab;
    [SerializeField]
    //操作説明UIのプレハブ
    private GameObject playOperatePrafab;
    //ポーズUIのインスタンス
    private GameObject pauseUIInstance;
    //操作説明UIのインスタンス
    private GameObject playOperateUIInstance;

    //ゲッターセッター
    public bool MenuFlag
    {
        get {return this.menuFlag; }
        set {this.menuFlag = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        playerC = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
      
        #region//メニュー画面が開く処理
        if (Input.GetKeyDown("q"))
        {
            if(pauseUIInstance == null)
            {
                pauseUIInstance = GameObject.Instantiate(pauseUIPrefab) as GameObject;
                Time.timeScale=0f;
                menuFlag = true;
            }
            else
            {
                menuFlag = false;
                Destroy(playOperateUIInstance);
                Destroy(pauseUIInstance);
                Time.timeScale = 1f;
            }
        }
        //メニューが開かれたら
        if(menuFlag == true)
        {
            PauseMenu();
        }
        #endregion
    }

    #region//メニュー画面が開いている時の処理
    private void PauseMenu()
    {
        if (Input.GetKey(KeyCode.Escape)) 
        { 
             /*#if UNITY_EDITOR
             UnityEditor.EditorApplication.isPlaying = false;
            //エディタ上の動作
            #else
            Application.Quit();
            //エディタ以外の操作
            #endif*/
              // SceneManager.LoadScene("FirstScene");
            Time.timeScale = 1f;
            menuFlag = false;
            ResetCommand();
        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            //StartCoroutine(PlayerXplanation());

            Destroy(pauseUIInstance);
            //操作説明開く
            playOperateUIInstance = GameObject.Instantiate(playOperatePrafab) as GameObject;
            operationExpFlag = true;
            Debug.Log(playOperateUIInstance);
        }
        if(operationExpFlag == true && Input.GetKeyUp(KeyCode.Tab))
        {
            operationExpFlag = false;
            Destroy(playOperateUIInstance);
            pauseUIInstance = GameObject.Instantiate(pauseUIPrefab) as GameObject;
        }

    }
    #endregion

   /* private IEnumerator PlayerXplanation()
    {

    }*/

    private void ResetCommand()
    {
        playerC.PlayerMaxHp = 3;
        playerC.PlayerHp = playerC.PlayerMaxHp;
        playerC.PlayerSpeed = 10.0f;
        playerC.JumpCount = 0;
    }

}

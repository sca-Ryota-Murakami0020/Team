using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PasueDisplayC : MonoBehaviour
{
    //ポーズ関係のスプリクト

    //ゲーマネを呼び出している
    private totalGameManager totalGM;

    //ポーズが開いたかのフラグ
    private bool menuFlag = false;

    //操作説明開いたときのフラグ
    private bool operationExpFlag = false;

    //一回だけ入るときのフラグ
    private bool onlyFlag = false;

    [SerializeField]
    //ポーズした時に表示するUIのプレハブ
    private GameObject pauseUIPrefab;
    //ポーズUIのインスタンス
    private GameObject pauseUIInstance;
    //操作説明UIのインスタンス
    private GameObject playOperateUIInstance;
    [SerializeField]
    //操作説明UIのプレハブ
    private GameObject playOperatePrafab;

    //操作説明フラグを立てるフラグ
    private bool openManual = false;
    //ゲームに戻る動作を行うフラグ
    private bool returnGame = false;

    //ゲッターセッター
    public bool MenuFlag
    {
        get {return this.menuFlag; }
        set {this.menuFlag = value; }
    }

    public bool OpenManual
    {
        get { return this.openManual; }
        set { this.openManual = value; }
    }

    public bool ReturnGame
    {
        get { return this.returnGame; }
        set { this.returnGame = value; }
    }

    public bool OnlyFlag
    {
        get { return this.onlyFlag;}
        set { this.onlyFlag = value;}
    }

    // Start is called before the first frame update
    void Start()
    {
        //ゲーマネ呼び出し
        totalGM = FindObjectOfType<totalGameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //コントローラ入力からの入力 縦軸 を取得
        //float verticalInput = Input.GetAxis("Vertical");

        #region//メニュー画面が開く処理

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            //ポーズ画面出す
            if (pauseUIInstance == null && menuFlag == false)
            {
                pauseUIInstance = GameObject.Instantiate(pauseUIPrefab) as GameObject;
                Time.timeScale=0f;
                menuFlag = true;
            }
            /*
            //ポーズ画面を消す
            else if(pauseUIInstance != null && returnGame == true)
            {
                menuFlag = false;
                Destroy(playOperateUIInstance);
                Destroy(pauseUIInstance);
                Time.timeScale = 1f;
                openManual = false;
                returnGame = false;
            }
            */
            //Debug.Log("確認:" + pUC.OpenManual);
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
        //escキーおしたとき
        if (Input.GetKey(KeyCode.Escape)) 
        { 
             #if UNITY_EDITOR
             UnityEditor.EditorApplication.isPlaying = false;
            //エディタ上の動作
            #else
            Application.Quit();
            //エディタ以外の操作
            #endif
            ResetCommand();
            Time.timeScale = 1f;
            menuFlag = false;
        }
    }

    //操作画面の呼び出し
    public void DisplayManual()
    {
        onlyFlag = true;
        //操作説明開くコルーチン
        Destroy(pauseUIInstance);
        playOperateUIInstance = GameObject.Instantiate(playOperatePrafab) as GameObject;
        operationExpFlag = true;

    }

    //ゲームに戻る
    public void CloseMenu()
    {
        menuFlag = false;
        Destroy(playOperateUIInstance);
        Destroy(pauseUIInstance);
        Time.timeScale = 1f;
        openManual = false;
        returnGame = false;
    }

    //操作説明画面を閉じる
    public void CloseManual()
    {
        //Tabキーから離れてた時かつ操作画面UIが出ていたら
        //ポーズ画面UIを出し、操作画面UIを消す
        onlyFlag = false;
        operationExpFlag = false;
        openManual = false;
        Destroy(playOperateUIInstance);
        pauseUIInstance = GameObject.Instantiate(pauseUIPrefab) as GameObject;
    }
    #endregion

        /*
        //操作説明の際のコルーチン
        private IEnumerator PlayerXplanation()
        {
            while (true)
            {
                //ポーズ画面を消して、操作画面UIを出す
                if (onlyFlag == true)
                {
                    Destroy(pauseUIInstance);
                    playOperateUIInstance = GameObject.Instantiate(playOperatePrafab) as GameObject;
                    operationExpFlag = true;
                    onlyFlag = false;
                }
                Debug.Log(playOperateUIInstance);
                //Tabキーから離れてた時かつ操作画面UIが出ていたら
                if (operationExpFlag == true && Input.GetKeyUp (KeyCode.Tab))
                {
                    //ポーズ画面UIを出し、操作画面UIを消す
                    operationExpFlag = false;
                    openManual = false;
                    Destroy(playOperateUIInstance);
                    pauseUIInstance = GameObject.Instantiate(pauseUIPrefab) as GameObject;
                    Debug.Log(pauseUIInstance);
                    yield break;
                }
               yield return null; 
            }
        }
        */

    //リセットするときに値を変える
    private void ResetCommand()
    {
        totalGM.PlayerHp = 3;
        totalGM.PlayerIC = 0;
        totalGM.TimeCounter = false;
    }

}

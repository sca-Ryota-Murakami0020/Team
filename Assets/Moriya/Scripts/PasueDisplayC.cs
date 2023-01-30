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
    //メニュー画面のカーソルに合わせて動作するために定義する
    private PauseUIC pUC;

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

    // Start is called before the first frame update
    void Start()
    {
        //ゲーマネ呼び出し
        totalGM = FindObjectOfType<totalGameManager>();
    }

    // Update is called once per frame
    void Update()
    {
      
        #region//メニュー画面が開く処理

        //メニューが開かれていないときかつQキーおしたとき


     
        if (Input.GetKeyDown("q"))
        {
            //ここでスプリクトを見つける
            pUC = FindObjectOfType<PauseUIC>();
            //ポーズ画面出す
            if (pauseUIInstance == null && menuFlag == false)

            {
                pauseUIInstance = GameObject.Instantiate(pauseUIPrefab) as GameObject;
                Time.timeScale=0f;
                menuFlag = true;
            }
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
        /*
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
            SceneManager.LoadScene("LoadBill");
            Time.timeScale = 1f;
            menuFlag = false;
        }*/

        //tabキー押したとき && pUC.OpenManual == true
        if (Input.GetKeyDown(KeyCode.Tab) && openManual == true)
        {
            onlyFlag = true;
            //操作説明開くコルーチン
            StartCoroutine("PlayerXplanation");
        }

        /*if(Input.GetKey(KeyCode.T)){
           ResetCommand();
           SceneManager.LoadScene("TitleScene");
           Time.timeScale = 1f;
           menuFlag = false;
        }*/
    }
    #endregion

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

    //リセットするときに値を変える
    private void ResetCommand()
    {
        totalGM.PlayerHp = 3;
        totalGM.PlayerIC = 0;
        totalGM.TimeCounter = false;
        /*for (int i = 0; i < tatalGM.PlayerHp; i++)
        {
            tatalGM.HeartArray[i].gameObject.SetActive(true);
        }*/
    }

}

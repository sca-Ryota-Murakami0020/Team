using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseUIC : MonoBehaviour
{
    [SerializeField] private Image[] image;
    private PasueDisplayC pDC;

    //操作説明フラグを立てるフラグ
    private bool openManual = false;
    //ゲームに戻る動作を行うフラグ
    private bool returnGame = false;

    //プロパティ
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
        pDC = FindObjectOfType<PasueDisplayC>();
        //開いた時にはすぐに操作画面のUIを見れるようにするための処理をここで書く
        //「操作説明」に合わせたカーソルの表示を行う
        image[0].enabled = true;
        //「ゲームに戻る」に合わせたカーソルの非表示を行う
        image[1].enabled = false;
        //ここで操作説明の画面を呼び出せるようにフラグを立てる
        openManual = true;
    }

    // Update is called once per frame
    void Update()
    {
        //メニューが開かれている時に関数を呼び出す
        if (pDC.MenuFlag == true)
        {
            SelectAction();
        }
    }

    public void SelectAction()
    {
        //Oキー（Lスティック上入力）での処理
        if (Input.GetKeyDown(KeyCode.O))
        {
            //「操作説明」に合わせたカーソルの表示を行う
            image[0].enabled = true;
            //「ゲームに戻る」に合わせたカーソルの非表示を行う
            image[1].enabled = false;
            //ここで操作説明の画面を呼び出せるようにフラグを立てる
            openManual = true;
            //「ゲームに戻る」を選択した後に「操作説明」のアクションを行う際に
            //バグが起きないようにするために、ここでreturnGameの値をfalseにしておく
            returnGame = false;
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            //「操作説明」に合わせたカーソルの非表示を行う
            image[0].enabled = false;
            //「ゲームに戻る」に合わせたカーソルの表示を行う
            image[1].enabled = true;
            //ゲームに戻るアクションが行えるようにするためのフラグを立てる
            returnGame = true;
            //「操作説明」を選択した後に「ゲームに戻る」のアクションを行う際に
            //バグが起きないようにするために、ここでopenManualの値をfalseにする
            openManual = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class BillCameraC : MonoBehaviour
{
    //参照先スクリプト
    //PasueDisplayC
    private PasueDisplayC pDC;
    //変数関係 
    //カメラのポジション
    [Header("カメラの位置")][SerializeField]
    private GameObject[] cameraPos;
    //カメラの座標を記憶する変数
    private Vector3[] cameraPosVec;
    //カメラの角度を記憶する変数
    private Quaternion[] cameraPosRot;
    //カメラの番地
    private int cameraPosNumber;
    //カメラがすぐに動かない様にするためのフラグ
    private bool canMoveCamera = true;

    void Start()
    {
        //スクリプトの定義
        pDC = FindObjectOfType<PasueDisplayC>();

        //配列数の定義化
        cameraPosVec = new Vector3 [4];
        cameraPosRot = new Quaternion [4];
        for(int count = 0; count < 4; count++)
        {           
            //座標の記憶
            cameraPosVec[count] = cameraPos[count].transform.position;
            Debug.Log("問題点");
            //角度の記憶
            cameraPosRot[count] = cameraPos[count].transform.rotation;
        }

        //位置の初期化
        //カメラ
        cameraPosNumber = 0;
        this.transform.position = cameraPosVec[cameraPosNumber];
        this.transform.rotation = cameraPosRot[cameraPosNumber];
    }

    public void CameraMover(string oldName, string getName)
    { 
        //string型のローカル変数changeを定義する
        //ー＞この関数内で行うswitch文による処理を行う際に使う変数として用いる
        string change = null;
        //引数に格納している名前が一致する
        // = 前のカメラの位置に移動する
        if (oldName == getName && (getName != "Empty" || getName != "Start")) change = "Back";
        //引数に格納している名前が一致しない、且つ一番最初に触れたのがEmptyorStartではないなら
        // = 次のカメラの位置に移動する
        if(oldName != getName && (getName != "Empty" || getName != "Start")) change = "Next";
        //もし取得した名前がEmptyorStartなら何もしない様にする
        //ー＞この後のカメラの動きを正常なものにする必要はあるから。
        //また、ゲーム開始時に踏むオブジェクトが必ずStartになっているから
        if (oldName == getName && (getName == "Empty" || getName == "Start")) change = "Empty";
        switch (change)
        {
            case "Back":
                BackCamera();
                break;
            case "Next":
                NextCamera();
                break;
            //何もしない
            case "Empty":
                break;
            default:
                break;
        }
    }

    public void NextCamera()
    {
        if(canMoveCamera)
        {
            //次のカメラのポジションに移動する
            cameraPosNumber++;
            this.transform.position = cameraPosVec[cameraPosNumber];
            //変更先のオブジェクトはカメラの向くべき角度を持っているので角度も変更する。
            this.transform.rotation = cameraPosRot[cameraPosNumber];
            Debug.Log("Next");
        }       
    }

    public void BackCamera()
    {
        if(canMoveCamera)
        {
            //前のカメラのポジションに移動する
            cameraPosNumber--;
            this.transform.position = cameraPosVec[cameraPosNumber];
            //変更先のオブジェクトはカメラの向くべき角度を持っているので角度も変更する。
            this.transform.rotation = cameraPosRot[cameraPosNumber];
            Debug.Log($"Back");
        }
    }

    private IEnumerator changeFlag()
    {
        //カメラの行動を2秒間不可にする
        canMoveCamera = false;
        yield return new WaitForSeconds(3);
        //2秒後に行動可能にする
        canMoveCamera = true;
        yield return null;
    }
}

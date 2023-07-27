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
    //カメラに映す対象
    [Header("追いかける対象（プレイヤー）")][SerializeField] 
    private GameObject player = null;
    //Playerの位置
    private Vector3 playerPosition;
    //現在のカメラのポジション
    private Vector3 cameraPosition;
    //カメラとプレイヤーとの距離
    private Vector3 cpDistance;
    //次のカメラに行く処理を行うトリガーになるオブジェクト
    private GameObject oldCameraObject;
    //カメラの移動方向を判別するオブジェクト
    private GameObject disObjecto;
    //移動方向を判別するためのオブジェクトの名前を記憶する
    private string disObjectoName;
    //移動前のカメラ移動オブジェクトの名前
    private string oldDisObjectName;
    //動く方向のステータス
    enum STATE
    {
        NULL,
        X,
        Z,
        EMUP
    };
    //ステータスを管理する変数
    private STATE state;
    //現在の場面番号のステータス
    enum STAGE
    {
        Null,
        One,
        Two,
        Three,
        Fuor
    };
    //場面番号のステータス型の変数
    private STAGE stage;


    //プロパティ
    public GameObject OldCamera
    {
        get { return this.oldCameraObject;}
        set { this.oldCameraObject = value;}
    }

    public string DisObjectoName 
    {
        get { return this.disObjectoName;}
        set { this.disObjectoName = value;}
    }

    void Start()
    {
        //スクリプトの定義
        pDC = FindObjectOfType<PasueDisplayC>();
        //変数関係の初期化
        state = STATE.Z;
        //場面番号変数の初期化
        stage = STAGE.One;
        //位置の初期化
        //プレイヤー
        playerPosition = player.transform.position;
        //カメラ
        cameraPosition = this.transform.position;
        cpDistance = cameraPosition - playerPosition;
        cameraPosition += cpDistance;
        this.transform.position = cameraPosition;     
    }

    void Update()
    {
        //ポーズしていない時
        if (pDC.MenuFlag == false)
        {
            //常にPlayerの位置とカメラの位置を取得する
            playerPosition = this.player.transform.position;
            //プレイヤーがワールド座標のz方向に移動しているなら
            if(state == STATE.Z)
            {
                CameraMoveForStopX();
            }
            //プレイヤーがワールド座標のx方向に移動しているなら
            if(state == STATE.X)
            {
                CameraMoveForStopZ();
            }
        }
        //Debug.Log(cpDistance);
    }

    public void CameraMover(string oldName, string getName)
    { 
        //string型のローカル変数changeを定義する
        //ー＞この関数内で行うswitch文による処理を行う際に使う変数として用いる
        string change = null;
        //引数に格納している名前が一致する
        // = 前のカメラの位置に移動する
        if (oldName == getName) change = "Back";
        //引数に格納している名前が一致しない、且つ一番最初に触れたのがEmptyではないなら
        // = 次のカメラの位置に移動する
        if(oldName != getName && getName != "Empty") change = "Next";
        //もし取得した名前がEmptyなら
        //ー＞この後のカメラの動きを正常なものにする必要はあるから。
        if (oldName != getName && getName == "Empty") change = "Empty";
        switch (change)
        {
            case "Back":
                BackCamera();
                break;
            case "Next":
                NextCamera();
                break;
            case "Empty":
               NextCamera();
                break;
            default:
                break;
        }

        if(stage.ToString() == getName)
        {
            
        }
    }

    private void CameraMoveForStopX()
    {
        //this.transform.position = new Vector3(this.cameraPosition.x, playerPosition.y, playerPosition.z);
        //cameraPosition += cpDistance;
        this.transform.position = cameraPosition;
        //Debug.Log("Z座標の移動" + state);
    }

    private void CameraMoveForStopZ()
    {
        //this.transform.position = new Vector3(playerPosition.x, playerPosition.y, this.cameraPosition.z);
        //cameraPosition += cpDistance;
        this.transform.position = cameraPosition;
        //Debug.Log("X座標の移動" + state);
    }

    public void NextCamera()
    {
        //プレイヤーのy方向を回転軸として90度回転する
        //transform.rotation = Quaternion.AngleAxis(this.transform.rotation.y - 90, player.transform.up);
        if (state == STATE.X)
        {
            state = STATE.Z;
        }
        if (state == STATE.Z)
        {
            state = STATE.X;
        }
        Debug.Log("Next");
    }

    public void BackCamera()
    {
        //プレイヤーのy方向を回転軸として-90度回転する
        //transform.rotation = Quaternion.AngleAxis(this.transform.rotation.y + 90, player.transform.up);
        //プレイヤーの移動方向を更新する
        if(state == STATE.X)
        {
            state = STATE.Z;
        }
        if(state == STATE.Z)
        {
            state = STATE.X;
        }
        Debug.Log($"Back");
    }
}

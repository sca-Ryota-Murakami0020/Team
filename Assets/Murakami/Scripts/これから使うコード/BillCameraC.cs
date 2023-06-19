using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class BillCameraC : MonoBehaviour
{
    //使用するバーチャルカメラ一覧
    [SerializeField]
    private CinemachineVirtualCamera[] cameraList;
    //次のカメラに行く処理を行うトリガーになるオブジェクト
    private GameObject oldCameraObject;
    //カメラに映す対象
    [SerializeField] private GameObject player = null;
    //Playerの位置
    private Vector3 playerPosition;
    //現在のカメラのポジション
    private Vector3 cameraPosition;
    //前のカメラの座標
    private Vector3 oldCameraPosition;
    //PasueDisplayC
    private PasueDisplayC pDC;
    //BillCameraMveC
    private BillCameraMveC bCM;
    //カメラの移動方向を判別するオブジェクト
    private GameObject disObjecto;
    //移動方向を判別するためのオブジェクトの名前を記憶する
    private string disObjectoName;


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
        pDC = FindObjectOfType<PasueDisplayC>();
        bCM = FindObjectOfType<BillCameraMveC>();

    }

    private void Update()
    {
        //ポーズしていない時
        if (pDC.MenuFlag == false)
        {
            //常にPlayerの位置とカメラの位置を取得する
            playerPosition = this.player.transform.position;
        }
    }

    private void CameraMover(int number)
    {
        switch(number)
        {
            case 0:
                CameraMoveForStopX();
                break;
            case 1:
                CameraMoveForStopZ();
                break;
            case 2:
                CameraMoveForStopX();
                break;
            case 3:
                CameraMoveForStopZ();
                break;
            default:
                return;

        }
    }

    private void CameraMoveForStopX()
    {
        this.transform.position = new Vector3(this.cameraPosition.x, playerPosition.y, playerPosition.z);
        Debug.Log("Z座標の移動");
    }

    private void CameraMoveForStopZ()
    {
        this.transform.position = new Vector3(playerPosition.x, playerPosition.y, this.cameraPosition.z);
        Debug.Log("X座標の移動");
    }

    public void NextCamera()
    {
        //移動前のカメラを記憶する
        var oldVcamPrev = cameraList[cameraIndex];
        oldVcamPrev.Priority = unSelectCameraPriority;
        oldCameraPosition = oldVcamPrev.transform.position;

        //もしカメラのインデックスがカメラの要素数を超えていたら
        if (++cameraIndex > cameraList.Length)
        {
            cameraIndex = 0;
        }

        //次の面のカメラに移る
        var nowVcamPrev = cameraList[cameraIndex];
        nowVcamPrev.Priority = selectCameraPriority;
        Debug.Log("次の面に移る");
    }

    public void BackCamera()
    {
        //移動前のカメラを記憶する
        var oldVcamPrev = cameraList[cameraIndex];
        oldVcamPrev.Priority = unSelectCameraPriority;
        oldCameraPosition = oldVcamPrev.transform.position;

        //もしカメラのインデックスがカメラの要素数を下回る
        if (--cameraIndex < 0)
        {
            cameraIndex = 3;
        }

        //次の面のカメラに移る
        var nowVcamPrev = cameraList[cameraIndex];
        nowVcamPrev.Priority = selectCameraPriority;
        Debug.Log("前の面に移る");
    }
}

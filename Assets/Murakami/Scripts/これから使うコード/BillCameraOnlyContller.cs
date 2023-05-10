using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillCameraOnlyContller : MonoBehaviour
{
    //回転の中心点
    [SerializeField] private GameObject centerPosition;
    //回転軸
    [SerializeField] private GameObject axisPosition;
    //円運動周期
    [SerializeField] private float period;
    //カメラの移動をワールド座標基準で縦移動にする(1,3面でのカメラ移動)
    private bool otFlag = true;
    //カメラの移動をワールド座標基準で横移動にする(2,4面でのカメラ移動)
    private bool sfFlag = true;
    //PasueDisplayC
    private PasueDisplayC pDC;
    //BillCameraMveC
    private BillCameraMveC bCM;
    //プレイヤーの位置
    private Vector3 playerPosition;
    //カメラの位置
    private Vector3 cameraPosition;
    //プレイヤー
    [SerializeField] private GameObject player;

    #region//プロパティ
    public bool OTFlag
    {
        get { return this.otFlag;}
        set { this.otFlag = value;}
    }

    public bool SFFlag
    {
        get { return this.sfFlag;}
        set { this.sfFlag = value;}
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        pDC = FindObjectOfType<PasueDisplayC>();
        bCM = FindObjectOfType<BillCameraMveC>();
    }

    // Update is called once per frame
    void Update()
    {
        //ポーズしていない時
        if (pDC.MenuFlag == false)
        {
            //常にPlayerの位置とカメラの位置を取得する
            playerPosition = this.player.transform.position;
            cameraPosition = this.transform.position;

            if(otFlag == true && sfFlag == false)
            {
                this.transform.position = new Vector3(this.cameraPosition.x, playerPosition.y, playerPosition.z);
                Debug.Log("Z座標の移動");
            }

            if(otFlag == false && sfFlag == true)
            {
                this.transform.position = new Vector3(playerPosition.x, playerPosition.y, this.cameraPosition.z);
                Debug.Log("X座標の移動");
            }
        }
    }

    public void GotoNextCameraPosition()
    {

    }

    public void GotoBeforeCameraPosition()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillOnly : MonoBehaviour
{
    //カメラに映す対象
    [SerializeField] private GameObject player = null;
    //Playerの位置
    private Vector3 playerPosition;
    //次のカメラの位置になる場所
    [SerializeField] private GameObject[] nextCameraPosition;
    //回転軸を与えるオブジェクト
    [SerializeField] private GameObject axisObject;
    //回転軸を与えるオブジェクトのポジション
    private Vector3 axisObjectPosition;

    //今映している面の番号
    private int nowSceneNumber = 1;
    //1つ前に映していた面の番号
    private int oldSceneNumber = 1;
    //次の場面に移動してる処理を行っている
    private bool moveFlag = false;

    //totalGameManager
    private totalGameManager tGM;
    //PasueDisplayC
    private PasueDisplayC pDC;

    #region//プロパティ
    public int OldSN
    {
        get { return this.oldSceneNumber;}
        set { this.oldSceneNumber = value;}
    }

    public int NowSN
    {
        get { return this.nowSceneNumber;}
        set { this.nowSceneNumber = value;}
    }
    #endregion

    void Start()
    {
        tGM = FindObjectOfType<totalGameManager>();
        pDC = FindObjectOfType<PasueDisplayC>();
        axisObjectPosition = axisObject.transform.position;
        this.transform.position = nextCameraPosition[0].transform.position;
    }

    private void Update()
    {
        //常にPlayerの位置を取得する
        playerPosition = this.player.transform.position;
        //旋回処理していない間かつポーズしていない時
        if (moveFlag == false && pDC.MenuFlag == false)
        {
            //映してる面がスタートした時の面orその反対の（最初の壁ジャンプがある）面なら
            if (nowSceneNumber == 1 || nowSceneNumber == 3)
            {
                this.transform.position = new Vector3(this.transform.position.x, playerPosition.y, playerPosition.z);
            }

            //映している面がスタートした時から次の面or鍵やゴールが配置されている面なら
            if (nowSceneNumber == 2 || nowSceneNumber == 4)
            {
                this.transform.position = new Vector3(playerPosition.x, playerPosition.y, this.transform.position.z);
            }
        }
    }

    //カメラが次にどこの面を映すかの判断
    public void Judge(int number)
    {
        Debug.Log("引き渡し完了");
        switch(number)
        {
            case 1:
                oldSceneNumber = nowSceneNumber;
                nowSceneNumber = number;
                Debug.Log("判定終了");
                StartCoroutine("FirstMoveCamera");
                break;
            case 2:
                oldSceneNumber = nowSceneNumber;
                nowSceneNumber = number;
                Debug.Log("判定終了");
                StartCoroutine("SecondMoveCamera");
                break;
            case 3:
                oldSceneNumber = nowSceneNumber;
                nowSceneNumber = number;
                Debug.Log("判定終了");
                StartCoroutine("TherdMoveCamera");
                break;
            case 4:
                oldSceneNumber = nowSceneNumber;
                nowSceneNumber = number;
                Debug.Log("判定終了");
                StartCoroutine("ForceMoveCamera");
                break;
        }
    }

    //スタートした時に映していたステージに移動する
    private IEnumerator FirstMoveCamera()
    {
        yield return new WaitForSeconds(0.5f);
        //移動の処理
        while(this.transform.position != nextCameraPosition[1].transform.position)
        {
            if (oldSceneNumber == 2 && this.transform.rotation.y <= 0)
            {
                transform.RotateAround(axisObjectPosition, Vector3.up, -2.0f);
            }

            if(oldSceneNumber == 4 && this.transform.rotation.y >= -180)
            {
                transform.RotateAround(axisObjectPosition, Vector3.up, 2.0f);
            }
        }
        yield break;
    }

    //スタートしてから次に映る面に移動する
    private IEnumerator SecondMoveCamera()
    {
        yield return new WaitForSeconds(0.5f);
        //移動の処理
        while (this.transform.position != nextCameraPosition[0].transform.position)
        {
            if (oldSceneNumber == 2 && this.transform.rotation.y <= 0)
            {
                transform.RotateAround(axisObjectPosition, Vector3.up, 2.0f);
            }

            if (oldSceneNumber == 4 && this.transform.rotation.y >= -180)
            {
                transform.RotateAround(axisObjectPosition, Vector3.up, 2.0f);
            }
        }
        yield break;
    }

    //最初に壁ジャンプを使う面に移動する
    private IEnumerator TherdMoveCamera()
    {
        yield return new WaitForSeconds(0.5f);
    }

    //鍵とゴールが設置してある面に移動する
    private IEnumerator ForceMoveCamera()
    {
        yield return new WaitForSeconds(0.5f);
    }
}

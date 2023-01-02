using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraC : MonoBehaviour
{
    #region//stageが変わっても変わらない変数
    //視点となるオブジェクト
    [SerializeField] private GameObject Player = null;
    //旋回した時のx座標
    private float xpos;
    //旋回した時のｚ座標
    private float zpos;
    //実際にカメラを向ける座標//カメラとプレイヤーとの距離
    private Vector3 D= Vector3.zero;//Lookpos
    //Playerを追従しないでカメラの向きだけ変える範囲
    private float lookPlayerdistance = 0.1f;
    //横方向のマウスの移動量
    private float mousex;
    //縦方向のマウスの移動量
    private float mousey;
    #endregion

    #region//stageによって変わる数値
    //PLayerを追従する速度
    [SerializeField] private float cameraSpeed;// = 4.0f;//=followSmooth 
    //視点からカメラまでの距離
    [SerializeField] private float cameraDistance;// = 1.5f;
    //デフォルトの高さ
    [SerializeField] private float cameraHeight;// = 1.0f;
    //現在のカメラの高さ
    [SerializeField] private float nowCameraHeight;// = 1.0f;//currentCameraHeight = 1.0f;
    //視点からカメラの距離の遊び
    [SerializeField] private float cPDistance;// = 0.3f;//cameraPlayDiatance = 0.3f; 
    //離れる時の速度
    [SerializeField] private float leaveCamera;// = 20.0f;//leaveSmooth
    //カメラの最低高度
    [SerializeField] private float cameraHeightMin;// = -5.0f;
    //カメラの最高高度
    [SerializeField] private float cameraHeightMax;// = 8.5f;
    #endregion


    //視点とカメラ座標を随時更新
    void Update()
    {
        if (Player == null) return;
        //マウスの移動量を取得
        mousex = Input.GetAxis("Mouse X");
        mousey = Input.GetAxis("Mouse Y");

        if (Mathf.Abs(mousex) > 0.019f || Mathf.Abs(mousey) > 0.06f)
        {
            Roll(mousex, mousey);
        }

        UpdateLookPosition();
        UpdateCameraPosition();
        this.transform.LookAt(D);
    }

    //カメラ視点の制御pl.FuckFlag == true && 
    public void UpdateLookPosition()
    {
        //目標の視点と現在の視点の距離を求める
        Vector3 vec = Player.transform.position - D;
        float distance = vec.magnitude;
        if (distance > lookPlayerdistance)
        {
            //一定範囲を超えたら目標に視点に近づける
            float move_distance = (distance - lookPlayerdistance) * (Time.deltaTime * cameraSpeed);
            D += vec.normalized * move_distance;
            //Debug.Log("UpdateLook");
            //Debug.Log("D" + D);
        }
        if (Input.GetKey(KeyCode.RightShift))
        {
            Reset();
        }
    }

    //カメラ座標の制御
    public void UpdateCameraPosition()
    {
        //XZ平面におけるカメラと視点の距離を湯徳
        Vector3 xz_vec = Player.transform.position - this.transform.position;
        xz_vec.y = 0;
        float distance = xz_vec.magnitude;

        //カメラの移動距離を求める
        float move_distance = 0;
        if (distance > cameraDistance + cPDistance)
        {
            //カメラの回転する範囲を超えたら追いかける
            move_distance = distance - (cameraDistance + cPDistance);
            move_distance *= Time.deltaTime * cameraSpeed;
        }

        //新しいカメラの位置を求める
        Vector3 camera_pos = this.transform.position + (xz_vec.normalized * move_distance);

        //高さは現在紫檀を常に一定で維持する
        camera_pos.y = D.y + nowCameraHeight;
        this.transform.position = camera_pos;
    }

    //カメラの回転
    public void Roll(float x, float y)
    {
        //移動前の距離を保持
        float prev_distans = Vector3.Distance(Player.transform.position, this.transform.position);
        Vector3 pos = this.transform.position;

        //横移動
        pos += this.transform.right * x / 5.0f;

        //縦移動
        nowCameraHeight = Mathf.Clamp(nowCameraHeight + y, cameraHeightMin, cameraHeightMax);
        pos.y = D.y + nowCameraHeight;

        //移動後の距離を取得
        float after_distance = Vector3.Distance(Player.transform.position, pos);

        //視点を対象に近づける（余裕をなくす）
        D = Vector3.Lerp(D, Player.transform.position, 0.1f);

        //カメラの更新
        this.transform.position = pos;
        this.transform.LookAt(D);

        //平行移動により若干距離が変わるので補正する
        this.transform.position += transform.forward * (after_distance - prev_distans);
        //D.z = 0.0f;
        //Player.transform.rotation = Quaternion.LookRotation(D);
    }

    //カメラリセット
    public void Reset(float rate = 1.0f)
    {
        //視点対象に近づける
        D = Vector3.Lerp(D, Player.transform.position, rate);

        //高さをデフォルトに変える
        nowCameraHeight = Mathf.Lerp(nowCameraHeight, cameraHeight, rate);

        //カメラの基本位置に近づける
        Vector3 pos_goal = Player.transform.position;
        pos_goal = Player.transform.position * cameraDistance;
        pos_goal.y = Player.transform.position.y + nowCameraHeight;
        this.transform.position = Vector3.Lerp(this.transform.position, pos_goal, rate);

        //視点を更新する
        this.transform.LookAt(D);

    }
}


/*
//ｘ方向に一定量移動していれば横回転
if (Mathf.Abs(mousex) > 0.001f)
{
    //回転軸はワールド座標のＹ軸
    transform.RotateAround(Player.transform.position, Vector3.up, mousex);
}
if (Mathf.Abs(mousey) > 0.001f)
{
    //回転軸はワールド座標のx軸
    if (Player.transform.rotation.x <= 80.0f || Player.transform.rotation.x >= -80.0f)
    {
        transform.RotateAround(Player.transform.position, Vector3.up, mousey);
    }
}
*/


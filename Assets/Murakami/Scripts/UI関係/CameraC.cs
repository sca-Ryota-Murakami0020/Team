using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraC : MonoBehaviour
{
    //視点となるオブジェクト
    [SerializeField] private GameObject Player = null;
    //旋回した時のx座標
    private float xpos;
    //旋回した時のｚ座標
    private float zpos;
    //実際にカメラを向ける座標//カメラとプレイヤーとの距離
    private Vector3 D = Vector3.zero;//Lookpos
    //Playerを追従しないでカメラの向きだけ変える範囲
    private float lookPlayerdistance = 0.1f;
    //PLayerを追従する速度
    private float cameraSpeed = 4.0f;//=followSmooth 
    //視点からカメラまでの距離
    private float cameraDistance = 15.5f;
    //デフォルトの高さ
    private float cameraHeight = 10.5f;
    //現在のカメラの高さ
    private float nowCameraHeight = 3.0f;//currentCameraHeight = 1.0f;
    //視点からカメラの距離の遊び
    private float cPDistance = 0.5f;//cameraPlayDiatance = 0.3f; 
    //離れる時の速度
    private float leaveCamera = 10.0f;//leaveSmooth
    //カメラの最低高度
    private float cameraHeightMin = -5.0f;
    //カメラの最高高度
    private float cameraHeightMax = 8.5f;
    //横方向のマウスの移動量
    private float mousex;
    //縦方向のマウスの移動量
    private float mousey;

    private Vector3 rayHitPosition;

    //[SerializeField] private GameObject bullet;

    [SerializeField] private GameObject bulletSponePosition;

    private Vector3 dir;

    public Vector3 RayHitPosition
    {
        get { return this.rayHitPosition;}
        set { this.rayHitPosition = value;}
    }

    public Vector3 Dir
    {
        get { return this.dir;}
        set { this.dir = value;}
    }

    void Start()
    {
        Player = GameObject.Find("Player");
    }

    //視点とカメラ座標を随時更新
    void Update()
    {    
        if (Player == null) return;
        //マウスの移動量を取得
        mousex = Input.GetAxis("Mouse X");
        mousey = Input.GetAxis("Mouse Y");

        if (Mathf.Abs(mousex) > 0.008f || Mathf.Abs(mousey) > 0.005f)
        {
            Roll(mousex, mousey);
        }
        if (Input.GetMouseButtonDown(1))
       {
            GetShotVector();
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
        pos += this.transform.right * x;

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

    
    public void GetShotVector()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit isHit;// = Physics.Raycast((Vector3) ray.origin,(Vector3) ray.direction,isHit);
        if(Physics.Raycast(ray, out isHit, Mathf.Infinity))
        {
            // rayの当たった位置 - ボール位置間の計算を行い、ベクトルを取得（y座標のみボールの座標を採用）
            rayHitPosition = new Vector3(isHit.point.x, isHit.point.y, isHit.point.z); 
            Debug.Log("rayHitPos" + rayHitPosition);
            dir = (isHit.point - bulletSponePosition.transform.position).normalized;
            //Instantiate(bullet, new Vector3(dir.x,dir.y,dir.z), Quaternion.identity);
            Debug.Log("はっしゃ");
        }
        Debug.Log(dir);
        Debug.DrawRay(ray.origin,ray.direction * 10, Color.green, 5);

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


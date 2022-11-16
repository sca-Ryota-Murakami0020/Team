using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireGun : MonoBehaviour
{
    //コンポーネント
    //private Animator animator;
    private LineRenderer lineRenderer;
    private SpringJoint springJoint;
    private float maxDistance = 100f;//最大距離
    private Color color = new Color32(248,168,133,1);//デフォの色
    private GameObject target;//ラインが当たった時の障害物

    private Player player;//プレイヤー

    [SerializeField]
    private Transform parentTran;//lineの初めの場所

    private CameraC camera;//カメラ定義

    [SerializeField] private GameObject bullet;//弾
    private bool bulletShootingFalg = false;//弾が発射する時のフラグ

    private bool isWire;//Wireが存在するフラグ

    public bool BulletShootingFalg
    {
        get { return this.bulletShootingFalg; }
        set { this.bulletShootingFalg = value; }
    }

    public GameObject Target
    {
        get { return this.target; }
        set { this.target = value; }
    }
      
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        camera = FindObjectOfType<CameraC>();
        //animator = this.GetComponent<Animator>();
        lineRenderer = GetComponent<LineRenderer>();
      
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))//左クリ押したら
        {
            lineRenderer.SetPosition(0, transform.position);//レイヤーの始点をWireGunオブジェクトする
            lineRenderer.transform.SetParent(parentTran);//プレイヤーが動いたときのポジションにLineRendererの始点をつける
            if (camera.RayTrueFlag == true)
            {
                // lineの終点
                lineRenderer.SetPosition(1, camera.IsHit.point);//カメラのレイが当たった時
                target = camera.IsHit.collider.gameObject;//当たったオブジェクト
                Vector3 dir = camera.Dir;//レイの単位ベクトル
                isWire = true;//Wireが発射された
            }
            else
            {
                //初期状態に戻す
                lineRenderer.SetPosition(1, camera.CameraRay.origin + (camera.Dir * maxDistance));//lineRendererのポジションを更新
                lineRenderer.startColor = color;
                lineRenderer.endColor = color;
            }
        }

        //Wireが存在するとき
        if (isWire)
        {
            //lineのstart場所=playerの位置に固定し続けることでWireが伸びているように見せる
            lineRenderer.SetPosition(0, player.transform.position);
        }
    }

   void ConnectWireCoroutine(GameObject target)
   {
        Debug.Log(target);
        springJoint.connectedBody = target.GetComponent<Rigidbody>();
        springJoint.autoConfigureConnectedAnchor = false;
        springJoint.anchor = new Vector3(0f,1f,0f);
        springJoint.connectedAnchor = new Vector3(0f,-0.5f,0f);
        springJoint.spring =20f;
        springJoint.damper = 0.5f;
   }

    void DestroyWireCoroutine()
    {
        if (springJoint != null)
        {
            Destroy(springJoint);
        }
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireGun : MonoBehaviour
{
    //コンポーネント
    //private Animator animator;
    private SpringJoint springJoint;
    private LineRenderer lineRenderer;
    private Player player;
    private Transform cameraTransForm;

    private CameraC camera;

    [SerializeField] private GameObject bullet;
    private bool bulletShootingFalg = false;
    private Vector3 normalDirection;


    private float maxDistance;//糸を伸ばせる最大距離
    [SerializeField] private LayerMask wireLayers; //糸をくっつかせるレイヤー
    [SerializeField] private Vector3 casterCenter = new Vector3(0.0f, 0.5f, 0.0f);//オブジェクトのローカル座標で表した糸の射出位置

    [SerializeField] private float spring = 50.0f;// 糸の物理的挙動を担当するSpringJointのspring
    [SerializeField] private float damper = 20.0f;// 糸の物理的挙動を担当するSpringJointのdamper
    [SerializeField] private float equilibrumLength;//糸を縮めた時の自然長

    private bool casting;//糸が射出中かどうかの表すフラグ
    private bool needsUpdateSpring; // FixedUpdate中でSpringJointの状態更新が必要かどうかを表すフラグ
    private float stringLength; // 現在の糸の長さ...この値をFixedUpdate中でSpringJointのmaxDistanceにセットする
    private readonly Vector3[] stringAnchor = new Vector3[2]; // SpringJointのキャラクター側と接着点側の末端
    private Vector3 worldCasterCenter; // casterCenterをワールド座標に変換したもの

    public bool BulletShootingFalg
    {
        get { return this.bulletShootingFalg; }
        set { this.bulletShootingFalg = value; }
    }

    public Vector3 NormalDirection
    {
        get { return this.normalDirection; }
        set { this.normalDirection = value; }
    }

      
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        camera = FindObjectOfType<CameraC>();
        //animator = this.GetComponent<Animator>();
        cameraTransForm = Camera.main.transform;
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        // まず画面中心から真っ正面に伸びるRayを求め、さらにworldCasterCenterから
        // そのRayの衝突点に向かうRayを求める...これを糸の射出方向とする
        //this.worldCasterCenter = this.transform.TransformPoint(this.casterCenter);
        //Ray pointerRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Ray aimingRay = new Ray(this.worldCasterCenter, Physics.Raycast(pointerRay, out var focus, float.PositiveInfinity, this.wireLayers) ? focus.point - this.worldCasterCenter : pointerRay.direction);
        //ローカル座標をワールド座標に変換
        //次に新しくRayを作る(スクリーンの点を通してカメラからレイを通す)
        //それを応用しRayを新しく作る
        //Ray(Rayの発生地点,Rayの進む方向(ワールド座標のRay判定,hitしたオブジェクトをフォーカス,Rayの長さは正の無限大数,衝突するrayerは指定したものか
        //そのポイントからワールド座標のカメラRayの角度を引く))


        if (Input.GetMouseButtonDown(0))
        {
            bulletShootingFalg = true;
            StartWireGun();
        }
    }

    private void StartWireGun()
    {
        normalDirection = (camera.RayHitPosition - transform.position).normalized;
        GameObject Bullet_obj = (GameObject)Instantiate(bullet, transform.position, Quaternion.Euler(0.0f, 0.0f, 0.0f));
        Bullet bullet_cs = Bullet_obj.GetComponent<Bullet>();
        bulletShootingFalg = false;

    }

    private void StopWireGun()
    {
      
    }
}

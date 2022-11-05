using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireGun : MonoBehaviour
{
    private float bulletDisTime = 0;
    //コンポーネント
    //private Animator animator;
    private LineRenderer lineRenderer;
    private SpringJoint springJoint;
    private float maxDistance = 100f;
    private Color color = new Color32(248,168,133,1);
    private GameObject target;

    private Player player;
    private Transform cameraTransForm;

    [SerializeField]
    private Transform parentTran;



    private CameraC camera;


    [SerializeField] private GameObject bullet;
    private bool bulletShootingFalg = false;



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
        cameraTransForm = Camera.main.transform;
        lineRenderer = GetComponent<LineRenderer>();
      
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.transform.SetParent(parentTran);
            if (camera.RayTrueFlag == true)
            {
                lineRenderer.SetPosition(1, camera.IsHit.point);
                target = camera.IsHit.collider.gameObject;

                Debug.Log(target);
                //GameObject Bullet_obj = (GameObject)Instantiate(bullet, transform.position, Quaternion.identity);
                //Bullet bullet_cs = Bullet_obj.GetComponent<Bullet>();
                Debug.Log(camera.Dir);
                Vector3 dir = camera.Dir;

                //if (bulletShootingFalg == true)
                //{
                ConnectWireCoroutine(target);
                //Bullet_obj.GetComponent<Rigidbody>().AddForce(dir * 1000.0f);
                bulletDisTime += Time.deltaTime;
                //if(bulletDisTime == 5.0f)
                //{
                //Destroy(Bullet_obj);
                bulletDisTime = 0.0f;
                bulletShootingFalg = false;
                camera.RayTrueFlag = false;
                DestroyWireCoroutine();
                //}
                // }
            }
            else
            {
                //初期状態に戻す
                lineRenderer.SetPosition(1, camera.CameraRay.origin + (camera.Dir * maxDistance));
                lineRenderer.startColor = color;
                lineRenderer.endColor = color;
            }
        }
    }

    private void StartWireGun()
    {
       
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

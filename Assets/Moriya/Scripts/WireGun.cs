using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireGun : MonoBehaviour
{
    //コンポーネント
    //private Animator animator;
    private LineRenderer lineRenderer;

    private Player player;
    private Transform cameraTransForm;

    private CameraC camera;


    [SerializeField] private GameObject bullet;
    private bool bulletShootingFalg = false;


    public bool BulletShootingFalg
    {
        get { return this.bulletShootingFalg; }
        set { this.bulletShootingFalg = value; }
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
            bulletShootingFalg = true;
            StartWireGun();
        }
    }

    private void StartWireGun()
    {
       lineRenderer.SetPosition(0,camera.CameraRay.origin);
       //if(camera.)
       //{

            GameObject Bullet_obj = (GameObject)Instantiate(bullet, transform.position, Quaternion.identity);
            //Debug.Log(transform.position);
            Bullet bullet_cs = Bullet_obj.GetComponent<Bullet>();
            Debug.Log(camera.Dir);
            Vector3 dir = camera.Dir;
            Bullet_obj.GetComponent<Rigidbody>().AddForce(dir * 1000.0f);
       //}
    }

    private void StopWireGun()
    {
      
    }
}

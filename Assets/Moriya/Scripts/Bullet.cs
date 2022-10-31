using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody rb;
    private WireGun wireGun;
    private CameraC camera;
    private float timeC;


   
    private SpringJoint springJoint;
    private LineRenderer lineRenderer;

    private bool shootFlag = false;
  
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        wireGun = FindObjectOfType<WireGun>();
        camera = FindObjectOfType<CameraC>();

        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            Debug.Log("�n�ʂƓ�������");
            PlayerSpring();
        }

        if (other.gameObject.CompareTag("Wall"))
        {
            Debug.Log("�ǂɓ�������");
            PlayerSpring();
        }
        /*if (other.gameObject.CompareTag("Wall"))
        {
            jumpCount = 0;
        }*/
    }

    private void PlayerSpring()
    {

    }
}

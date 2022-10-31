using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody rb;
    private WireGun wireGun;
    private float timeC;
    private bool shootFlag=false;

    private Vector3 dir;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        wireGun = FindObjectOfType<WireGun>();
    }

    // Update is called once per frame
    void Update()
    {
        timeC += Time.deltaTime;

        if(shootFlag == false)
        {
            dir = wireGun.NormalDirection;
            rb.velocity = (dir * 15.0f);
            shootFlag =true;
        }

       

        if(timeC >= 1.0f&& shootFlag == true)
        {
            Debug.Log("”­ŽË");
            Destroy(this.gameObject);
            shootFlag = false;
            timeC = 0.0f;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            Debug.Log("’n–Ê‚Æ“–‚½‚Á‚½");
        }

        if (other.gameObject.CompareTag("Wall"))
        {
            Debug.Log("•Ç‚É“–‚½‚Á‚½");
        }
        /*if (other.gameObject.CompareTag("Wall"))
        {
            jumpCount = 0;
        }*/
    }
}

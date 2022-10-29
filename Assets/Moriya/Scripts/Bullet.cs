using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody rb;
    private WireGun wireGun;

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


            dir = wireGun.NormalDirection;
        rb.velocity = (dir * 15.0f);
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

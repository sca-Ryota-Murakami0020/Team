using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class switch1 : MonoBehaviour
{
    public GameObject obj;
    public player player;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionEnter(Collision collision)
    {
        _ = collision.gameObject.name == "player";
        {
            Instantiate(obj, new Vector3(-9, 0, 0), Quaternion.identity);

        }
    }
}


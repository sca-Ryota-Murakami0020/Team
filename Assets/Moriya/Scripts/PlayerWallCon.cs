using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallCon : MonoBehaviour
{
    //一個上の親のオブジェクト
    private GameObject parent;

    Texture2D tex;
    MeshRenderer meshRenderer;

    private bool wallHitFlag;

    //private Color color;
    /*public Color Color
    {
        get { return this.color; }
        set { this.color = value; }
    }*/

    // Start is called before the first frame update
    void Start()
    {
        parent = transform.parent.gameObject;
        meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("WallJumpPoint"))
        {
            Debug.Log("a");
        }

        if (other.gameObject.CompareTag("ClingPoint"))
        {
            Debug.Log("i");
        }

        if (other.gameObject.CompareTag("ClingPoint")&& other.gameObject.CompareTag("WallJumpPoint"))
        {
            Debug.Log("u");
        }

        /*if (other.gameObject.CompareTag("Ground"))
        {
            meshRenderer.material.color = color;
            Debug.Log(color)
        }*/
    }

}

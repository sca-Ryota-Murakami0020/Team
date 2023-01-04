using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallCon : MonoBehaviour
{
    //一個上の親のオブジェクト
    private GameObject parent;

    Texture2D tex;
    MeshRenderer meshRenderer;

    private bool wallJumpFlag = false;
    private bool clingFlag = false;
    private bool doubleHitFlag = false;

    public bool WallJumpFlag
    {
        get { return this.wallJumpFlag; }
        set { this.wallJumpFlag = value; }
    }

    public bool ClingFlag
    {
        get { return this.clingFlag; }
        set { this.clingFlag = value; }
    }

    public bool DoubleHitFlag
    {
        get { return this.doubleHitFlag; }
        set { this.doubleHitFlag = value; }
    }

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
            //Debug.Log("a");
            wallJumpFlag = true;
        }

        if (other.gameObject.CompareTag("ClingPoint"))
        {
            //Debug.Log("i");
            clingFlag = true;
        }

        if (other.gameObject.CompareTag("ClingPoint")&& other.gameObject.CompareTag("WallJumpPoint"))
        {
            doubleHitFlag = true;
        }

    }

}

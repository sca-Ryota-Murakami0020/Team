using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallCon : MonoBehaviour
{
    //���̐e�̃I�u�W�F�N�g
    private GameObject parent;

    private bool wallJumpHitFlag = false;
    
    public bool WallJumpHitFlag
    {
        get { return this.wallJumpHitFlag; }
        set { this.wallJumpHitFlag = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        parent = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.CompareTag("WallJumpPoint"))
        {
            //Debug.Log("����t��");
            wallJumpHitFlag = true;
        }
    }
}

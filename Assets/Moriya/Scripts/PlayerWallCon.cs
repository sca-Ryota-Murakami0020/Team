using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallCon : MonoBehaviour
{
    //一個上の親のオブジェクト
    private GameObject parent;

    //ジャンプ地点に触れた時のフラグ
    private bool wallJumpHitFlag = false;
    
    //ゲッターセッター
    public bool WallJumpHitFlag
    {
        get { return this.wallJumpHitFlag; }
        set { this.wallJumpHitFlag = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        //親のゲームオブジェクト取得(念のため)
        parent = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        //壁はりつきできる地点に触れたら
        if (other.gameObject.CompareTag("WallJumpPoint"))
        {
            //フラグを立てる
            wallJumpHitFlag = true;
        }
    }
}

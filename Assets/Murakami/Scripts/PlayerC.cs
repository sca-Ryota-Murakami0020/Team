using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerC : MonoBehaviour
{
    //RigidBody
    private Rigidbody rb;
    //プレイヤーの速さ
    private float speed = 1.5f;
    //ジャンプ力
    private float jumpPower = 10.5f;
    //接地確認用フラグ
    private bool jumpFlag;
    //体力
    private int hp;
    //生存確認フラグ
    private bool aliveFlag;
    private bool fuckFlag;

    //プロパティ
    public int Hp
    {
        get { return this.hp;}
        set { this.hp = value;}
    }

    public bool AliveFlag
    {
        get { return this.aliveFlag;}
        set { this.aliveFlag = value;}
    }
    public bool FuckFlag
    {
        get { return this.fuckFlag;}
        set { this.fuckFlag = value;}
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        jumpFlag = false;
        this.hp = 1;
        this.aliveFlag = true;
        this.fuckFlag = false;
    }

    // Update is called once per frame
    void Update()
    {
        //移動
        float H = Input.GetAxis("Horizontal") * speed;
        float V = Input.GetAxis("Vertical") * speed;
        rb.AddForce(H,0,V);
        //ジャンプ
        if(Input.GetKey(KeyCode.Space) && jumpFlag == false)
        {
            rb.AddForce(0,jumpPower * jumpPower,0);
            jumpFlag = true;          
        } 
        //次のジャンプまでの間隔の計算
        float i =+ Time.deltaTime;
        if(i > 3.0f || jumpFlag == true)
        {
            jumpFlag = false;
            i = 0.0f;
        }
        //体力０で処理
        if(this.hp  <= 0)
        {
            GameOver();
        }
    }

    //ゲームオーバー処理
    public void GameOver()
    {
        aliveFlag = false;
    }
}
/*
    playerText.text = "ライフ：" + pl.hp.ToString();
    playerText = GetComponentInChildren<Text>();
    //体力表示
    public Text playerText;
 */



using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerC : MonoBehaviour
{
    //RigidBody
    private Rigidbody rb;
    //プレイヤーの速さ
    private float speed = 1.0f;
    //ジャンプ力
    private float jumpPower = 10.5f;
    //接地確認用フラグ
    private bool jumpFlag;
    //体力
    private int hp;
    //生存確認フラグ
    private bool aliveFlag;
    private bool fuckFlag;
    private float mouseX;
    private float mouseY;


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
        //camera = GameObject.Find("Main Camera");
        jumpFlag = false;
        this.hp = 3;
        this.aliveFlag = true;
        this.fuckFlag = false;
    }

    // Update is called once per frame
    void Update()
    {
        //移動
        float H = Input.GetAxis("Horizontal") * speed;
        float V = Input.GetAxis("Vertical") * speed;
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");

        if (Mathf.Abs(mouseX) > 0.008f || Mathf.Abs(mouseY) > 0.005f)
        {
            Roll(mouseX, mouseY);
        }

        rb.AddForce(H,0,V);

        //ジャンプ
        if (Input.GetKeyDown(KeyCode.Space) && jumpFlag == false)
        {
            rb.AddForce(0,Mathf.Pow(jumpPower,2),0);
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

    public void Roll(float x, float y)
    {
        transform.RotateAround(this.transform.position, Vector3.up, x);
        transform.RotateAround(this.transform.position, Vector3.up, y);
    }

    //ゲームオーバー処理
    public void GameOver()
    {
        aliveFlag = false;
        Debug.Log("GameOverの呼び出し");
        SceneManager.LoadScene("村上用GameOver");
    }
}
/*
    playerText.text = "ライフ：" + pl.hp.ToString();
    playerText = GetComponentInChildren<Text>();
    //体力表示
    public Text playerText;
 */



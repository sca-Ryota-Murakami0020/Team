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
    //�v���C���[�̑���
    private float speed = 1.0f;
    //�W�����v��
    private float jumpPower = 10.5f;
    //�ڒn�m�F�p�t���O
    private bool jumpFlag;
    //�̗�
    private int hp;
    //�����m�F�t���O
    private bool aliveFlag;
    private bool fuckFlag;
    private float mouseX;
    private float mouseY;


    //�v���p�e�B
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
        //�ړ�
        float H = Input.GetAxis("Horizontal") * speed;
        float V = Input.GetAxis("Vertical") * speed;
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");

        if (Mathf.Abs(mouseX) > 0.008f || Mathf.Abs(mouseY) > 0.005f)
        {
            Roll(mouseX, mouseY);
        }

        rb.AddForce(H,0,V);

        //�W�����v
        if (Input.GetKeyDown(KeyCode.Space) && jumpFlag == false)
        {
            rb.AddForce(0,Mathf.Pow(jumpPower,2),0);
            jumpFlag = true;          
        } 
        //���̃W�����v�܂ł̊Ԋu�̌v�Z
        float i =+ Time.deltaTime;
        if(i > 3.0f || jumpFlag == true)
        {
            jumpFlag = false;
            i = 0.0f;
        }
        //�̗͂O�ŏ���
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

    //�Q�[���I�[�o�[����
    public void GameOver()
    {
        aliveFlag = false;
        Debug.Log("GameOver�̌Ăяo��");
        SceneManager.LoadScene("����pGameOver");
    }
}
/*
    playerText.text = "���C�t�F" + pl.hp.ToString();
    playerText = GetComponentInChildren<Text>();
    //�̗͕\��
    public Text playerText;
 */



using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerC : MonoBehaviour
{
    //RigidBody
    private Rigidbody rb;
    //�v���C���[�̑���
    private float speed = 1.5f;
    //�W�����v��
    private float jumpPower = 10.5f;
    //�ڒn�m�F�p�t���O
    private bool jumpFlag;
    //�̗�
    private int hp;
    //�����m�F�t���O
    private bool aliveFlag;
    private bool fuckFlag;

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
        jumpFlag = false;
        this.hp = 1;
        this.aliveFlag = true;
        this.fuckFlag = false;
    }

    // Update is called once per frame
    void Update()
    {
        //�ړ�
        float H = Input.GetAxis("Horizontal") * speed;
        float V = Input.GetAxis("Vertical") * speed;
        rb.AddForce(H,0,V);
        //�W�����v
        if(Input.GetKey(KeyCode.Space) && jumpFlag == false)
        {
            rb.AddForce(0,jumpPower * jumpPower,0);
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

    //�Q�[���I�[�o�[����
    public void GameOver()
    {
        aliveFlag = false;
    }
}
/*
    playerText.text = "���C�t�F" + pl.hp.ToString();
    playerText = GetComponentInChildren<Text>();
    //�̗͕\��
    public Text playerText;
 */



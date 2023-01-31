using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReSponeEnemy : MonoBehaviour
{
    //�Ăяo��Enemy
    [SerializeField] private GameObject enemy;
    //1�̂̂ݏo�͂��邽�߂̃t���O
    private bool aliveFlag;
    //Enemy���Ăяo���I�u�W�F�N�g
    [SerializeField] private GameObject sponePosObj;
    //�Ăяo���ʒu
    private Vector3 sponePosition;

    /*
    public bool AliveFlag
    {
        get { return this.aliveFlag;}
        set { this.aliveFlag = value;}
    }*/


    // Start is called before the first frame update
    void Start()
    {
        aliveFlag = false;
        sponePosition = sponePosObj.transform.position;
        this.transform.position = sponePosition;
        Debug.Log("�ŏ��̌Ăяo������");
    }

    // Update is called once per frame
    void Update()
    {      
        //�����ݒu
        if(aliveFlag == false)
        {
            //�ŏ��A�X�e�[�W�ɂ͓G���z�u���Ă��Ȃ��̂ł����ň�x�����Ăяo��
            Instantiate(enemy, new Vector3(sponePosition.x, sponePosition.y,sponePosition.z), Quaternion.identity);
            aliveFlag = true;
            Debug.Log("�Ăяo���ʒu" + sponePosition);
        }
        
    }

    public void SponeEnemy()
    {
        StartCoroutine("StartSponeEnemy");
    }

    private IEnumerator StartSponeEnemy()
    {
        Debug.Log("������");
        //10�b�ԑҋ@����
        yield return new WaitForSeconds(10);
        //10�b���EnemyC�̕���SetSctive��false�ɂ���GameObject��SetActive��true�ɂ��čĂъ����ł���悤�ɂ��Ă�����
        Instantiate(enemy, this.transform.position, Quaternion.identity);
        Debug.Log("��������");
        yield break;
    }
}
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

    void Start()
    {
        aliveFlag = false;
        sponePosition = sponePosObj.transform.position;
        this.transform.position = sponePosition;
    }

    void Update()
    {      
        //�����ݒu
        if(aliveFlag == false)
        {
            //�ŏ��A�X�e�[�W�ɂ͓G���z�u���Ă��Ȃ��̂ł����ň�x�����Ăяo��
            Instantiate(enemy, new Vector3(sponePosition.x, sponePosition.y,sponePosition.z), Quaternion.identity);
            aliveFlag = true;
        }
        
    }

    public void SponeEnemy()
    {
        //������������R���[�`���̋N��
        StartCoroutine("StartSponeEnemy");
    }

    private IEnumerator StartSponeEnemy()
    {
        //10�b�ԑҋ@����
        yield return new WaitForSeconds(10);
        //10�b���EnemyC�̕���SetSctive��false�ɂ���GameObject��SetActive��true�ɂ��čĂъ����ł���悤�ɂ��Ă�����
        Instantiate(enemy, this.transform.position, Quaternion.identity);
        yield break;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySponerC : MonoBehaviour
{
    //�X�^�[�g�n�_
    [SerializeField] private GameObject setStartPoint;
    //�I���_
    [SerializeField] private GameObject setEndPoint;
    //�Ăяo���G�̃I�u�W�F�N�g
    [SerializeField] private GameObject isSponeEnemy;
    //�o���Ԋu
    private float coolSponeEnemyTime = 3.0f;
    //Enemy���Ăяo��������true�ɂȂ���������܂�true�ɂȂ�
    //private bool isEnemyAlive;

    public GameObject StartPoint
    {
        get { return this.setStartPoint;}
        set { this.setStartPoint = value;}
    }

    public GameObject EndPoint
    {
        get { return this.setEndPoint;}
        set { this.setEndPoint = value;}
    }

    void Start()
    {
        this.transform.position = this.setStartPoint.transform.position;
    }

    public void SponeEnemy()
    {
        Debug.Log("�R���[�`���J�n");
        StartCoroutine("StartSponeEnemy");
    }

    private IEnumerable StartSponeEnemy()
    {
        coolSponeEnemyTime -= Time.deltaTime;
        if(coolSponeEnemyTime <= 0.0f)
        {
            coolSponeEnemyTime = 3.0f;
            Instantiate(isSponeEnemy, this.gameObject.transform.position, Quaternion.identity);
            yield break;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SponeEnemy : MonoBehaviour
{
    //�X�^�[�g�ʒu
    private Vector3 startPos;
    //�Ăяo��Enemy
    [SerializeField] private GameObject enemy;
    //�Ăяo���Ƃ���Enemy�̊p�x
    [SerializeField] 


    // Start is called before the first frame update
    void Start()
    {
        startPos = this.transform.position;
        Instantiate(enemy, startPos, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReSponeEnemy()
    {
        StartCoroutine("StartSponeEnemy");
    }

    private IEnumerator StartSponeEnemy()
    {
        Debug.Log("������");
        //10�b�ԑҋ@����
        yield return new WaitForSeconds(10);
        //10�b���EnemyC�̕���SetSctive��false�ɂ���GameObject��SetActive��true�ɂ��čĂъ����ł���悤�ɂ��Ă�����
        Instantiate(enemy,startPos,Quaternion.identity);
        Debug.Log("��������");
        yield break;
    }
}

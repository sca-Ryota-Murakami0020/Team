using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetEnemyPosition : MonoBehaviour
{

    [SerializeField] private GameObject enemy;
     
    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartCountDistance()
    {
        //EnemyC����󂯎悽���Ɏw������R���[�`�����Ăяo��
        Debug.Log("�����n������");
        StartCoroutine("ActiveEnemy");
    }

    private IEnumerator ActiveEnemy()
    {
        Debug.Log("������");
        //10�b�ԑҋ@����
        yield return new WaitForSeconds(10);
        //10�b���EnemyC�̕���SetSctive��false�ɂ���GameObject��SetActive��true�ɂ��čĂъ����ł���悤�ɂ��Ă�����
        enemy.SetActive(true);
        Debug.Log("��������");
        yield break;
    }
}

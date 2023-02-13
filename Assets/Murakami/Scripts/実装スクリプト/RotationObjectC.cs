using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationObjectC : MonoBehaviour
{
    //���Ԍv���Ɏg���ϐ�
    private float countTime = 0.0f;
    //�I�u�W�F�N�g�̃|�W�V����
    private Vector3 pos;

    void Start()
    {
        pos = this.transform.position;
    }

    void Update()
    {
        countTime += 0.01f;
        //�I�u�W�F�N�g���c�ɉ����ړ�������
        this.transform.position = new Vector3(pos.x, pos.y, pos.z + Mathf.Sin(countTime) * 0.07f);
        this.transform.Rotate(0, 0, 0.05f);
    }
}

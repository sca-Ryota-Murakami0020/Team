using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackPasueButton : MonoBehaviour
{
    //�v���������
    private float countTime = 0.0f;
    //�I�u�W�F�N�g�̃|�W�V����
    private Vector3 pos;
    //PasueDisplayC
    private PasueDisplayC pDC;

    void Start()
    {
        pos = this.transform.position;
        pDC = FindObjectOfType<PasueDisplayC>();
    }

    void Update()
    {
        //������������悤�ɓ�����
        countTime += 0.01f;
        this.transform.position = new Vector3(pos.x + Mathf.Sin(countTime) * 2, pos.y, pos.z);
    }

    //�P�y�[�W�ڂ���Q�y�[�W�ڂɈړ�����
    public void BackFirstPasueUI()
    {
        pDC.OpenFirstMenuPage();
    }
}

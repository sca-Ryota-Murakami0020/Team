using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBottonC : MonoBehaviour
{
    private PasueDisplayC pDC;

    void Start()
    {
        pDC = FindObjectOfType<PasueDisplayC>();
    }

    //�|�[�Y��ʂ��瑀�������ʂɐ؂�ւ���
    public void OnManual()
    {
        //�����ʂ̊J��
        pDC.DisplayManual();
    }
}

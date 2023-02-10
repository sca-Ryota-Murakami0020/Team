using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleButtonC : MonoBehaviour
{
    //TotalGameManager
    private totalGameManager tGM;

    private void Awake()
    {
        tGM = FindObjectOfType<totalGameManager>();
    }

    //�^�C�g���ɖ߂�
    public void GotoTitle()
    {
        //totalGameManager���Ȃ��^�C�g����ʂ��Ăяo��
        SceneManager.LoadScene("AnyLoadTitle");
        //���݂̃v���C���Ԃ�����������
        tGM.TotalTime = 0.0f;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryButtonC : MonoBehaviour
{
    //�Q�[�������g���C����
    public void RetryGame()
    {
        //�r���X�e�[�W���Ăяo�����[�h�V�[�����Ăяo��
        SceneManager.LoadScene("LoadBill");
    }
}

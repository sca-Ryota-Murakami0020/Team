using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverS : MonoBehaviour
{
    //�Q�[���I�[�o�[�V�[���̃X�v���N�g
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //�{�^�����ƂɃ��[�h����V�[�����g��������
    public void Retry()
    {
        SceneManager.LoadScene("LoadBill");
    }

    public void Titleback()
    {
        SceneManager.LoadScene("AnyLoadTitle");
    }
}

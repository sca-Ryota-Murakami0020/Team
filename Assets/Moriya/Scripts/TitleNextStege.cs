using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleNextStege : MonoBehaviour
{
    //�^�C�g����ʂ̃X�v���N�g

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextStege()
    {
      //��������̃V�[���ɂ���
      SceneManager.LoadScene("�������"); 
    }
}

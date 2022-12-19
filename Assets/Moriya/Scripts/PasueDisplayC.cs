using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PasueDisplayC : MonoBehaviour
{
    //�|�[�Y�֌W�̃X�v���N�g
    private Player playerC;

    //�|�[�Y���J�������̃t���O
    private bool menuFlag = false;

    //��������J�����Ƃ��̃t���O
    private bool operationExpFlag = false;

    //��񂾂�����Ƃ��̃t���O
    private bool onlyFlag = false;

    [SerializeField]
    //�|�[�Y�������ɕ\������UI�̃v���n�u
    private GameObject pauseUIPrefab;
    //�|�[�YUI�̃C���X�^���X
    private GameObject pauseUIInstance;
    //�������UI�̃C���X�^���X
    private GameObject playOperateUIInstance;
    [SerializeField]
    //�������UI�̃v���n�u
    private GameObject playOperatePrafab;

    //�Q�b�^�[�Z�b�^�[
    public bool MenuFlag
    {
        get {return this.menuFlag; }
        set {this.menuFlag = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        playerC = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
      
        #region//���j���[��ʂ��J������
        if (Input.GetKeyDown("q"))
        {
            if(pauseUIInstance == null)
            {
                pauseUIInstance = GameObject.Instantiate(pauseUIPrefab) as GameObject;
                Time.timeScale=0f;
                menuFlag = true;
            }
            else
            {
                menuFlag = false;
                Destroy(playOperateUIInstance);
                Destroy(pauseUIInstance);
                Time.timeScale = 1f;
            }
        }
        //���j���[���J���ꂽ��
        if(menuFlag == true)
        {
            PauseMenu();
        }
        #endregion
    }

    #region//���j���[��ʂ��J���Ă��鎞�̏���
    private void PauseMenu()
    {
        if (Input.GetKey(KeyCode.Escape)) 
        { 
             /*#if UNITY_EDITOR
             UnityEditor.EditorApplication.isPlaying = false;
            //�G�f�B�^��̓���
            #else
            Application.Quit();
            //�G�f�B�^�ȊO�̑���
            #endif*/
              // SceneManager.LoadScene("FirstScene");
            Time.timeScale = 1f;
            menuFlag = false;
            ResetCommand();
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            onlyFlag = true;
            StartCoroutine("PlayerXplanation");
            //��������J��
           
        }
        

    }
    #endregion

    private IEnumerator PlayerXplanation()
    {
        while (true)
        {
            Debug.Log(onlyFlag);
            if (onlyFlag == true)
            {
                Destroy(pauseUIInstance);
                playOperateUIInstance = GameObject.Instantiate(playOperatePrafab) as GameObject;
                operationExpFlag = true;
                onlyFlag = false;
            }
            Debug.Log(playOperateUIInstance);
            if (operationExpFlag == true && Input.GetKeyUp (KeyCode.Tab))
            {
                operationExpFlag = false;
                Destroy(playOperateUIInstance);
                pauseUIInstance = GameObject.Instantiate(pauseUIPrefab) as GameObject;
                Debug.Log(pauseUIInstance);
                yield break;
            }
           yield return null; 
        }
    }

    private void ResetCommand()
    {
        playerC.PlayerMaxHp = 3;
        playerC.PlayerHp = playerC.PlayerMaxHp;
        playerC.PlayerSpeed = 10.0f;
        playerC.JumpCount = 0;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PasueDisplayC : MonoBehaviour
{
    //�|�[�Y�֌W�̃X�v���N�g
    private totalGameManager totalGM;

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
    //���j���[��ʂ̃J�[�\���ɍ��킹�ē��삷�邽�߂ɒ�`����
    private PauseUIC pUC;

    //�Q�b�^�[�Z�b�^�[
    public bool MenuFlag
    {
        get {return this.menuFlag; }
        set {this.menuFlag = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        totalGM = FindObjectOfType<totalGameManager>();
    }

    // Update is called once per frame
    void Update()
    {
      
        #region//���j���[��ʂ��J������
        if (Input.GetKeyDown("q") && menuFlag == false)
        {
            pUC = FindObjectOfType<PauseUIC>();
            if (pauseUIInstance == null)
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
            //Debug.Log("�m�F:" + pUC.OpenManual);
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
        //esc�L�[�������Ƃ�
        /*
        if (Input.GetKey(KeyCode.Escape)) 
        { 
             #if UNITY_EDITOR
             UnityEditor.EditorApplication.isPlaying = false;
            //�G�f�B�^��̓���
            #else
            Application.Quit();
            //�G�f�B�^�ȊO�̑���
            #endif
            ResetCommand();
            SceneManager.LoadScene("LoadBill");
            Time.timeScale = 1f;
            menuFlag = false;
        }*/

        //tab�L�[�������Ƃ� && pUC.OpenManual == true
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            onlyFlag = true;
            //��������J���R���[�`��
            StartCoroutine("PlayerXplanation");
        }

        /*if(Input.GetKey(KeyCode.T)){
           ResetCommand();
           SceneManager.LoadScene("TitleScene");
           Time.timeScale = 1f;
           menuFlag = false;
        }*/
    }
    #endregion

    //��������̍ۂ̃R���[�`��
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
        totalGM.PlayerHp = 3;
        totalGM.PlayerIC = 0;
        totalGM.TimeCounter = false;
        /*for (int i = 0; i < tatalGM.PlayerHp; i++)
        {
            tatalGM.HeartArray[i].gameObject.SetActive(true);
        }*/
    }

}

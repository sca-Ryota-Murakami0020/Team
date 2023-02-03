using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PasueDisplayC : MonoBehaviour
{
    //�|�[�Y�֌W�̃X�v���N�g

    //�Q�[�}�l���Ăяo���Ă���
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

    //��������t���O�𗧂Ă�t���O
    private bool openManual = false;
    //�Q�[���ɖ߂铮����s���t���O
    private bool returnGame = false;

    //�Q�b�^�[�Z�b�^�[
    public bool MenuFlag
    {
        get {return this.menuFlag; }
        set {this.menuFlag = value; }
    }

    public bool OpenManual
    {
        get { return this.openManual; }
        set { this.openManual = value; }
    }

    public bool ReturnGame
    {
        get { return this.returnGame; }
        set { this.returnGame = value; }
    }

    public bool OnlyFlag
    {
        get { return this.onlyFlag;}
        set { this.onlyFlag = value;}
    }

    // Start is called before the first frame update
    void Start()
    {
        //�Q�[�}�l�Ăяo��
        totalGM = FindObjectOfType<totalGameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //�R���g���[�����͂���̓��� �c�� ���擾
        //float verticalInput = Input.GetAxis("Vertical");

        #region//���j���[��ʂ��J������

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            //�|�[�Y��ʏo��
            if (pauseUIInstance == null && menuFlag == false)
            {
                pauseUIInstance = GameObject.Instantiate(pauseUIPrefab) as GameObject;
                Time.timeScale=0f;
                menuFlag = true;
            }
            /*
            //�|�[�Y��ʂ�����
            else if(pauseUIInstance != null && returnGame == true)
            {
                menuFlag = false;
                Destroy(playOperateUIInstance);
                Destroy(pauseUIInstance);
                Time.timeScale = 1f;
                openManual = false;
                returnGame = false;
            }
            */
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
            Time.timeScale = 1f;
            menuFlag = false;
        }
    }

    //�����ʂ̌Ăяo��
    public void DisplayManual()
    {
        onlyFlag = true;
        //��������J���R���[�`��
        Destroy(pauseUIInstance);
        playOperateUIInstance = GameObject.Instantiate(playOperatePrafab) as GameObject;
        operationExpFlag = true;

    }

    //�Q�[���ɖ߂�
    public void CloseMenu()
    {
        menuFlag = false;
        Destroy(playOperateUIInstance);
        Destroy(pauseUIInstance);
        Time.timeScale = 1f;
        openManual = false;
        returnGame = false;
    }

    //���������ʂ����
    public void CloseManual()
    {
        //Tab�L�[���痣��Ă�����������UI���o�Ă�����
        //�|�[�Y���UI���o���A������UI������
        onlyFlag = false;
        operationExpFlag = false;
        openManual = false;
        Destroy(playOperateUIInstance);
        pauseUIInstance = GameObject.Instantiate(pauseUIPrefab) as GameObject;
    }
    #endregion

        /*
        //��������̍ۂ̃R���[�`��
        private IEnumerator PlayerXplanation()
        {
            while (true)
            {
                //�|�[�Y��ʂ������āA������UI���o��
                if (onlyFlag == true)
                {
                    Destroy(pauseUIInstance);
                    playOperateUIInstance = GameObject.Instantiate(playOperatePrafab) as GameObject;
                    operationExpFlag = true;
                    onlyFlag = false;
                }
                Debug.Log(playOperateUIInstance);
                //Tab�L�[���痣��Ă�����������UI���o�Ă�����
                if (operationExpFlag == true && Input.GetKeyUp (KeyCode.Tab))
                {
                    //�|�[�Y���UI���o���A������UI������
                    operationExpFlag = false;
                    openManual = false;
                    Destroy(playOperateUIInstance);
                    pauseUIInstance = GameObject.Instantiate(pauseUIPrefab) as GameObject;
                    Debug.Log(pauseUIInstance);
                    yield break;
                }
               yield return null; 
            }
        }
        */

    //���Z�b�g����Ƃ��ɒl��ς���
    private void ResetCommand()
    {
        totalGM.PlayerHp = 3;
        totalGM.PlayerIC = 0;
        totalGM.TimeCounter = false;
    }

}

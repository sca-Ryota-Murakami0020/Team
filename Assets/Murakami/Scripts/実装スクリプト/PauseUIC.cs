using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseUIC : MonoBehaviour
{
    //�\������摜�̔z��
    [SerializeField] private Image[] image;
    //PauseDisplayC
    private PasueDisplayC pDC;
    //�����ŕϐ��̒�`�E��������Ȃ����R
    //��������Pasue��ʂ̎g�p��A���̃X�N���v�g�͍폜�����̂�PauseDisplayC�̕��łňӋ`���Ă����������������

    // Start is called before the first frame update
    void Start()
    {
        pDC = FindObjectOfType<PasueDisplayC>();
        //�J�������ɂ͂����ɑ����ʂ�UI�������悤�ɂ��邽�߂̏����������ŏ���
        //�u��������v�ɍ��킹���J�[�\���̕\�����s��
        image[0].enabled = true;
        //�u�Q�[���ɖ߂�v�ɍ��킹���J�[�\���̔�\�����s��
        image[1].enabled = false;
        //�����ő�������̉�ʂ��Ăяo����悤�Ƀt���O�𗧂Ă�
        pDC.OpenManual = true;
    }

    // Update is called once per frame
    void Update()
    {
        //���j���[���J����Ă��鎞�Ɋ֐����Ăяo��
        if (pDC.MenuFlag == true)
        {
            SelectAction();
        }
    }

    public void SelectAction()
    {
        //O�L�[�iL�X�e�B�b�N����́j�ł̏���
        if (Input.GetKeyDown(KeyCode.O))
        {
            //�u��������v�ɍ��킹���J�[�\���̕\�����s��
            image[0].enabled = true;
            //�u�Q�[���ɖ߂�v�ɍ��킹���J�[�\���̔�\�����s��
            image[1].enabled = false;
            //�����ő�������̉�ʂ��Ăяo����悤�Ƀt���O�𗧂Ă�
            pDC.OpenManual = true;
            //�u�Q�[���ɖ߂�v��I��������Ɂu��������v�̃A�N�V�������s���ۂ�
            //�o�O���N���Ȃ��悤�ɂ��邽�߂ɁA������PasueDisplayC��returnGame�̒l��false�ɂ��Ă���
            pDC.ReturnGame = false;
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            //�u��������v�ɍ��킹���J�[�\���̔�\�����s��
            image[0].enabled = false;
            //�u�Q�[���ɖ߂�v�ɍ��킹���J�[�\���̕\�����s��
            image[1].enabled = true;
            //�Q�[���ɖ߂�A�N�V�������s����悤�ɂ��邽�߂̃t���O�𗧂Ă�
            pDC.ReturnGame = true;
            //�u��������v��I��������Ɂu�Q�[���ɖ߂�v�̃A�N�V�������s���ۂ�
            //�o�O���N���Ȃ��悤�ɂ��邽�߂ɁA������PasueDisplayC��openManual�̒l��false�ɂ���
            pDC.OpenManual = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCountCon : MonoBehaviour
{

    //�E��̃A�C�e���J�E���g�p�X�v���N�g

    //�e�L�X�g���Ăяo���悤�ɂ���
    private Text itemText = null;
    //�G��|�����L�^�p�ϐ�
    private float oldItemCount = 0;
    //GM�ƃv���C���[�ƃJ�����X�v���N�g�̒�`
    //private GManeger gmaneger;
    private totalGameManager totalGM;
    //�A�C�e���L�^�p�ϐ�
    private int item = 0;

    // Start is called before the first frame update
    void Start()
    {
        //GM�ƃv���C���[�ƃJ�����X�v���N�g�̌Ăяo��
        //this.gmaneger = FindObjectOfType<GManeger>();
        this.totalGM = FindObjectOfType<totalGameManager>();
        //�e�L�X�g���g����悤�ɂ���
        itemText = GetComponent<Text>();
        //GM����`����Ă�����
        if (totalGM != null)
        {
            //�e�L�X�g����ʂɏo��
            itemText.text = "�A�C�e���̐�:" + item;
        }
    }
    // Update is called once per frame
    void Update()
    {
        // //�|�����G�̐���\�L�ύX����ꍇ
        item = totalGM.PlayerIC;
        //�L�^�p�ϐ���GM���玝���Ă����l��ۑ�����ϐ��̒l���������
        if (oldItemCount != item)
        {
            //�l��ς��ăe�L�X�g����ʂɏo��
            itemText.text = "������A�C�e���̐�:" + item;
            oldItemCount = item;
        }


    }
}

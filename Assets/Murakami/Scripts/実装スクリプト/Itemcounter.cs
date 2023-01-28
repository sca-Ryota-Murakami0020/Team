using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Itemcounter : MonoBehaviour
{
    //�\������摜�̔z��
    [SerializeField] private Sprite[] numberImage;
    //�v���C���[���擾�����A�C�e���̌��B�����p���ČĂяo���摜�����ʂ���
    private int itemCon;
    //�摜��\������Image
    [SerializeField] private Image image;
    //TotalGameManager
    private totalGameManager gm;
    //SpriteRenderer sr;

    private void Awake()
    {
        gm = FindObjectOfType<totalGameManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //TotalGameManager�̃A�C�e���J�E���g���擾����
        itemCon = gm.PlayerIC;
        //TotalGameManager�����A�C�e���J�E���g�̐��l�ɉ����������̉摜���Ăяo��
        image.sprite =  numberImage[itemCon];
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SecondMImage : MonoBehaviour
{
    //TotalGameManager
    private totalGameManager gm;
    //�\������摜�̔z��
    [SerializeField] private Sprite[] numberImage;
    //�摜��\������Image
    [SerializeField] private Image image;
    //TotalGameManager����擾�����Q�[�����Ԃ�int�^�Ƃ��Ĉ�����悤�ɂ��邽�߂̕ϐ�
    private int SecondMCount;

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<totalGameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //TotalGameManager����擾�����Q�[�����Ԃ�int�^�ɕϊ�
        SecondMCount = Mathf.FloorToInt(gm.TotalTime / 600);
        //�Q�[�����Ԃ�10�b�P�ʂ̕\�����s��(0�`5)
        image.sprite = numberImage[SecondMCount];
    }
}

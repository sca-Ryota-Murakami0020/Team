using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class firstMImage : MonoBehaviour
{
    //TotalGameManager
    private totalGameManager gm;
    //�\������摜�̔z��
    [SerializeField] private Sprite[] numberImage;
    //�摜��\������Image
    [SerializeField] private Image image;
    //TotalGameManager����擾�����Q�[�����Ԃ�int�^�ɂ��Ď擾����ϐ�
    private int firstMCount;

    void Start()
    {
        gm = FindObjectOfType<totalGameManager>();
    }

    void Update()
    {
        //TotalGameManager����擾�����Q�[�����Ԃ�int�^�ɕϊ�
        firstMCount = Mathf.FloorToInt(gm.TotalTime / 60);
        //�Q�[�����Ԃ�1���P�ʂ̕\�����s��(0�`9)
        image.sprite = numberImage[firstMCount % 10];
    }
}

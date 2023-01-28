using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//0�`9�܂ł̈ꌅ�܂ł�Image�̕\��
public class FirstSecondTimeImage : MonoBehaviour
{
    //TotalGameManger
    private totalGameManager gm;
    //�\������摜�̔z��
    [SerializeField] private Sprite[] numberImage;
    //�摜��\������Image
    [SerializeField] private Image image;
    //TotalGameManager����擾�����Q�[�����Ԃ�int�^�Ƃ��Ĉ�����悤�ɂ��邽�߂̕ϐ�
    private int firstSCount;

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<totalGameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //TotalGameManager����擾�����Q�[�����Ԃ�int�^�ɕϊ�
        firstSCount = Mathf.FloorToInt(gm.TotalTime % 10);
        //�Q�[�����Ԃ�1�b�P�ʂ̕\�����s��(0�`9)
        image.sprite = numberImage[firstSCount];
    }
}

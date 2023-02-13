using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPlayerposition : MonoBehaviour
{
    //�v���C���[���߂�ʒu�̊�_�ɂȂ�I�u�W�F�N�g
    [SerializeField] private GameObject resetPosition;
    private Player pl;

    void Start()
    {
        pl = FindObjectOfType<Player>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            //�v���C���[���r���X�e�[�W�̃R���N���[�g�I�u�W�F�N�g�ɐG�ꂽ��
            //�w��̈ʒu�ɐݒu���Ă���I�u�W�F�N�g�̈ʒu�Ɉړ�������
            pl.transform.position = resetPosition.transform.position;
        }
    }
}

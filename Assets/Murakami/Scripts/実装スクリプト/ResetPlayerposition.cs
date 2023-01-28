using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPlayerposition : MonoBehaviour
{
    //�v���C���[���߂�ʒu�̊�_�ɂȂ�I�u�W�F�N�g
    [SerializeField] private GameObject resetPosition;
    private Player pl;

    // Start is called before the first frame update
    void Start()
    {
        pl = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
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

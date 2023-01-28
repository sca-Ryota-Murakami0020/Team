using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallCon : MonoBehaviour
{
    //���̐e�̃I�u�W�F�N�g
    private GameObject parent;

    //�W�����v�n�_�ɐG�ꂽ���̃t���O
    private bool wallJumpHitFlag = false;
    
    //�Q�b�^�[�Z�b�^�[
    public bool WallJumpHitFlag
    {
        get { return this.wallJumpHitFlag; }
        set { this.wallJumpHitFlag = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        //�e�̃Q�[���I�u�W�F�N�g�擾(�O�̂���)
        parent = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        //�ǂ͂���ł���n�_�ɐG�ꂽ��
        if (other.gameObject.CompareTag("WallJumpPoint"))
        {
            //�t���O�𗧂Ă�
            wallJumpHitFlag = true;
        }
    }
}

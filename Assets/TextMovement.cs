using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextMovement : MonoBehaviour
{
    [SerializeField] Text[] text = new Text[3];
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //�V�[���������ɂ����Ă�����
        if (false)
        {
            StartCoroutine("Movement");
        }
    }

    private IEnumerator Movement()
    {
        for(int i = 0; i < 3; i++)
        {
            Vector3 pos = text[i].transform.position;
            //text[i].transform.position���E���獶��
            while (true)
            {
                //�������W������
                pos.x -= 10.0f;
                //���[�ɒB������break
                if(pos.x <= 350)
                {
                    break;
                }
                yield return null;//1F�҂�
            }
            yield return new WaitForSeconds(1);
        }
    }
}

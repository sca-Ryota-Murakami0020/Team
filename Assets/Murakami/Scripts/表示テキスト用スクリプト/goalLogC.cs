using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class goalLogC : MonoBehaviour
{
    //TimeTextC
    private TimeTextC tTC;
    //�g�傷��摜
    [SerializeField] private RectTransform goalImage;

    // Start is called before the first frame update
    void Start()
    {
        tTC = FindObjectOfType<TimeTextC>();
        goalImage = gameObject.GetComponent<RectTransform>();
        //�摜�̑傫����0�ɂ���
        goalImage.transform.localScale = new Vector3(0,0,0);       
    }

    public void SrartGoalCorotine()
    {
        //�R���[�`���J�n
        StartCoroutine("UpScale");
    }

    //�Q�[���N���A�̃��S���g�傷��
    private IEnumerator UpScale()
    {
        //���̑傫���܂Ŋg�傷��
        while(this.transform.localScale.x >= 1000 && this.transform.localScale.y >= 300)
        {
            goalImage.transform.localScale += new Vector3(10,3,0);
            yield return new WaitForSeconds(0.1f);
        }
        yield break;
    }
}

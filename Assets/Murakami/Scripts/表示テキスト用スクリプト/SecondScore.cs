using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SecondScore : MonoBehaviour
{
    private totalGameManager totalGM;
    [SerializeField] Text secondTimeText;
    //private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        totalGM = FindObjectOfType<totalGameManager>();
        for (int i = 0; i <= 2; i++)
        {
            secondTimeText.text = "";
        }
        //anim = this.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(olt.OldSecondTime);
        //2�ʃX�R�A�̕\�L
            if (totalGM.TimeText[1] == "1")
            {
                //�Q�ʂ̃f�t�H���g�\��
                secondTimeText.text = "2��:00:00";
            }
            else
            {
                //�Q�ʂ̃����L���O�\��
                secondTimeText.text = totalGM.TimeText[1];
            //if(olt.LoadCout > 3)
                //Debug.Log("text2 " + olt.TimeText[1]);
            }
        //anim.SetBool("setSecondScore", true);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TherdScore : MonoBehaviour
{
    private totalGameManager totalGM;
    [SerializeField] Text therdTimeText;
    //private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        totalGM = FindObjectOfType<totalGameManager>();
        for (int i = 0; i <= 2; i++)
        {
            therdTimeText.text = "";
        }
        //anim = this.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(olt.OldSecondTime);
        //2�ʃX�R�A�̕\�L


        if (totalGM.TimeText[2] == "2")
        {
            //�R�ʂ̃f�t�H���g�\��"
            therdTimeText.text = "3��:00:00";
        }
        else
        {
            //�R�ʂ̃����L���O�\��"
            therdTimeText.text = totalGM.TimeText[2];
            //if(olt.LoadCout > 3)
                //Debug.Log("text3 " + olt.TimeText[2]);
        }
    }
}

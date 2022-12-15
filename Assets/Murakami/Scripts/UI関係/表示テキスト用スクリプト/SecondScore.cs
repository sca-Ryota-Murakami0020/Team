using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SecondScore : MonoBehaviour
{
    private OverLoadTimer olt;
    [SerializeField] Text secondTimeText;

    // Start is called before the first frame update
    void Start()
    {
        olt = FindObjectOfType<OverLoadTimer>();
        for (int i = 0; i <= 2; i++)
        {
            secondTimeText.text = "";
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(olt.OldSecondTime);
        //2�ʃX�R�A�̕\�L
            if (olt.TimeText[1] == "1")
            {
                //�Q�ʂ̃f�t�H���g�\��
                secondTimeText.text = "2��:00:00:00";
            }
            else
            {
                //�Q�ʂ̃����L���O�\��
                secondTimeText.text = olt.TimeText[1];
            //if(olt.LoadCout > 3)
                //Debug.Log("text2 " + olt.TimeText[1]);
            }
    }
}

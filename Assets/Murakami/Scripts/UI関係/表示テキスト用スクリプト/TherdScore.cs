using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TherdScore : MonoBehaviour
{
    private OverLoadTimer olt;
    [SerializeField] Text therdTimeText;

    // Start is called before the first frame update
    void Start()
    {
        olt = FindObjectOfType<OverLoadTimer>();
        for (int i = 0; i <= 2; i++)
        {
            therdTimeText.text = "";
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(olt.OldSecondTime);
        //2�ʃX�R�A�̕\�L


        if (olt.TimeText[2] == "2")
        {
            //�R�ʂ̃f�t�H���g�\��"
            therdTimeText.text = "3��:00:00:00";
        }
        else
        {
            //�R�ʂ̃����L���O�\��"
            therdTimeText.text = olt.TimeText[2];
        }

    }
}

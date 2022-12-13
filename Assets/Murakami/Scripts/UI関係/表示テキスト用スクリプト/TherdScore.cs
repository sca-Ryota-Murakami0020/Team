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
        //2位スコアの表記


        if (olt.TimeText[2] == "2")
        {
            //３位のデフォルト表示"
            therdTimeText.text = "3位:00:00:00";
        }
        else
        {
            //３位のランキング表示"
            therdTimeText.text = olt.TimeText[2];
        }

    }
}

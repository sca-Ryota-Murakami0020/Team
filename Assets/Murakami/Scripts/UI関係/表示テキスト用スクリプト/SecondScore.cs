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
        //2位スコアの表記


            if (olt.TimeText[1] == "1")
            {
                Debug.Log("２位のデフォルト表示");
                secondTimeText.text = "2位:0:00:00";
            }
            else
            {
                Debug.Log("２位のランキング表示");
                secondTimeText.text = olt.TimeText[1];
            }

    }
}

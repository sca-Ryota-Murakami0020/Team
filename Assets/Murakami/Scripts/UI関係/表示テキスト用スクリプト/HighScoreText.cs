using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreText : MonoBehaviour
{
    private OverLoadTimer olt;
    [SerializeField] Text bestTimeText;

    // Start is called before the first frame update
    void Start()
    {
        olt = FindObjectOfType<OverLoadTimer>();
        for (int i = 0; i <= 2; i++)
        {
            bestTimeText.text = "";
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(olt.OldSecondTime);
        //ハイスコアの表記

            if(olt.TimeText[0] == "0") {
                Debug.Log("１位のデフォルト表示");
                bestTimeText.text ="1位:" + "0:00:00";
            }
            else {
                Debug.Log("１位のランキング表示");
                bestTimeText.text  = olt.TimeText[0];
            }

    }


}

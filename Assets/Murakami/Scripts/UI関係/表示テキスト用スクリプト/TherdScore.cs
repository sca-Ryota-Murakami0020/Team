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
        //2位スコアの表記


        if (totalGM.TimeText[2] == "2")
        {
            //３位のデフォルト表示"
            therdTimeText.text = "3位:00:00:00";
        }
        else
        {
            //３位のランキング表示"
            therdTimeText.text = totalGM.TimeText[2];
            //if(olt.LoadCout > 3)
                //Debug.Log("text3 " + olt.TimeText[2]);
        }
    }
}

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
        //2位スコアの表記
            if (totalGM.TimeText[1] == "1")
            {
                //２位のデフォルト表示
                secondTimeText.text = "2位:00:00";
            }
            else
            {
                //２位のランキング表示
                secondTimeText.text = totalGM.TimeText[1];
            //if(olt.LoadCout > 3)
                //Debug.Log("text2 " + olt.TimeText[1]);
            }
        //anim.SetBool("setSecondScore", true);
    }
}

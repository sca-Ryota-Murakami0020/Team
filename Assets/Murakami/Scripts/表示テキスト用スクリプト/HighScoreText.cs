using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreText : MonoBehaviour
{
    private totalGameManager totalGM;
    [SerializeField] Text bestTimeText;
    //private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        totalGM = FindObjectOfType<totalGameManager>();
        for (int i = 0; i <= 2; i++)
        {
            bestTimeText.text = "";
        }
        //anim = this.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(olt.OldSecondTime);
        //ハイスコアの表記

            if(totalGM.TimeText[0] == "0") {
                //１位のデフォルト表示"
                bestTimeText.text ="1位:00:00:00";
            }
            else {
                //１位のランキング表示"
                bestTimeText.text  = totalGM.TimeText[0];
                /*if(olt.LoadCout > 3)
                {
                    Debug.Log("text1 " + olt.TimeText[0]);
                }*/              
            }
        //anim.SetBool("setHighScore", true);
    }


}

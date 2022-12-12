using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreText : MonoBehaviour
{
    private OverLoadTimer olt;
    [SerializeField] Text[] bestTimeText;

    // Start is called before the first frame update
    void Start()
    {
        olt = FindObjectOfType<OverLoadTimer>();
        for (int i = 0; i <= 2; i++)
        {
            bestTimeText[i].text = "";
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(olt.OldSecondTime);
        //�n�C�X�R�A�̕\�L


        for (int i = 0; i <= 2; i++)
        {
            
            if(olt.TimeText[i] == null) {
                bestTimeText[i].text = i + 1 + "��:" + "0:00:00";
            }
            else {
                bestTimeText[i].text  = olt.TimeText[i];
            }
            
        }
    }


}

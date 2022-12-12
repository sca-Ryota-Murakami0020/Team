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
        //�n�C�X�R�A�̕\�L

            if(olt.TimeText[0] == "0") {
                Debug.Log("�P�ʂ̃f�t�H���g�\��");
                bestTimeText.text ="1��:" + "0:00:00";
            }
            else {
                Debug.Log("�P�ʂ̃����L���O�\��");
                bestTimeText.text  = olt.TimeText[0];
            }

    }


}

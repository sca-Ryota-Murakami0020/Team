using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryButtonC : MonoBehaviour
{
    private OverLoadTimer olt;

    // Start is called before the first frame update
    void Start()
    {
        olt = FindObjectOfType<OverLoadTimer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RetryGame()
    {
        olt.TotalTime =0.0f;
        /*
        olt.SecondTime = 0.0f;
        olt.MinuteTime = 0;
        olt.HourTime = 0;
        */
        SceneManager.LoadScene("LoadBill");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayExplanation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //適当にボタンが押された時
        if (Input.anyKey)
        {
            SceneManager.LoadScene("LoadBill");
        }
    }

    public void NextStege()
    {
        
    }
}

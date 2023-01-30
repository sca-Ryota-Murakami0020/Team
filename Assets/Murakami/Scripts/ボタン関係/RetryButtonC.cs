using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryButtonC : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //ƒQ[ƒ€‚ğƒŠƒgƒ‰ƒC‚·‚é
    public void RetryGame()
    {
        SceneManager.LoadScene("LoadBill");
    }
}

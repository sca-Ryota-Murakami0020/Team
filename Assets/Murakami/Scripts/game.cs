using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class game : MonoBehaviour
{
    //private OverLoadTimer olt;
    // Start is called before the first frame update
    void Start()
    {
        //olt = GetComponent<OverLoadTimer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Game()
    {
        //if(olt.LoadCout >= 3) olt.BestTime[3] = 0.0f;
        SceneManager.LoadScene("ë∫è„óp");
    }
}

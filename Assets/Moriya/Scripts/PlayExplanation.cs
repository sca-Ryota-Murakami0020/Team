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
        //“K“–‚Éƒ{ƒ^ƒ“‚ª‰Ÿ‚³‚ê‚½Žž
        if (Input.anyKey)
        {
            SceneManager.LoadScene("LoadBill");
        }
    }

    public void NextStege()
    {
        
    }
}

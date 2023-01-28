using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleButtonC : MonoBehaviour
{
    //TotalGameManager
    private totalGameManager tGM;

    private void Awake()
    {
        tGM = FindObjectOfType<totalGameManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //ƒ^ƒCƒgƒ‹‚É–ß‚é
    public void GotoTitle()
    {
        SceneManager.LoadScene("AnyLoadTitle");
        tGM.TotalTime = 0.0f;
    }
}

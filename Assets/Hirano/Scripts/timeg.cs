using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class timeg : MonoBehaviour
{
    public Image maxGaze;
    private float nowGaze = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        nowGaze += Time.deltaTime;
        maxGaze.fillAmount = nowGaze / 3.0f;
        if ((float)nowGaze % 3.0f <= 0)
        {
            SceneManager.LoadSceneAsync("FirstScene");
        }
    }
}

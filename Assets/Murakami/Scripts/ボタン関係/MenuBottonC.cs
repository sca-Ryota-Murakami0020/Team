using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBottonC : MonoBehaviour
{
    private PasueDisplayC pDC;

    // Start is called before the first frame update
    void Start()
    {
        pDC = FindObjectOfType<PasueDisplayC>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnManual()
    {
        //ëÄçÏâÊñ ÇÃäJé¶
        pDC.DisplayManual();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnGameButtonC : MonoBehaviour
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

    public void ReturnGame()
    {
        //É|Å[ÉYâÊñ Çï¬Ç∂ÇÈ
        pDC.CloseMenu();
    }
}

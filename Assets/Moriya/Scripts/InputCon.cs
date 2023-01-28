using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputCon : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //L Stick
        float lsh = Input.GetAxis("Horizontal");
        float lsv = Input.GetAxis("Vertical");
        if ((lsh != 0) || (lsv != 0))
        {
            Debug.Log("L stick:" + lsh + "," + lsv);
        }
        //R Stick
        float rsh = Input.GetAxis("Mouse X");
        float rsv = Input.GetAxis("Mouse Y");
        if ((rsh != 0) || (rsv != 0))
        {
            Debug.Log("R stick:" + rsh + "," + rsv);
        }
        //D-Pad
        float dph = Input.GetAxis("D_Pad_H");
        float dpv = Input.GetAxis("D_Pad_V");
        if ((dph != 0) || (dpv != 0))
        {
            Debug.Log("D Pad:" + dph + "," + dpv);
        }
        //Trigger
        float tri = Input.GetAxis("L_R_Trigger");
        if (tri > 0)
        {
            Debug.Log("L trigger:" + tri);
        }
        else if (tri < 0)
        {
            Debug.Log("R trigger:" + tri);
        }
        else
        {
            Debug.Log("  trigger:none");
        }
    }
}

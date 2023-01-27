using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class goalLogC : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.transform.localScale = new Vector3(0,0,0);
        StartCoroutine("UpScale");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator UpScale()
    {
        while(this.transform.localScale.x >= 1000 && this.transform.localScale.y >= 300)
        {
            this.transform.localScale += new Vector3(10,3,0);
            yield return new WaitForSeconds(0.1f);
        }
        yield break;
    }
}

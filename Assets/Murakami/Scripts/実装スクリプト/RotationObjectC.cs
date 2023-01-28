using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationObjectC : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("RotationObject");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator RotationObject()
    {
        this.transform.Rotate(0, 1.0f, 0);
        yield return new WaitForSeconds(0.1f);
    }
}

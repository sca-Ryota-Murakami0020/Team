using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationObjectC : MonoBehaviour
{
    private float countTime = 0.0f;
    private Vector3 pos;

    // Start is called before the first frame update
    void Start()
    {
        pos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        countTime += 0.01f;
        this.transform.position = new Vector3(pos.x, pos.y, pos.z + Mathf.Sin(countTime) * 0.07f);
        this.transform.Rotate(0, 0, 0.05f);
    }
}

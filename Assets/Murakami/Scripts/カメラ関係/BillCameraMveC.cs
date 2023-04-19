using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillCameraMveC : MonoBehaviour
{
    //BillCameraC
    private BillCameraC bC;
    //要素数
    private int limitNumber = 0;
    //
    [SerializeField] private GameObject[] returnObject;

    void Start()
    {
        bC = FindObjectOfType<BillCameraC>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            for(int i = 0; i <= returnObject.Length; i++)
            {
                var cameraNumber = other.gameObject;
                if ((cameraNumber != bC.OldCamera) && i == returnObject.Length)
                {
                    //次の面に移る
                    bC.NextCamera();
                }

                if (returnObject[i] == bC.OldCamera || cameraNumber == bC.OldCamera)
                {
                    //前の面に移る
                    bC.BackCamera();
                    break;
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hp : MonoBehaviour
{
    public GameObject[] heartArray = new GameObject[3];
    private int heartCount;

    void Start()
    {
        heartCount = 3;
    }

    void Update()
    {
        //���N���b�N ���C�t�|�C���g���炷
        if (Input.GetMouseButtonDown(0))
        {
            if (heartCount > 0)
            {
                heartCount--;
            }
        }

        //�E�N���b�N ���C�t�|�C���g���₷
        if (Input.GetMouseButtonDown(1))
        {
            if (heartCount < 3)
            {
                heartCount++;
            }
        }

        if (heartCount == 3)
        {
            heartArray[2].gameObject.SetActive(true);
            heartArray[1].gameObject.SetActive(true);
            heartArray[0].gameObject.SetActive(true);
        }

        if (heartCount == 2)
        {
            heartArray[2].gameObject.SetActive(false);
            heartArray[1].gameObject.SetActive(true);
            heartArray[0].gameObject.SetActive(true);
        }
        if (heartCount == 1)
        {
            heartArray[2].gameObject.SetActive(false);
            heartArray[1].gameObject.SetActive(false);
            heartArray[0].gameObject.SetActive(true);
        }

        if (heartCount == 0)
        {
            heartArray[2].gameObject.SetActive(false);
            heartArray[1].gameObject.SetActive(false);
            heartArray[0].gameObject.SetActive(false);
        }
    }
}

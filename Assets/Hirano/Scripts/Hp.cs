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
        //左クリック ライフポイント減らす
        if (Input.GetMouseButtonDown(0))
        {
            if (heartCount > 0)
            {
                heartCount--;
            }
        }

        //右クリック ライフポイント増やす
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SecondMImage : MonoBehaviour
{
    [SerializeField] private Sprite[] numberImage;
    private int SecondMCount;
    [SerializeField] private Image image;
    private totalGameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<totalGameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        SecondMCount = Mathf.FloorToInt(gm.TotalTime / 600);
        image.sprite = numberImage[SecondMCount];
    }
}

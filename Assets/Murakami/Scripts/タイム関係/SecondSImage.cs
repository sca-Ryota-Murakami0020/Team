using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SecondSImage : MonoBehaviour
{
    [SerializeField] private Sprite[] numberImage;
    private int secondSCount;
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
        secondSCount = Mathf.FloorToInt(gm.TotalTime % 10);
        image.sprite = numberImage[secondSCount];
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//0〜9までの一桁までのImageの表示
public class FirstSecondTimeImage : MonoBehaviour
{

    [SerializeField] private Sprite[] numberImage;
    private int firstSCount;
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
        firstSCount = Mathf.FloorToInt(gm.TotalTime % 10);
        image.sprite = numberImage[firstSCount];
    }
}
